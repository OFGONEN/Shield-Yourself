/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[CreateAssetMenu( fileName = "pool_recycled_sequence", menuName = "FF/Data/Pool/Recycled Sequence" )]
public class PoolRecycledSequence : RuntimePool< RecycledSequence >
{

#region API
	public override void InitPool()
	{
		stack = new Stack< RecycledSequence >( stackSize );

        for( var i = 0; i < stackSize; i++ )
		{
			var entity = InitEntity();
			stack.Push( entity );
		}
	}

	public override RecycledSequence GetEntity()
	{
		RecycledSequence entity;

		if( stack.Count > 0 )
			entity = stack.Pop();
		else
			entity = InitEntity();

		return entity;
	}

	public override void ReturnEntity( RecycledSequence entity )
	{
		entity.Kill();
		stack.Push( entity );
	}

	protected override RecycledSequence InitEntity()
	{
		return new RecycledSequence();
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
