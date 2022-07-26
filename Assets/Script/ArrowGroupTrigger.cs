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

	int arrow_trigger_index;
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
	{
		arrow_trigger_index = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.ArrowTrigger_Key, 1 );
	}
#endregion

#region API
	public void OnLevelStart()
	{
		Spawn();
	}

    public void OnTrigger()
    {
		arrow_group_trigger_collider.enabled = false;
		arrow_trigger_index++;

		if( arrow_trigger_index <= GameSettings.Instance.arrow_trigger_spawn_count )
		{
			PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.ArrowTrigger_Key, arrow_trigger_index );
			Spawn();
		}
	}
#endregion

#region Implementation
    void Spawn()
    {
		transform.position = transform.position.SetX( ReturnSpawnPosition() );
		arrow_group_trigger_collider.enabled = true;
	}

	float ReturnSpawnPosition()
	{
		return ( GameSettings.Instance.game_travel_distance - GameSettings.Instance.player_speed ) * DOVirtual.EasedValue( 0, 1, ( float )arrow_trigger_index / GameSettings.Instance.arrow_trigger_spawn_count, GameSettings.Instance.arrow_trigger_spawn_ease );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
