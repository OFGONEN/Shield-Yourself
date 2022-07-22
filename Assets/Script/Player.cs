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
    [ SerializeField ] GameEvent event_shield_activate;

// Private
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
	}

    public void OnShieldActivate()
    {
		//todo left arm
		onUpdateMethod = PlayerBlocking;
	}
#endregion

#region Implementation
    void PlayerWalking()
    {

    }

    void PlayerBlocking()
    {

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

    void EmptyDelegates()
    {
		onFingerDown   = ExtensionMethods.EmptyMethod;
		onFingerUp     = ExtensionMethods.EmptyMethod;
		onUpdateMethod = ExtensionMethods.EmptyMethod;
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}