/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class UIHealth : MonoBehaviour
{
#region Fields
  [ Title( "Components" ) ] 
    [ SerializeField ] GameObject parent_rectTransform;
    [ SerializeField ] Image image_fill;

  [ Title( "Shared Variables" ) ] 
    [ SerializeField ] SharedFloatNotifier notif_player_health_ratio;

    RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void OnHealthChange( float value )
    {
		parent_rectTransform.SetActive( true );

		recycledSequence.Kill();
		recycledSequence.Recycle( OnFillComplete );

		var distance = Mathf.Abs( image_fill.fillAmount - value );
		var duration = distance / GameSettings.Instance.player_ui_health_fill_speed;

		var sequence = recycledSequence.Sequence;
		sequence.Append( image_fill.DOFillAmount( value, duration ) );
		sequence.AppendInterval( GameSettings.Instance.player_ui_health_delay );
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
