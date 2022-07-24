/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class AnimatorIKPass : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] SharedReferenceNotifier notif_IK_target;
    [ SerializeField ] SharedFloatNotifier notif_IK_weight;

  [ Title( "Components" ) ]
    [ SerializeField ] Animator _animator;

    Transform target_transform;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
		target_transform = notif_IK_target.SharedValue as Transform;
	}

    void OnAnimatorIK( int layerIndex ) 
    {
		_animator.SetIKPositionWeight( AvatarIKGoal.LeftHand, notif_IK_weight.sharedValue );
		_animator.SetIKPosition( AvatarIKGoal.LeftHand, target_transform.position );
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
