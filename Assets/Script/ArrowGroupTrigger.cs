/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class ArrowGroupTrigger : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] Collider arrow_group_trigger_collider;
    [ SerializeField ] GameEvent event_arrow_shot;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void Spawn( Vector3 position )
    {
		transform.position        = position;
		arrow_group_trigger_collider.enabled = true;
	}

    public void OnTrigger()
    {
		arrow_group_trigger_collider.enabled = false;
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
