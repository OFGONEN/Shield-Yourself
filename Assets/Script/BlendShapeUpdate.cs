/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class BlendShapeUpdate : MonoBehaviour
{
#region Fields
    [ SerializeField ] SkinnedMeshRenderer skinnedMeshRenderer;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
	public void UpdateBlendShape_Inverted( SharedFloatNotifier notifier )
	{
		skinnedMeshRenderer.SetBlendShapeWeight( 0, 100.0f * ( 1.0f - notifier.sharedValue ) );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
