/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;


public class UIArrowIndicator : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ] 
    [ SerializeField ] SharedReferenceNotifier notif_camera;
    // [ SerializeField ] Pool notif_camera;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void Spawn( float height, float delay )
    {
        var camera = ( notif_camera.sharedValue as Transform ).GetComponent< Camera >();
		var screenPosition = camera.WorldToScreenPoint( Vector3.up * height );

		transform.position = new Vector3( Screen.width, screenPosition.y, 0 );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
