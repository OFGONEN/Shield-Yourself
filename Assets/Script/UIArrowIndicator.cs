/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;


public class UIArrowIndicator : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ] 
    [ SerializeField ] SharedReferenceNotifier notif_camera;
    [ SerializeField ] PoolUIArrowIndicator pool_ui_arrow_indicator;

  [ Title( "Components" ) ] 
	[ SerializeField ] RectTransform rectTransform;
	[ SerializeField ] Image[] image_filled;

// Private
	RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    [ Button ]
    public void Spawn( float height, float delay )
    {
		gameObject.SetActive( true );

		for( var i = 0; i < image_filled.Length; i++ )
			image_filled[ i ].fillAmount = 0;

		var camera = ( notif_camera.sharedValue as Transform ).GetComponent< Camera >();
		var screenPosition = camera.WorldToScreenPoint( Vector3.up * height );

#if UNITY_EDITOR
		transform.position = new Vector3( Screen.width, screenPosition.y, 0 );
		rectTransform.anchoredPosition = rectTransform.anchoredPosition.SetX( 0 );
#else
		transform.position = new Vector3( Screen.width, screenPosition.y, 0 );
#endif

		var sequence = recycledSequence.Recycle( OnSequenceComplete );
		sequence.Append( image_filled[ 0 ].DOFillAmount( 1, delay / 3f ) );
		sequence.Append( image_filled[ 1 ].DOFillAmount( 1, delay / 3f ) );
		sequence.Append( image_filled[ 2 ].DOFillAmount( 1, delay / 3f ) );
	}

	public void OnLevelFinished()
	{
		recycledSequence.Kill();
		pool_ui_arrow_indicator.ReturnEntity( this );
	}
#endregion

#region Implementation
	void OnSequenceComplete()
	{
		pool_ui_arrow_indicator.ReturnEntity( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
