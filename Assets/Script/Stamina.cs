/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "notif_player_stamina", menuName = "FF/Game/Stamina" ) ]
public class Stamina : SharedFloatNotifier
{
#region Fields
    // Shared Vale is current stamina
    // Stamina is always calculated assuming stamina capacity is always 1 as default.
    // Incremental upgrades only change recover, deplete and capacity deplete values.
    // Keeping max stamina capacity as 1 on all incremental upgrades

    float stamina_capacity;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void SetUp()
    {
		sharedValue      = 1;
		stamina_capacity = 1;
	}

    public void Recover( float recover )
    {
		SharedValue = Mathf.Max( sharedValue + recover, stamina_capacity );
	}

    public void Deplete( float depleteCurrent, float depleteCapacity )
    {
		SharedValue -= depleteCurrent;
		stamina_capacity -= depleteCapacity;
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
