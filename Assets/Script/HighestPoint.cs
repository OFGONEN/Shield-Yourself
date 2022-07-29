/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class HighestPoint : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] SharedFloatNotifier notif_player_speed;
    [ SerializeField ] SharedFloatNotifier notif_player_travel;
    [ SerializeField ] SharedFloat shared_screen_left_position;
    [ SerializeField ] GameEvent event_level_paused;

  [ Title( "Setup" ) ]
    [ SerializeField ] Collider outpost_collider;
    [ SerializeField ] GameObject outpost_graphic_active;
    [ SerializeField ] GameObject outpost_graphic_inactive;
    [ SerializeField ] ParticleSystem outpost_particle;
    [ SerializeField ] float outpost_length;

    float point_distance;

    UnityMessage onUpdateMethod;
    UnityMessage onLevelStart;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnDisable()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

    private void Start()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
		onLevelStart = ExtensionMethods.EmptyMethod;

		point_distance = PlayerPrefsUtility.Instance.GetFloat( ExtensionMethods.HighestPoint_Key, -1 );

        if( point_distance > 0 )
		    Spawm();
        else
			outpost_graphic_inactive.SetActive( false );
	}

    private void Update()
    {
		onUpdateMethod();
	}
#endregion

#region API
    public void OnLevelStart()
    {
		onLevelStart();
	}

    public void OnLevelFailed()
    {
		PlayerPrefsUtility.Instance.SetFloat( ExtensionMethods.HighestPoint_Key, notif_player_travel.sharedValue );
    }

    public void OnTrigger()
    {
		event_level_paused.Raise();

		outpost_collider.enabled = false;
		outpost_graphic_inactive.SetActive( false );
		outpost_graphic_active.SetActive( true );
		outpost_particle.Play();
    }
#endregion

#region Implementation
    void LevelStart()
    {
		onUpdateMethod = Movement;
	}

    void Spawm()
    {
		onLevelStart = LevelStart;

		transform.position = transform.position.SetX( ReturnSpawnPosition() );
		outpost_collider.enabled = true;
		outpost_graphic_inactive.SetActive( true );
		outpost_particle.Stop( true, ParticleSystemStopBehavior.StopEmittingAndClear );

		onUpdateMethod = Movement;
    }

	void Movement()
	{
		var position = transform.position;

		if( position.x < shared_screen_left_position.sharedValue - outpost_length )
        {
			outpost_graphic_inactive.SetActive( false );
			outpost_graphic_active.SetActive( false );
			outpost_particle.Stop( true, ParticleSystemStopBehavior.StopEmittingAndClear );

			onUpdateMethod = ExtensionMethods.EmptyMethod;
		}
        else
        {
			transform.position = Vector3.MoveTowards( position, position + Vector3.left, Time.deltaTime * notif_player_speed.sharedValue );
        }	
    }

	float ReturnSpawnPosition()
	{
		return PlayerPrefsUtility.Instance.GetFloat( ExtensionMethods.HighestPoint_Key, 0 ) - notif_player_travel.sharedValue;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}