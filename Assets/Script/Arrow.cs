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
    [ SerializeField ] ParticleSystem arrow_trail_renderer;
    [ SerializeField ] Collider arrow_collider;

// Private VariablesSpawn
	private float arrow_speed;
	private Vector3 arrow_target_direction;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
	public void Spawn( float height, float speed )
	{
		gameObject.SetActive( true );

		var playerPosition = ( notif_player_target.SharedValue as Transform ).position;

		//Cache data
		var spawnRatio = Mathf.InverseLerp( GameSettings.Instance.arrow_shoot_spawn_range.x,
										GameSettings.Instance.arrow_shoot_spawn_range.y,
										height );

		var targetPosition = playerPosition.SetY( Mathf.Lerp( GameSettings.Instance.player_target_range.x,
								GameSettings.Instance.player_target_range.y,
								spawnRatio ) );

		var position               = new Vector3( shared_arrow_spawn_point.sharedValue, height, 0 );
		    arrow_speed            = speed;
		    arrow_target_direction = targetPosition - position;
		    transform.position     = position;

		//Configure components
		arrow_trail_renderer.Stop( true, ParticleSystemStopBehavior.StopEmittingAndClear );

		transform.LookAt( targetPosition, Vector3.up );

		arrow_collider.enabled = true;
		arrow_trail_renderer.Play();

		Subscribe_Cluster();
	}

	public void ReturnToPool()
	{
		arrow_collider.enabled       = false;
		arrow_trail_renderer.Stop( true, ParticleSystemStopBehavior.StopEmittingAndClear );

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
		var position = transform.position;
		transform.position = Vector3.MoveTowards( position, position + arrow_target_direction, Time.deltaTime * arrow_speed );
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