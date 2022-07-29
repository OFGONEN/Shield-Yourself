/* Created by and for usage of FF Studios (2021). */

using Lean.Touch;
using UnityEngine;

namespace FFStudio
{
	public delegate void ChangeEvent();
	public delegate void DrawShape( Camera cam );
    public delegate void TriggerMessage( Collider other );
    public delegate void CollisionMessage( Collision collision );
	public delegate void UnityMessage();
	public delegate void Vector2Message( Vector2 value );
	public delegate void LeanFingerDelegate( LeanFinger finger );
	public delegate void ParticleEffectStopped( ParticleEffect effect );
}
