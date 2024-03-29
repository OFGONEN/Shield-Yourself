/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "event_arrow_spawn", menuName = "FF/Event/Arrow Spawn Event" ) ]
public class ArrowGroupDataGameEvent : GameEvent 
{
	public ArrowGroupData eventValue;

    public void Raise( ArrowGroupData value )
    {
		eventValue = value;
		Raise();
	}
}
