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

    [ Title( "Level Releated" ) ]
        public SharedReferenceNotifier notif_camera;
        public SharedFloatNotifier notif_level_progress;
        public SharedFloat shared_arrow_spawn_point; 

    [ Title( "Shared Variables" ) ]
        [ SerializeField ] Currency player_currency;
        [ SerializeField ] SharedFloatNotifier player_health;
        [ SerializeField ] Stamina player_stamina;

    [ Title( "Shared Variables" ) ]
        [ SerializeField ] IncrementalCurrency incremental_currency;
        [ SerializeField ] IncrementalStamina incremental_stamina;
        [ SerializeField ] IncrementalHealth incremental_health;

// Private
        Camera mainCamera;

        IncrementalStaminaData incremental_stamina_data;
        IncrementalHealthData incremental_health_data;
        IncrementalCurrecyData incremental_currency_data;
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
		}

        // Info: Called from Editor.
        public void LevelRevealedResponse()
        {
            // Cache Camera component from main camera
            mainCamera = ( notif_camera.sharedValue as Transform ).GetComponent< Camera >();

            // Determina the most right visible position of the world 
			var mostRightPosition = mainCamera.ScreenToWorldPoint( new Vector3( Screen.width, 0, Mathf.Abs( mainCamera.transform.position.z ) ) );
			shared_arrow_spawn_point.sharedValue = mostRightPosition.x;

			CacheCurrentIncrementals(); // Cache current incrementals

			// Set Up Player Properties
			player_currency.Load(); // Currency
			player_stamina.Default(); // Stamina
			player_health.sharedValue = incremental_health_data.incremental_health_value; // Health
		}

        // Info: Called from Editor.
        public void LevelStartedResponse()
        {

        }

        public void CacheCurrentIncrementals()
        {
			incremental_stamina_data  = incremental_stamina.CurrentIncremental();
			incremental_health_data   = incremental_health.CurrentIncremental();
			incremental_currency_data = incremental_currency.CurrentIncremental();
		}
#endregion

#region Implementation
#endregion
    }
}