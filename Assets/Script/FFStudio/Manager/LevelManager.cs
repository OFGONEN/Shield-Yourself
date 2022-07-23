/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
    [ Title( "Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;
        public GameEvent event_level_started;

    [ Title( "Level Releated" ) ]
        public SharedReferenceNotifier notif_camera;
        public SharedFloatNotifier notif_level_progress;
        public SharedFloat shared_arrow_spawn_point; 

    [ Title( "Shared Variables" ) ]
        [ SerializeField ] Currency player_currency;
// Private
        Camera mainCamera;
#endregion

#region UnityAPI
#endregion

#region API
        // Info: Called from Editor.
        public void LevelLoadedResponse()
        {
			notif_level_progress.SetValue_NotifyAlways( 0 );

			var levelData = CurrentLevelData.Instance.levelData;
            // Set Active Scene.
			if( levelData.scene_overrideAsActiveScene )
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 1 ) );
            else
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 0 ) );
            
            // Set Up Player Properties
			player_currency.Load(); // Currency
		}

        // Info: Called from Editor.
        public void LevelRevealedResponse()
        {
            // Cache Camera component from main camera
            mainCamera = ( notif_camera.sharedValue as Transform ).GetComponent< Camera >();

            // Determina the most right visible position of the world 
			var mostRightPosition = mainCamera.ScreenToWorldPoint( new Vector3( Screen.width, 0, Mathf.Abs( mainCamera.transform.position.z ) ) );
			shared_arrow_spawn_point.sharedValue = mostRightPosition.x;

			event_level_started.Raise();
		}

        // Info: Called from Editor.
        public void LevelStartedResponse()
        {

        }
#endregion

#region Implementation
#endregion
    }
}