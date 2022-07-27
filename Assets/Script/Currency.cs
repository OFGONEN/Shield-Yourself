/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "notif_player_currency", menuName = "FF/Game/Currency" ) ]
public class Currency : SharedFloatNotifier
{
#region Fields
    [ SerializeField ] UIParticlePool pool_ui_particle;
    [ SerializeField ] SharedReferenceNotifier notif_camera_transform;
    [ SerializeField ] SharedReferenceNotifier notif_ui_currency_transform;
    [ SerializeField ] SharedReferenceNotifier notif_player_transform;

    float gain_cooldown;

    Vector3 player_screen_position;
    Vector3 ui_currency_position;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void Gain( float value, float rate )
    {
        if( Time.time >= gain_cooldown )
        {
			sharedValue += value;
			gain_cooldown = Time.time + rate;

			pool_ui_particle.Spawn( player_screen_position, ui_currency_position, "+" + value.ToString( "f" ), Notify );
		}
    }

    public void Load()
    {
      	gain_cooldown = 0;
		SharedValue = PlayerPrefsUtility.Instance.GetFloat( ExtensionMethods.Currency_Key, 0 );
	}

    public void Save()
    {
		PlayerPrefsUtility.Instance.SetFloat( ExtensionMethods.Currency_Key, sharedValue );
	}

    public void OnLevelStart()
    {
		var playerPosition = ( notif_player_transform.sharedValue as Transform ).position;
		player_screen_position = ( notif_camera_transform.SharedValue as Transform ).GetComponent< Camera >().WorldToScreenPoint( playerPosition );
        ui_currency_position   = ( notif_ui_currency_transform.SharedValue as Transform ).position;
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
