/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class OutpostGraphic : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] SharedFloatNotifier notif_player_speed;
    [ SerializeField ] SharedFloat shared_screen_left_position;
    [ SerializeField ] SharedReferenceNotifier notif_outpost_transform;
  [ Title( "Setup" ) ]
    [ SerializeField ] GameObject outpost_graphic;
    [ SerializeField ] ParticleSystem outpost_particle;
    [ SerializeField ] float outpost_length;

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
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		onUpdateMethod();
	}
#endregion

#region API
    public void OnLevelCompletePseudo()
    {
		transform.position = ( notif_outpost_transform.sharedValue as Transform ).position;

		outpost_graphic.SetActive( true );
		outpost_particle.Stop( true, ParticleSystemStopBehavior.StopEmittingAndClear );
		outpost_particle.Play();

		onUpdateMethod = Movement;
	}
#endregion

#region Implementation
    void Movement()
    {
		var position = transform.position;

		if( position.x < shared_screen_left_position.sharedValue - outpost_length )
        {
			outpost_graphic.SetActive( false );
			outpost_particle.Stop( true, ParticleSystemStopBehavior.StopEmittingAndClear );

			onUpdateMethod = ExtensionMethods.EmptyMethod;
		}
        else
        {
			transform.position = Vector3.MoveTowards( position, position + Vector3.left, Time.deltaTime * notif_player_speed.sharedValue );
        }
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}