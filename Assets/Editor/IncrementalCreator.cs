/* Created by and for usage of FF Studios (2021). */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEditor;

[ CreateAssetMenu( fileName = "tool_incremental_creator", menuName = "FFEditor/Incremental Creator" ) ]
public class IncrementalCreator : ScriptableObject
{
#region Fields
  [ Title( "Health Incremental" ) ] 
    public IncrementalHealth incremental_health;
    public CreateDataHealth data_health;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    [ Button() ]
    public void CreateHealth()
    {
        EditorUtility.SetDirty( incremental_health );

        IncrementalHealthData[] array = new IncrementalHealthData[ data_health.level_max ];

        for( int i = 0; i < data_health.level_max; i++ )
        {
            var ratio = (float)i / data_health.level_max;

            var data = new IncrementalHealthData();
            data.incremental_health_value = DOVirtual.EasedValue( data_health.range_health.x, data_health.range_health.y, ratio, data_health.ease_health );
            data.incremental_health_cost = Mathf.RoundToInt( DOVirtual.EasedValue( data_health.range_cost.x, data_health.range_cost.y, ratio, data_health.ease_cost ) );

            array[ i ] = data;
        }

        incremental_health.SetIncremental( array );
        AssetDatabase.SaveAssets();
    }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}

[ Serializable ]
public struct CreateDataHealth
{
    public int level_max;
    public Vector2 range_health;
    public Ease ease_health;
    public Vector2 range_cost;
    public Ease ease_cost;
}
