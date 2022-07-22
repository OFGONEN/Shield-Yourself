/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public abstract class IncrementalSystem< Incremental > : ScriptableObject where Incremental : struct, IIncrementalData
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] Incremental[] incremental;
    [ SerializeField ] string incremental_name;

  [ Title( "Setup" ) ]
    [ SerializeField ] SharedFloatNotifier currency;
  
    Incremental incremental_current;
    public Incremental CurrentIncremental => incremental_current;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public bool CanUnlock()
    {
		var currentIndex = PlayerPrefsUtility.Instance.GetInt( incremental_name, 0 );

		var lastIndex = currentIndex == incremental.Length - 1;

		var canAfford = currency.sharedValue > Cost();
		return !lastIndex && canAfford;
	}

    public void Unlock()
    {
		var currentIndex = PlayerPrefsUtility.Instance.GetInt( incremental_name, 0 );
		PlayerPrefsUtility.Instance.SetInt( incremental_name, currentIndex + 1 );

		currency.SharedValue -= Cost();
	}

    public Incremental CacheCurrentIncremental()
    {
		return incremental_current = incremental[ PlayerPrefsUtility.Instance.GetInt( incremental_name, 0 ) ];
	}

    public int Cost()
    {
		return incremental_current.Cost();
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
