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
    [ SerializeField ] ArrowGroupDataGameEvent event_arrow_spawn;
    [ SerializeField ] PoolArrowGroupTrigger pool_arrow_trigger_group;

  [ Title( "Setup" ) ]
    [ SerializeField ] Collider arrow_group_trigger_collider;

// Private
    ArrowGroupData arrow_group_trigger_index;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void Spawn( ArrowGroupData data, Vector3 position )
    {
		transform.position        = position;
		arrow_group_trigger_index = data;

		arrow_group_trigger_collider.enabled = true;
	}

    public void OnTrigger()
    {
		event_arrow_spawn.Raise( arrow_group_trigger_index );

		arrow_group_trigger_collider.enabled = false;

		pool_arrow_trigger_group.ReturnEntity( this );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
