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
    [ SerializeField ] GameEvent event_level_complete_pseudo;
    [ SerializeField ] SharedFloatNotifier notif_player_speed;
    [ SerializeField ] SharedFloatNotifier notif_player_travel;

  [ Title( "Setup" ) ]
    [ SerializeField ] Collider outpost_collider;

	[ ShowInInspector, ReadOnly ] int outpost_index;
	UnityMessage onUpdateMethod;
#endregion

#region Properties
#endregion

#region Unity API
  private void Awake()
  {
		outpost_index  = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.Outpost_Key, 1 );
		onUpdateMethod = ExtensionMethods.EmptyMethod;
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

	public void OnLevelFailed()
	{
		outpost_index = Mathf.Max( outpost_index - 1, 1 );
		PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.Outpost_Key, outpost_index );
	}

    public void OnTrigger()
    {
		outpost_collider.enabled = false;
		outpost_index++;
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
		transform.position = Vector3.MoveTowards( position, position + Vector3.left, Time.deltaTime * notif_player_speed.sharedValue );
	}

    void Spawn()
    {
		transform.position = transform.position.SetX( ReturnSpawnPosition() );
		outpost_collider.enabled = true;

		onUpdateMethod = Movement;
	}

	float ReturnSpawnPosition()
	{
		return GameSettings.Instance.game_travel_distance * 
            DOVirtual.EasedValue( 0, 1, ( float )outpost_index / GameSettings.Instance.outpost_spawn_count, 
            GameSettings.Instance.outpost_spawn_ease ) - notif_player_travel.sharedValue;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}