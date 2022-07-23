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
    [ SerializeField ] SharedBoolNotifier player_is_blocking;
    [ SerializeField ] SharedReferenceNotifier shield_arm_target;
    [ SerializeField ] GameEvent event_shield_activate;
    [ SerializeField ] GameEvent event_shield_deactivate;

  [ Title( "Components" ) ]
    [ SerializeField ] Animator player_animator;

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
    UnityMessage onAnimatorIKUpdate;

    RecycledTween recycledTween = new RecycledTween();
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

	void OnAnimatorIK( int layerIndex )
	{
		onAnimatorIKUpdate();
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

		shield_arm_target_transform = shield_arm_target.sharedValue as Transform;

		player_animator.SetBool( "walking", true );
		player_speed.SharedValue = GameSettings.Instance.player_speed;
	}

    public void OnIncrementalUnlocked()
    {
		CacheIncrementals();

		SetHealthRatio();
	}

    public void OnShieldActivate()
    {
		onUpdateMethod     = PlayerBlocking;
		onAnimatorIKUpdate = PositionLeftArm;
		player_animator.SetIKPositionWeight( AvatarIKGoal.LeftHand, 1 );

		player_is_blocking.SharedValue = true;
	}

    [ Button() ]
    public void OnArrowHit()
    {
		player_health.SharedValue -= GameSettings.Instance.player_arrow_damage;
		SetHealthRatio();

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

        if( player_stamina.sharedValue <= 0 )
			Die();
	}

    void FingerDown()
    {
		onFingerDown = ExtensionMethods.EmptyMethod;
		onFingerUp   = FingerUp;

		recycledTween.Recycle( DOVirtual.DelayedCall(
			GameSettings.Instance.player_shield_activate_delay,
			event_shield_activate.Raise ) );

		player_speed.SharedValue = 0;
		player_animator.SetBool( "walking", false );
	}

    void FingerUp()
    {
		onFingerDown       = FingerDown;
		onUpdateMethod     = PlayerWalking;
		onFingerUp         = ExtensionMethods.EmptyMethod;
		onAnimatorIKUpdate = ExtensionMethods.EmptyMethod;

		recycledTween.Kill();

		player_speed.SharedValue = GameSettings.Instance.player_speed;

		player_animator.SetBool( "walking", true );

		player_animator.SetIKPositionWeight( AvatarIKGoal.LeftHand, 0 );

		player_is_blocking.SharedValue = false;

		event_shield_deactivate.Raise();
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
		onAnimatorIKUpdate = ExtensionMethods.EmptyMethod;
	}

    void Die()
    {
		player_animator.SetTrigger( "die" );
	}

	void SetHealthRatio()
	{
		player_health_ratio.SharedValue = player_health.sharedValue / incremental_health_data.incremental_health_value;
	}

	void PositionLeftArm()
	{
		player_animator.SetIKPosition( AvatarIKGoal.LeftHand, shield_arm_target_transform.position );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}