/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Outpost : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] GameEvent event_level_pause;
    [ SerializeField ] GameEvent event_level_complete_pseudo;
    [ SerializeField ] SharedFloatNotifier notif_player_speed;
    [ SerializeField ] SharedFloatNotifier notif_player_travel;
    [ SerializeField ] SharedFloatNotifier notif_level_progress;
	[ SerializeField ] ElephantLevelEvent event_elephant;

  [ Title( "Setup" ) ]
    [ SerializeField ] Collider outpost_collider;

	[ ShowInInspector, ReadOnly ] int outpost_index;
	[ ShowInInspector, ReadOnly ] float outpost_spawn_point;
	UnityMessage onUpdateMethod;
	
	float outpost_spawm_point_actual;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnDisable()
	{
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

    private void Awake()
    {
		outpost_index  = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.Outpost_Key, 1 );
		onUpdateMethod = ExtensionMethods.EmptyMethod;
    }

    private void Start()
    {
		notif_level_progress.SetValue_NotifyAlways( 0 );
		Spawn();
	}

	private void Update()
	{
		onUpdateMethod();
	}
#endregion

#region API
	public void OnTrigger()
    {
		event_elephant.level             = CurrentLevelData.Instance.currentLevel_Shown;
        event_elephant.elephantEventType = ElephantEvent.LevelCompleted;
        event_elephant.Raise();

		outpost_collider.enabled = false;
		outpost_index++;

		PlayerPrefsUtility.Instance.SetFloat( ExtensionMethods.PlayerTravel_Key, outpost_spawm_point_actual );

		event_level_pause.Raise();
		event_level_complete_pseudo.Raise();

		onUpdateMethod = ExtensionMethods.EmptyMethod;

		if( outpost_index <= GameSettings.Instance.outpost_spawn_count )
		{
			PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.Outpost_Key, outpost_index );
			Spawn();
		}
        else
			gameObject.SetActive( false );
	}
#endregion

#region Implementation
	void Movement()
	{
		var position = transform.position;
		    position = Vector3.MoveTowards( position, position + Vector3.left, Time.deltaTime * notif_player_speed.sharedValue );

		transform.position = position;

		notif_level_progress.SetValue_NotifyAlways( Mathf.InverseLerp( outpost_spawn_point, 0, position.x ) );
	}

    void Spawn()
    {
		outpost_spawm_point_actual = ReturnSpawnActualPosition();
		outpost_spawn_point = outpost_spawm_point_actual - notif_player_travel.sharedValue;

		notif_level_progress.SetValue_NotifyAlways( 0 );

		transform.position = transform.position.SetX( outpost_spawn_point );
		outpost_collider.enabled = true;

		onUpdateMethod = Movement;
	}

	float ReturnSpawnActualPosition()
	{
		return GameSettings.Instance.game_travel_distance * 
            DOVirtual.EasedValue( 0, 1, ( float )outpost_index / GameSettings.Instance.outpost_spawn_count, 
            GameSettings.Instance.outpost_spawn_ease );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}