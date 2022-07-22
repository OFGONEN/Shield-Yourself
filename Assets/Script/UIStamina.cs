/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class UIStamina : MonoBehaviour
{
#region Fields
  [ Title( "Components" ) ] 
    [ SerializeField ] GameObject parent_rectTransform;
    [ SerializeField ] Image image_fill_background;
    [ SerializeField ] Image image_fill_foreground;

  [ Title( "Shared Variables" ) ] 
    [ SerializeField ] Stamina notif_player_stamina;

    RecycledTween recycledTween = new RecycledTween();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void OnStaminaChange()
    {
		parent_rectTransform.SetActive( true );

		recycledTween.Kill();

		image_fill_background.fillAmount = notif_player_stamina.StaminaCapacity;
		image_fill_foreground.fillAmount = notif_player_stamina.SharedValue;

		recycledTween.Recycle( DOVirtual.DelayedCall( GameSettings.Instance.player_ui_stamina_delay, OnFillComplete ) );
	}
#endregion

#region Implementation
    void OnFillComplete()
    {
		parent_rectTransform.SetActive( false );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
