/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class UIManager : MonoBehaviour
    {
#region Fields
    [ Title( "Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedResponse;
        public EventListenerDelegateResponse levelCompleteResponse;
        public EventListenerDelegateResponse levelFailResponse;
        public EventListenerDelegateResponse tapInputListener;

    [ Title( "UI Elements" ) ]
        public Transform pool_parent;
        public UI_Patrol_Scale level_loadingBar_Scale;
        public TextMeshProUGUI level_count_text;
        public TextMeshProUGUI level_information_text;
        public UI_Patrol_Scale level_information_text_Scale;
        public Image loadingScreenImage;
        public Image foreGroundImage;
        public RectTransform tutorialObjects;
        public UIIncrementalHealth ui_incremental_health;
        public UIIncrementalStamina ui_incremental_stamina;
        public UIIncrementalCurrency ui_incremental_currency;

    [ Title( "Fired Events" ) ]
        public GameEvent levelRevealedEvent;
        public GameEvent loadNewLevelEvent;
        public GameEvent resetLevelEvent;
        public GameEvent event_level_resumed;
        public ElephantLevelEvent elephantLevelEvent;

    [ Title( "Shared Variables" ) ]
        public PoolUIArrowIndicator pool_ui_arrow_indicator;
        public UIParticlePool pool_ui_particle_currency;

        RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Unity API
        private void OnEnable()
        {
            levelLoadedResponse.OnEnable();
            levelFailResponse.OnEnable();
            levelCompleteResponse.OnEnable();
            tapInputListener.OnEnable();
        }

        private void OnDisable()
        {
            levelLoadedResponse.OnDisable();
            levelFailResponse.OnDisable();
            levelCompleteResponse.OnDisable();
            tapInputListener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedResponse.response   = LevelLoadedResponse;
            levelFailResponse.response     = LevelFailResponse;
            levelCompleteResponse.response = LevelCompleteResponse;
            tapInputListener.response      = ExtensionMethods.EmptyMethod;

			level_information_text.text = "Tap to Start";

			pool_ui_arrow_indicator.InitPool( pool_parent, false );
			pool_ui_particle_currency.InitPool( pool_parent, false );
		}
#endregion

#region API
        public void OnLevelCompletePseudo()
        {
			level_information_text.text = "Tap to Continue";

			var sequence = recycledSequence.Recycle( OnShowIncrementalComplete );
			sequence.AppendCallback( ShowIncrementalButtons );
			sequence.Join( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );
			sequence.AppendInterval( Mathf.Abs( GameSettings.Instance.ui_Entity_Fade_TweenDuration - GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );
		}
#endregion

#region Implementation
        void OnShowIncrementalComplete()
        {
			tapInputListener.response = OnContinue;
		}

        void OnContinue()
        {
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			HideIncrementalButtons();
			level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ).OnComplete( event_level_resumed.Raise);
		}

        private void LevelLoadedResponse()
        {
			var sequence = DOTween.Sequence()
								.Append( level_loadingBar_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
								.Append( loadingScreenImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
								.AppendCallback( ShowIncrementalButtons )
								.Join( foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
								.AppendCallback( () => tapInputListener.response = StartLevel );

            levelLoadedResponse.response = NewLevelLoaded;
        }

        private void NewLevelLoaded()
        {
			level_information_text.text = "Tap to Start";

			var sequence = DOTween.Sequence();

			// Tween tween = null;

			sequence.Append( foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.AppendCallback( ShowIncrementalButtons )
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = StartLevel );

            // elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            // elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
            // elephantLevelEvent.Raise();
        }

        private void LevelCompleteResponse()
        {
            var sequence = DOTween.Sequence();

			// Tween tween = null;

			level_information_text.text = "Completed \n\n Tap to Continue";

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = LoadNewLevel );

        //     elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
        //     elephantLevelEvent.elephantEventType = ElephantEvent.LevelCompleted;
        //     elephantLevelEvent.Raise();
        }

        private void LevelFailResponse()
        {
            var sequence = DOTween.Sequence();

			// Tween tween = null;
			level_information_text.text = "Try Again \n\n Tap to Continue";

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    // .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = Resetlevel );

            elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelFailed;
            elephantLevelEvent.Raise();
        }



		private void StartLevel()
		{
			foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration );

			level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration );
			level_information_text_Scale.Subscribe_OnComplete( levelRevealedEvent.Raise );
			HideIncrementalButtons();

			tutorialObjects.gameObject.SetActive( false );

			tapInputListener.response = ExtensionMethods.EmptyMethod;

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
			elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
			elephantLevelEvent.Raise();
		}

		private void LoadNewLevel()
		{
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
			        .AppendCallback( loadNewLevelEvent.Raise );
		}

		private void Resetlevel()
		{
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
			        .AppendCallback( resetLevelEvent.Raise );
		}

        void ShowIncrementalButtons()
        {
			ui_incremental_health.ShowButton();
			ui_incremental_stamina.ShowButton();
			ui_incremental_currency.ShowButton();
		}

        void HideIncrementalButtons()
        {
			ui_incremental_health.HideButton();
			ui_incremental_stamina.HideButton();
			ui_incremental_currency.HideButton();
        }
#endregion
    }
}