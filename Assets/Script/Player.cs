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
    [ SerializeField ] GameEvent event_shield_activate;

  [ Title( "Shared Variables" ) ]
    [ SerializeField ] IncrementalHealth incremental_health;
    [ SerializeField ] IncrementalStamina incremental_stamina;
    [ SerializeField ] IncrementalCurrency incremental_currency;
// Private

    IncrementalHealthData incremental_health_data;
    IncrementalStaminaData incremental_stamina_data;
    IncrementalCurrecyData incremental_currency_data;

    UnityMessage onFingerDown;
    UnityMessage onFingerUp;
    UnityMessage onUpdateMethod;

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
        // Set incremental properties to default values
		player_stamina.Default();
		player_health.sharedValue = incremental_health.CurrentIncremental.incremental_health_value;
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

		CacheIncrementals();
	}

    public void OnIncrementalUnlocked()
    {
		CacheIncrementals();
	}

    public void OnShieldActivate()
    {
		onUpdateMethod = PlayerBlocking;
	}

    [ Button() ]
    public void OnArrowHit()
    {
		player_health.SharedValue -= GameSettings.Instance.player_arrow_damage;

        if( player_health.sharedValue <= 0 )
			Die();
	}
#endregion

#region Implementation
    void PlayerWalking()
    {
		player_currency.Gain( incremental_currency_data.incremental_currency_gain_value, incremental_currency_data.incremental_currency_gain_rate );
		player_stamina.Recover( incremental_stamina_data.incremental_stamina_recover );
	}

    void PlayerBlocking()
    {
		//todo left arm
		player_stamina.Deplete( incremental_stamina_data.incremental_stamina_deplete, incremental_stamina_data.incremental_stamina_deplete_capacity );

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
        
        // todo set player speed = 0
        // todo do blocking animation
	}

    void FingerUp()
    {
		onFingerDown   = FingerDown;
		onUpdateMethod = PlayerWalking;
		onFingerUp     = ExtensionMethods.EmptyMethod;

		recycledTween.Kill();

        // todo set player speed = GameSetting
        // todo do walk animation
	}

    void CacheIncrementals()
    {
		incremental_health_data   = incremental_health.CacheCurrentIncremental();
		incremental_stamina_data  = incremental_stamina.CacheCurrentIncremental();
		incremental_currency_data = incremental_currency.CacheCurrentIncremental();
    }

    void EmptyDelegates()
    {
		onFingerDown   = ExtensionMethods.EmptyMethod;
		onFingerUp     = ExtensionMethods.EmptyMethod;
		onUpdateMethod = ExtensionMethods.EmptyMethod;
    }

    void Die()
    {
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}