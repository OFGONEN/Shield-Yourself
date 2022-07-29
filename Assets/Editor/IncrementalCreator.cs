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
    [ ReadOnly, LabelText( "Total Health Cost" ) ] public int incremental_health_cost;

  [ Title( "Stamina Incremental" ) ] 
    public IncrementalStamina incremental_stamina;
    public CreateStaminaCurrency data_stamina;
    [ ReadOnly, LabelText( "Total Stamina Cost" ) ] public int incremental_stamina_cost;


  [ Title( "Currency Incremental" ) ] 
    public IncrementalCurrency incremental_currency;
    public CreateDataCurrency data_currency;
    [ ReadOnly, LabelText( "Total Currency Cost" ) ] public int incremental_currency_cost;

    
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    [ Button() ]
    public void Create()
    {
		CreateHealth();
		CreateStamina();
		CreateCurrency();
	}

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

    [ Button() ]
    public void CreateStamina()
    {
        EditorUtility.SetDirty( incremental_stamina );

		IncrementalStaminaData[] array = new IncrementalStaminaData[ data_stamina.level_max ];

        for( int i = 0; i < data_stamina.level_max; i++ )
        {
            var ratio = (float)i / data_stamina.level_max;

            var data = new IncrementalStaminaData();

            data.incremental_stamina_deplete = DOVirtual.EasedValue( data_stamina.range_deplete.x, data_stamina.range_deplete.y, ratio, data_stamina.ease_deplete );
            data.incremental_stamina_deplete_capacity = DOVirtual.EasedValue( data_stamina.range_deplete_capacity.x, data_stamina.range_deplete_capacity.y, ratio, data_stamina.ease_deplete_capacity );
            data.incremental_stamina_recover = DOVirtual.EasedValue( data_stamina.range_recover.x, data_stamina.range_recover.y, ratio, data_stamina.ease_recover );
            data.incremental_stamina_cost = Mathf.RoundToInt( DOVirtual.EasedValue( data_stamina.range_cost.x, data_stamina.range_cost.y, ratio, data_stamina.ease_cost ) );

            array[ i ] = data;
        }

        incremental_stamina.SetIncremental( array );
        AssetDatabase.SaveAssets();
    }

    [ Button() ]
    public void CreateCurrency()
    {
        EditorUtility.SetDirty( incremental_currency );

		IncrementalCurrecyData[] array = new IncrementalCurrecyData[ data_currency.level_max ];

        for( int i = 0; i < data_currency.level_max; i++ )
        {
            var ratio = (float)i / data_currency.level_max;

            var data = new IncrementalCurrecyData();

            data.incremental_currency_gain_value = DOVirtual.EasedValue( data_currency.range_gain_value.x, data_currency.range_gain_value.y, ratio, data_currency.ease_gain_value );
            data.incremental_currency_gain_rate = DOVirtual.EasedValue( data_currency.range_gain_rate.x, data_currency.range_gain_rate.y, ratio, data_currency.ease_gain_rate );
            data.incremental_currency_cost = Mathf.RoundToInt( DOVirtual.EasedValue( data_currency.range_cost.x, data_currency.range_cost.y, ratio, data_currency.ease_cost ) );

            array[ i ] = data;
        }

        incremental_currency.SetIncremental( array );
        AssetDatabase.SaveAssets();
    }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnValidate()
    {
		incremental_health_cost   = incremental_health.TotalCost();
		incremental_stamina_cost  = incremental_stamina.TotalCost();
		incremental_currency_cost = incremental_currency.TotalCost();
	}
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

[ Serializable ]
public struct CreateDataCurrency
{
    public int level_max;
    public Vector2 range_gain_value;
    public Ease ease_gain_value;
    public Vector2 range_gain_rate;
    public Ease ease_gain_rate;
    public Vector2 range_cost;
    public Ease ease_cost;
}

[ Serializable ]
public struct CreateStaminaCurrency
{
    public int level_max;
    public Vector2 range_deplete;
    public Ease ease_deplete;
    public Vector2 range_deplete_capacity;
    public Ease ease_deplete_capacity;
    public Vector2 range_recover;
    public Ease ease_recover;
    public Vector2 range_cost;
    public Ease ease_cost;
}