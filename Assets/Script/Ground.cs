/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Ground : MonoBehaviour
{
#region Fields
    [ SerializeField ] SharedFloatNotifier notif_player_speed;
    [ SerializeField ] SharedFloat shared_screen_left_position;
    [ SerializeField ] Transform[] grounds;
    [ SerializeField ] float ground_lenght;

    Queue< Transform > ground_queue;
    Transform ground_last;

// Private
    UnityMessage onUpdateMethod;
	Camera _camera;
	#endregion

	#region Properties
	#endregion

	#region Unity API
	private void Awake()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
        ground_queue = new Queue< Transform >( grounds.Length );

        for( var i = 0; i < grounds.Length; i++ )
        {
			ground_queue.Enqueue( grounds[ i ] );
		}

		ground_last = grounds[ grounds.Length - 1 ];
	}

    private void Update()
    {
		onUpdateMethod();
	}
#endregion

#region API
    public void OnLevelStarted()
    {
		onUpdateMethod = Movement;
	}

    public void OnLevelFinished()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}
#endregion

#region Implementation
    void Movement()
    {
		var firstGround = ground_queue.Peek();
		var firstGroundPosition = firstGround.position;
		if( firstGroundPosition.x < shared_screen_left_position.sharedValue - ground_lenght )
        {
			firstGround.position = firstGroundPosition.SetX( ground_last.position.x + ground_lenght );
			ground_last = firstGround;

			ground_queue.Dequeue();
			ground_queue.Enqueue( firstGround );
		}

        // Move all grounds towards left
        for( var i = 0; i < grounds.Length; i++ )
        {
			var ground = grounds[ i ];

			var position = ground.position;
			ground.position = Vector3.MoveTowards( position, position + Vector3.left, Time.deltaTime * notif_player_speed.sharedValue );
		}
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}