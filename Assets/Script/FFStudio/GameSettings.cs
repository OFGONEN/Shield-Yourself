/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace FFStudio
{
	public class GameSettings : ScriptableObject
    {
#region Fields (Settings)
    // Info: You can use Title() attribute ONCE for every game-specific group of settings.
    [ Title( "Player" ) ]
        [ LabelText( "Target Range" ) ] public Vector2 player_target_range;
        [ LabelText( "Walking Speed" ) ] public float player_speed;
        [ LabelText( "Shield Activate Delay" ) ] public float player_shield_activate_delay;
        [ LabelText( "Arrow Damage Value" ) ] public float player_arrow_damage;
        [ LabelText( "Level Failed Delay" ) ] public float player_death_delay = 1f;
		[ LabelText( "Stamina Low Threshold" ), SuffixLabel( "%" ) ] public NormalizedValue player_stamina_threshold = 0.75f;
		[ LabelText( "Stamina Low Vignette Range" ), MinMaxSlider( 0, 1 ) ] public Vector2 player_stamina_vignette_range;
		[ LabelText( "Vignette effect fall-off duration" ) ] public float player_stamina_vignette_falloff_duration;
		[ LabelText( "Color Update Speed" ), Min( 0 ) ] public float player_redness_speed = 0.75f;
		[ LabelText( "Infilation Speed Range" ), MinMaxSlider( 0, 10, true )] public Vector2 player_inflation_speedRange = Vector2.up;
		[ LabelText( "Player Travel Text Update Rate" ) ] public float player_travel_text_update_rate = 0.1f;

    [ Title( "Arrow" ) ]
        [ LabelText( "Arrow Shot Delay Between Arrows" ) ] public Vector2 arrow_shoot_delay_between_range;
        [ LabelText( "Arrow Shot Delay Between Arrows Difficulty Easing" ) ] public Ease arrow_shoot_delay_between_ease;
        [ LabelText( "Arrow Shot Delay" ) ] public Vector2 arrow_shoot_delay_range;
        [ LabelText( "Arrow Shot Delay Difficulty Easing" ) ] public Ease arrow_shoot_delay_ease;
        [ LabelText( "Arrow Speed Range" ) ] public Vector2 arrow_shoot_speed_range;
        [ LabelText( "Arrow Speed Difficulty Easing" ) ] public Ease arrow_shoot_speed_ease;
        [ LabelText( "Arrow Count Range" ) ] public Vector2 arrow_shoot_count_range;
        [ LabelText( "Arrow Count Difficulty Easing" ) ] public Ease arrow_shoot_count_ease;
        [ LabelText( "Arrow Spawn Height Range" ) ] public Vector2 arrow_shoot_spawn_range;
        [ LabelText( "Arrow Trigger Spawn Count" ) ] public int arrow_trigger_spawn_count;
        [ LabelText( "Arrow Trigger Spawn Ease" ) ] public Ease arrow_trigger_spawn_ease;

    [ Title( "Shield" ) ]
        [ LabelText( "Shield Movement Speed" ) ] public float shield_movement_speed = 1f;

    [ Title( "Outpost" ) ]
        [ LabelText( "Outpost Spawn Count" ) ] public int outpost_spawn_count;
        [ LabelText( "Outpost Spawn Ease" ) ] public Ease outpost_spawn_ease;

    [ Title( "Player UI" ) ]
        [ LabelText( "Health Bar Fill Speed" ) ] public float player_ui_health_fill_speed;
        [ LabelText( "Health Bar Disappear Delay" ) ] public float player_ui_health_delay;
        [ LabelText( "Health Bar Disappear Delay" ) ] public float player_ui_stamina_delay;
    
    [ Title( "Camera" ) ]
        [ LabelText( "Follow Speed (Z)" ), SuffixLabel( "units/seconds" ), Min( 0 ) ] public float camera_follow_speed_depth = 2.8f;
    
    [ Title( "Project Setup", "These settings should not be edited by Level Designer(s).", TitleAlignments.Centered ) ]
        public int maxLevelCount;
        [ LabelText( "Pseudo Level Count" ) ] public int game_level_pseudoCount;
        [ LabelText( "Game Travel Distance" ) ] public float game_travel_distance = 450;
        
        // Info: 3 groups below (coming from template project) are foldout by design: They should remain hidden.
		[ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_GameSettings;
        [ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_Components;

        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_Move_TweenDuration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the fading for ui element"            ) ] public float ui_Entity_Fade_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the scaling for ui element"           ) ] public float ui_Entity_Scale_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for floating ui element" ) ] public float ui_Entity_FloatingMove_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Joy Stick"                                        ) ] public float ui_Entity_JoyStick_Gap;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Pop Up Text relative float height"                ) ] public float ui_PopUp_height;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Pop Up Text float duration"                       ) ] public float ui_PopUp_duration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Random Spawn Area in Screen" ), SuffixLabel( "percentage" ) ] public float ui_particle_spawn_width; 
		[ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Spawn Duration" ), SuffixLabel( "seconds" ) ] public float ui_particle_spawn_duration; 
		[ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Spawn Ease" ) ] public Ease ui_particle_spawn_ease;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Wait Time Before Target" ) ] public float ui_particle_target_waitTime;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Target Travel Time" ) ] public float ui_particle_target_duration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "UI Particle Target Travel Ease" ) ] public Ease ui_particle_target_ease;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Percentage of the screen to register a swipe"     ) ] public int swipeThreshold;

        [ FoldoutGroup( "Debug" ) ] public float debug_ui_text_float_height;
        [ FoldoutGroup( "Debug" ) ] public float debug_ui_text_float_duration;
#endregion

#region Properties
        public float LevelRatio => CurrentLevelData.Instance.currentLevel_Shown / ( float )game_level_pseudoCount;
#endregion

#region Fields (Singleton Related)
        static GameSettings instance;

        delegate GameSettings ReturnGameSettings();
        static ReturnGameSettings returnInstance = LoadInstance;

		public static GameSettings Instance => returnInstance();
#endregion

#region Implementation
        static GameSettings LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< GameSettings >( "game_settings" );

			returnInstance = ReturnInstance;

			return instance;
		}

		static GameSettings ReturnInstance()
        {
            return instance;
        }
#endregion

#region Editor Only
#if UNITY_EDITOR
        private void OnValidate()
        {
            game_level_pseudoCount = outpost_spawn_count;
        }
#endif
#endregion
    }
}
