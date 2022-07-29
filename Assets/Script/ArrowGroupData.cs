/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "arrow_group_", menuName = "FF/Data/Arrow Group" ) ]
public class ArrowGroupData : ScriptableObject
{
#region Fields
	public float arrow_indicator_height;
	public float arrow_shoot_delay;
	public ArrowData[] arrow_shoot_group;
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}