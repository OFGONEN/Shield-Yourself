/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor;

[ CreateAssetMenu( fileName = "system_skybox", menuName = "FF/System/Skybox" ) ]
public class SkyboxSystem : ScriptableObject
{
#region Fields
    [ SerializeField ] SkyboxMaterialData[] skybox_materials;
    [ SerializeField ] Material skybox_material_default;
    [ SerializeField ] float skybox_lerp_duration;
    [ SerializeField ] Ease skybox_lerp_ease;
    [ LabelText( "Texture Change Threshold" ), SerializeField ] float skybox_lerp_percentage;
    
    [ ShowInInspector, ReadOnly ] int skybox_index_current;
    [ ShowInInspector, ReadOnly ] int skybox_index_target;
    [ ShowInInspector, ReadOnly ] float skybox_lerp_amount;
    [ ShowInInspector, ReadOnly ] bool skybox_lerping;

	RecycledTween recycledTween = new RecycledTween();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void SetDefault()
    {
		skybox_index_current = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.Skybox_Key, 0 );
		skybox_material_default.CopyPropertiesFromMaterial( skybox_materials[ skybox_index_current ].skybox_material );
	}

    [ Button() ]
    public void LerpToSkybox( int index )
    {
		skybox_lerp_amount  = 0;
		skybox_index_target = index;
		skybox_lerping      = true;

		recycledTween.Recycle( DOTween.To( GetLerp, SetLerp, 1, skybox_lerp_duration ).SetEase( skybox_lerp_ease ), OnSkyboxChanged );
	}
#endregion

#region Implementation
    void OnSkyboxChanged()
    {
		skybox_index_current = skybox_index_target;
		PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.Skybox_Key, skybox_index_current );
	}

    //todo test setting null
    //todo test textures

    float GetLerp() 
    {
		return skybox_lerp_amount;
	}

    void SetLerp( float value )
    {
		skybox_lerp_amount = value;
		skybox_material_default.Lerp( skybox_materials[ skybox_index_current ].skybox_material, skybox_materials[ skybox_index_target ].skybox_material, skybox_lerp_amount );

        if( skybox_lerping && recycledTween.Tween.ElapsedPercentage() >= skybox_lerp_percentage )
        {
            FFLogger.Log( "Setting Textures" );
			skybox_lerping = false;

			var targetMaterial = skybox_materials[ skybox_index_target ];

            // Texture
			skybox_material_default.SetTexture( "_TwinklingTexture", targetMaterial.skybox_texture_twinkling );
			skybox_material_default.SetTexture( "_SunTexture", targetMaterial.skybox_texture_sun );
			skybox_material_default.SetTexture( "_MoonTexture", targetMaterial.skybox_texture_moon );
            // CUBE Map
			skybox_material_default.SetTexture( "_BackgroundCubemap", targetMaterial.skybox_cubemap_background );
			skybox_material_default.SetTexture( "_CloudsCubemap", targetMaterial.skybox_cubemap_clouds );
			skybox_material_default.SetTexture( "_PatternCubemap", targetMaterial.skybox_cubemap_pattern );
			skybox_material_default.SetTexture( "_StarsCubemap", targetMaterial.skybox_cubemap_starts );
		}
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
    [ Button() ]
    void SetSkyboxMaterialData()
    {
		EditorUtility.SetDirty( this );

		for( var i = 0; i < skybox_materials.Length; i++ )
        {
			var data = skybox_materials[ i ];
			var skyboxMaterial = data.skybox_material;

            // Texture
			data.skybox_texture_twinkling = skyboxMaterial.GetTexture( "_TwinklingTexture" );
			data.skybox_texture_sun       = skyboxMaterial.GetTexture( "_SunTexture" );
			data.skybox_texture_moon      = skyboxMaterial.GetTexture( "_MoonTexture" );
            // CUBE Map
			data.skybox_cubemap_background = skyboxMaterial.GetTexture( "_BackgroundCubemap" );
			data.skybox_cubemap_clouds     = skyboxMaterial.GetTexture( "_CloudsCubemap" );
			data.skybox_cubemap_pattern    = skyboxMaterial.GetTexture( "_PatternCubemap" );
			data.skybox_cubemap_starts     = skyboxMaterial.GetTexture( "_StarsCubemap" );

			skybox_materials[ i ] = data;
		}

		AssetDatabase.SaveAssets();
	}
#endif
#endregion
}