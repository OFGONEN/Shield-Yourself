/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Arrow : MonoBehaviour, IClusterEntity
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] Cluster cluster_arrow;
    [ SerializeField ] PoolArrow pool_arrow;
    [ SerializeField ] SharedReferenceNotifier notif_player_target;
	[ SerializeField ] SharedFloat shared_arrow_spawn_point;

  [ Title( "Components" ) ]
    [ SerializeField ] TrailRenderer arrow_trail_renderer;
    [ SerializeField ] Collider arrow_collider;

// Private VariablesSpawn
	private float arrow_speed;
	private Vector3 arrow_target_position;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
	public void Spawn( float height, float speed )
	{
		//Cache data
		transform.position    = new Vector3( shared_arrow_spawn_point.sharedValue, height, 0 );
		arrow_target_position = ( notif_player_target.sharedValue as Transform ).position;
		arrow_speed           = speed;

		//Configure components
		arrow_trail_renderer.Clear();

		arrow_collider.enabled       = true;
		arrow_trail_renderer.enabled = true;

		transform.LookAt( arrow_target_position, Vector3.up );

		Subscribe_Cluster();
	}

	public void ReturnToPool()
	{
		arrow_collider.enabled       = false;
		arrow_trail_renderer.enabled = false;

		UnSubscribe_Cluster();

		pool_arrow.ReturnEntity( this );
	}
#endregion

#region Implementation
#endregion

#region IClusterEntity
	public int GetID()
	{
		return GetInstanceID();
	}

	public void OnUpdate_Cluster()
	{
		transform.position = Vector3.MoveTowards( transform.position, arrow_target_position, Time.deltaTime * arrow_speed );
	}

	public void Subscribe_Cluster()
	{
		cluster_arrow.Subscribe( this );
	}

	public void UnSubscribe_Cluster()
	{
		cluster_arrow.UnSubscribe( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}