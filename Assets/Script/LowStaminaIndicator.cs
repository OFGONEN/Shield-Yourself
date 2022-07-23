/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class LowStaminaIndicator : MonoBehaviour
{
#region Fields
    [ SerializeField ] Stamina player_stamina;
    [ SerializeField ] SharedFloatNotifier player_redness;
    [ SerializeField ] SharedFloatNotifier player_inflation;
    [ SerializeField ] SharedBoolNotifier player_is_blocking;
    [ SerializeField ] SharedFloatNotifier notifier_vignette_intencity;
    [ SerializeField ] ParticleSystem pfx_sweat;
    
    int direction = +1;

	RecycledTween recycledTween = new RecycledTween();
#endregion

#region Properties
#endregion

#region Unity API
    void Awake()
    {
		player_inflation.SetValue_NotifyAlways( 0 );
		player_redness.SetValue_NotifyAlways( 1 );
	}
    
    void Update()
    {		
		var lowStaminaCofactor  = DetermineLowStaminaCofactor();
		var targetColorRatio    = Mathf.InverseLerp( 0, GameSettings.Instance.player_stamina_threshold.value, lowStaminaCofactor );
		
		if( targetColorRatio < 0.99f )
		{
			pfx_sweat.Play( true );
			UpdateCharacterInflation();

			recycledTween.Kill();
			notifier_vignette_intencity.SharedValue = GameSettings.Instance.player_stamina_vignette_range.ReturnProgressInverse( lowStaminaCofactor );
		}
		else
		{
			pfx_sweat.Stop( true, ParticleSystemStopBehavior.StopEmitting );
			player_inflation.SharedValue = 0.0f;

			if( !recycledTween.IsPlaying() )
				recycledTween.Recycle( DOTween.To( GetVignette, SetVignette, 0, GameSettings.Instance.player_stamina_vignette_falloff_duration ).SetEase( Ease.Linear ) );
		}

		// var targetColorRatio_eased = DOVirtual.EasedValue( 0, 1, targetColorRatio, Ease.OutCubic );
		player_redness.SharedValue = Mathf.MoveTowards( player_redness.SharedValue, targetColorRatio,
															  GameSettings.Instance.player_redness_speed * Time.deltaTime );
	}
#endregion

#region API
#endregion

#region Implementation
	float DetermineLowStaminaCofactor()
	{
		// Info: Use stamina as it is for the ratio when inflation is active, current/recoverable as the ratio when inflation is NOT active.
		var value = player_is_blocking.SharedValue == true
						? player_stamina.SharedValue
						: player_stamina.SharedValue / player_stamina.StaminaCapacity;

		return value;
	}
    
    void UpdateCharacterInflation()
    {
		var speed = Mathf.Lerp( GameSettings.Instance.player_inflation_speedRange.x,
								GameSettings.Instance.player_inflation_speedRange.y,
								1 - DetermineLowStaminaCofactor() );

		var characterInflation = player_inflation.SharedValue + direction * Time.deltaTime * speed;

		if( direction == +1 && characterInflation >= +1 )
		{
			direction = -1;
			characterInflation -= characterInflation - 1;
		}
		else if( direction == -1 && characterInflation <= -1 )
		{
			direction = +1;
			characterInflation -= 1 + characterInflation;
		}

		player_inflation.SharedValue = characterInflation;
	}

	float GetVignette()
	{
		return notifier_vignette_intencity.sharedValue;
	}

	void SetVignette( float value )
	{
		notifier_vignette_intencity.SharedValue = value;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
