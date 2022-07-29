/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "pool_ui_particle", menuName = "FF/Data/Pool/UI Particle Pool" ) ]
public class UIParticlePool : ComponentPool< UIParticle > 
{
    [ Button() ]
    public void Spawn( Vector3 screenStartPosition, Vector3 screenWorldPosition, string value, UnityMessage delegateMethod )
    {
		GetEntity().Spawn( screenStartPosition, screenWorldPosition, value, delegateMethod );
	}
}