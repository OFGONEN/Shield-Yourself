/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "system_arrow", menuName = "FF/System/Arrow" ) ]
public class ArrowSystem : ScriptableObject
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] PoolRecycledSequence pool_recycled_sequence;
    [ SerializeField ] PoolUIArrowIndicator pool_ui_arrow_indicator;
    [ SerializeField ] PoolArrow pool_arrow;
// Private
    Dictionary< int, RecycledSequence > sequence_active = new Dictionary< int, RecycledSequence >();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
	public void SpawnArrows()
	{

	}
    // public void SpawnArrowGroup( ArrowGroupDataGameEvent gameEvent )
    // {
	// 	// Setup sequence
	// 	var recycledSequence = pool_recycled_sequence.GetEntity();
	// 	recycledSequence.Recycle( () => OnSpawnSequenceComplete( recycledSequence ) );

	// 	sequence_active.Add( recycledSequence.ID, recycledSequence );
	// 	var sequence = recycledSequence.Sequence;

	// 	// Shoot Delay
	// 	var delay = gameEvent.eventValue.arrow_shoot_delay;
	// 	var arrowGroup = gameEvent.eventValue.arrow_shoot_group;

	// 	sequence.AppendInterval( delay );

	// 	for( var i = 0; i < arrowGroup.Length; i++ )
    //     {
	// 		var data = arrowGroup[ i ];
	// 		sequence.AppendInterval( arrowGroup[ i ].arrow_spawn_delay );
	// 		sequence.AppendCallback( () => SpawnArrow( data.arrow_spawn_height, data.arrow_speed ) );
	// 	}

    //     // Spawn Arrow indicator
	// 	var uiArrowIndicator = pool_ui_arrow_indicator.GetEntity();
	// 	uiArrowIndicator.Spawn( gameEvent.eventValue.arrow_indicator_height, delay );
	// }

    public void OnLevelFinished()
    {
        foreach( var recycledSequence in sequence_active.Values )
        {
			recycledSequence.Kill();
		}

		sequence_active.Clear();
	}
#endregion

#region Implementation
    void SpawnArrow( float height, float delay )
    {
		pool_arrow.GetEntity().Spawn( height, delay );
	}

    void OnSpawnSequenceComplete( RecycledSequence recycledSequence )
    {
		sequence_active.Remove( recycledSequence.GetHashCode() );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
