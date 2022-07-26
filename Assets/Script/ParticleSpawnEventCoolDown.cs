/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "event_pfx_spawn_cooldown", menuName = "FF/Event/Particle Cooldown GameEvent" ) ]
public class ParticleSpawnEventCoolDown : ScriptableObject
{
	public ParticleSpawnEvent particleSpawnEvent;
    public float rate;

	float cooldown;

	public void Raise( string alias, Vector3 position, Transform parent = null, float size = 1f )
	{
        if( Time.time >= cooldown )
        {
			cooldown = Time.time + rate;
			particleSpawnEvent.Raise( alias, position, parent, size );
        }
	}

    public void Default()
    {
		cooldown = 0;
	}
}
