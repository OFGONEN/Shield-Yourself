/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Arrow : MonoBehaviour, IClusterEntity
{
#region Fields
  [ Title(" Setup" ) ]
    [ SerializeField ] Cluster cluster_arrow;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
#endregion

#region IClusterEntity
	public int GetID()
	{
		return GetInstanceID();
	}

	public void OnUpdate_Cluster()
	{
		// throw new System.NotImplementedException();
	}

	public void Subscribe_Cluster()
	{
		cluster_arrow.Subscribe( this );
	}

	public void UnSubscribe_Cluster()
	{
		cluster_arrow.UnSubscribe( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
