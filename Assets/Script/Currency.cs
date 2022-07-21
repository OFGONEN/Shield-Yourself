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
    float gain_cooldown;
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
			SharedValue += value;
			gain_cooldown = Time.time + rate;
		}
    }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
