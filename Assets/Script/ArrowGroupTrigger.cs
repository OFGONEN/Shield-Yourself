/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ArrowGroupTrigger : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] Collider arrow_group_trigger_collider;
    [ SerializeField ] GameEvent event_arrow_shot;
    [ SerializeField ] SharedFloatNotifier notif_player_speed;
    [ SerializeField ] SharedFloatNotifier notif_player_travel;

	[ ShowInInspector, ReadOnly ] int arrow_trigger_index;
	UnityMessage onUpdateMethod;
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
		arrow_trigger_index = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.ArrowTrigger_Key, 0 ) + 1;
		onUpdateMethod      = ExtensionMethods.EmptyMethod;
  	}

	private void Update()
	{
		onUpdateMethod();
	}
#endregion

#region API
	public void OnLevelStart()
	{
		Spawn();
	}

	public void OnLevelCompletePseudo()
	{
		PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.ArrowTrigger_Key, arrow_trigger_index );
	}

    public void OnTrigger()
    {
		arrow_group_trigger_collider.enabled = false;
		arrow_trigger_index++;
		event_arrow_shot.Raise();

		onUpdateMethod = ExtensionMethods.EmptyMethod;

		if( arrow_trigger_index <= GameSettings.Instance.arrow_trigger_spawn_count )
		{
			Spawn();
		}
	}
#endregion

#region Implementation
	void Movement()
	{
		var position = transform.position;
		transform.position = Vector3.MoveTowards( position, position + Vector3.left, Time.deltaTime * notif_player_speed.sharedValue );
	}

    void Spawn()
    {
		transform.position = transform.position.SetX( ReturnSpawnPosition() );
		arrow_group_trigger_collider.enabled = true;

		onUpdateMethod = Movement;
	}

	float ReturnSpawnPosition()
	{
		return ( GameSettings.Instance.game_travel_distance - GameSettings.Instance.player_speed ) * DOVirtual.EasedValue( 0, 1, ( float )arrow_trigger_index / GameSettings.Instance.arrow_trigger_spawn_count, GameSettings.Instance.arrow_trigger_spawn_ease ) - notif_player_travel.sharedValue;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
