/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] Stamina player_stamina;
    [ SerializeField ] Currency player_currency;
    [ SerializeField ] SharedFloatNotifier player_health;
    [ SerializeField ] SharedFloatNotifier player_health_ratio;
    [ SerializeField ] SharedFloatNotifier player_speed;
    [ SerializeField ] SharedFloatNotifier player_leftArm_weight;
    [ SerializeField ] SharedBoolNotifier player_is_blocking;
    [ SerializeField ] SharedReferenceNotifier player_leftArm_target;
    [ SerializeField ] GameEvent event_shield_activate;
    [ SerializeField ] GameEvent event_shield_deactivate;
    [ SerializeField ] ParticleSpawnEventCoolDown player_particle_spawn;

  [ Title( "Components" ) ]
    [ SerializeField ] Animator player_animator;
    [ SerializeField ] Transform player_shield_transform;

  [ Title( "Incrementals" ) ]
    [ SerializeField ] IncrementalHealth incremental_health;
    [ SerializeField ] IncrementalStamina incremental_stamina;
    [ SerializeField ] IncrementalCurrency incremental_currency;
// Private
	Transform shield_arm_target_transform;

    IncrementalHealthData incremental_health_data;
    IncrementalStaminaData incremental_stamina_data;
    IncrementalCurrecyData incremental_currency_data;

    UnityMessage onFingerDown;
    UnityMessage onFingerUp;
    UnityMessage onUpdateMethod;

    RecycledTween recycledTween = new RecycledTween();
    RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
    private void OnDisable()
    {
		EmptyDelegates();
	}

    private void Awake()
    {
		EmptyDelegates();
	}

    private void Start()
    {
		CacheIncrementals();

		// Set incremental properties to default values
		player_is_blocking.SharedValue = false;
		player_stamina.Default();
		player_health.sharedValue = incremental_health.CurrentIncremental.incremental_health_value;
		player_health_ratio.sharedValue = 1;
	}

    private void Update()
    {
		onUpdateMethod();
	}
#endregion

#region API
    public void OnFingerDown()
    {
		onFingerDown();
	}

    public void OnFingerUp()
    {
		onFingerUp();
	}

    public void OnLevelStarted()
    {
		onFingerDown   = FingerDown;
		onUpdateMethod = PlayerWalking;

		player_animator.SetBool( "walking", true );
		player_speed.SharedValue = GameSettings.Instance.player_speed;

		shield_arm_target_transform = player_leftArm_target.SharedValue as Transform;
	}

    public void OnIncrementalUnlocked()
    {
		CacheIncrementals();

		SetHealthRatio();
	}

    public void OnShieldActivate()
    {
		onUpdateMethod = PlayerBlocking;

		player_is_blocking.SharedValue    = true;
		//todo player_leftArm_weight.SharedValue = 1;
	}

    [ Button() ]
    public void OnArrowHit( Collider collider )
    {
		player_health.SharedValue -= GameSettings.Instance.player_arrow_damage;
		SetHealthRatio();

		player_particle_spawn.Raise( "hit_player", collider.transform.position );

		if( player_health.sharedValue <= 0 )
			Die();
	}
#endregion

#region Implementation
    void PlayerWalking()
    {
		player_currency.Gain( incremental_currency_data.incremental_currency_gain_value, incremental_currency_data.incremental_currency_gain_rate );
		player_stamina.Recover( incremental_stamina_data.incremental_stamina_recover * Time.deltaTime );
	}

    void PlayerBlocking()
    {
		player_stamina.Deplete( incremental_stamina_data.incremental_stamina_deplete * Time.deltaTime, incremental_stamina_data.incremental_stamina_deplete_capacity * Time.deltaTime );

		player_shield_transform.rotation = shield_arm_target_transform.rotation;

		if( player_stamina.sharedValue <= 0 )
			Die();
	}

    void FingerDown()
    {
		onFingerDown = ExtensionMethods.EmptyMethod;
		onFingerUp   = FingerUp;

		var shieldActivateDelay = GameSettings.Instance.player_shield_activate_delay;

		recycledTween.Recycle( DOVirtual.DelayedCall(
			shieldActivateDelay,
			event_shield_activate.Raise ) );

		player_speed.SharedValue = 0;
		player_animator.SetBool( "walking", false );

		if( recycledSequence.IsPlaying() )
			shieldActivateDelay -= recycledSequence.Sequence.Elapsed();

		recycledSequence.Kill();

		var sequence = recycledSequence.Recycle();
		sequence.Append( DOTween.To( GetLeftArmWeight, SetLeftArmWeight, 1, shieldActivateDelay ) );
		sequence.Join( player_shield_transform.DORotate( shield_arm_target_transform.eulerAngles, shieldActivateDelay ) );
	}

    void FingerUp()
    {
		onFingerDown       = FingerDown;
		onUpdateMethod     = PlayerWalking;
		onFingerUp         = ExtensionMethods.EmptyMethod;

		recycledTween.Kill();

		var newDuration = GameSettings.Instance.player_shield_activate_delay;

		if( recycledSequence.IsPlaying() )
			newDuration -= recycledSequence.Sequence.Elapsed();

		recycledSequence.Kill();
		var sequence = recycledSequence.Recycle();
		sequence.Append( DOTween.To( GetLeftArmWeight, SetLeftArmWeight, 0, newDuration ) );
		sequence.Join( player_shield_transform.DOLocalRotate( Vector3.zero, newDuration ) );

		player_speed.SharedValue = GameSettings.Instance.player_speed;

		player_animator.SetBool( "walking", true );

		player_is_blocking.SharedValue = false;
		event_shield_deactivate.Raise();

		player_shield_transform.localRotation = Quaternion.identity;
	}

    void CacheIncrementals()
    {
		incremental_health_data   = incremental_health.CacheCurrentIncremental();
		incremental_stamina_data  = incremental_stamina.CacheCurrentIncremental();
		incremental_currency_data = incremental_currency.CacheCurrentIncremental();

		// Conver durations to speed
		incremental_stamina_data.incremental_stamina_deplete          = 1 / incremental_stamina_data.incremental_stamina_deplete;
		incremental_stamina_data.incremental_stamina_deplete_capacity = 1 / incremental_stamina_data.incremental_stamina_deplete_capacity;
		incremental_stamina_data.incremental_stamina_recover          = 1 / incremental_stamina_data.incremental_stamina_recover;
	}

    void EmptyDelegates()
    {
		onFingerDown       = ExtensionMethods.EmptyMethod;
		onFingerUp         = ExtensionMethods.EmptyMethod;
		onUpdateMethod     = ExtensionMethods.EmptyMethod;
	}

    void Die()
    {
		player_animator.SetTrigger( "die" );

		event_shield_deactivate.Raise();
		EmptyDelegates();
	}

	void SetHealthRatio()
	{
		player_health_ratio.SharedValue = player_health.sharedValue / incremental_health_data.incremental_health_value;
	}

	float GetLeftArmWeight()
	{
		return player_leftArm_weight.sharedValue;
	}

	void SetLeftArmWeight( float value )
	{
		player_leftArm_weight.SharedValue = value;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}