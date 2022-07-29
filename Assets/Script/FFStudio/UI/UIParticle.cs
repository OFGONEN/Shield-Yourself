/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

namespace FFStudio
{
	public class UIParticle : MonoBehaviour
	{
#region Fields
		[ Title( "SharedVariable" )]
		[ SerializeField ] UIParticlePool pool_ui_particle;
		[ SerializeField ] TextMeshProUGUI ui_text;

		RecycledSequence recycledSequence = new RecycledSequence();
		UnityMessage onSequenceComplete;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
		[ Button() ]
		public void Spawn( Vector3 screenPositionStart, Vector3 screenPositionEnd, string value, UnityMessage onComplete )
		{
			gameObject.SetActive( true );

			onSequenceComplete = onComplete;
			ui_text.text = value;

			transform.position = screenPositionStart;
			var spawnTargetPosition = screenPositionStart + Random.insideUnitCircle.ConvertV3() * GameSettings.Instance.ui_particle_spawn_width * Screen.width / 100f;

			var sequence = recycledSequence.Recycle( OnSequenceComplete )
								.Append( transform
											.DOMove( spawnTargetPosition, GameSettings.Instance.ui_particle_spawn_duration )
											.SetEase( GameSettings.Instance.ui_particle_spawn_ease ) )
								.AppendInterval( GameSettings.Instance.ui_particle_target_waitTime )
								.Append( transform
											.DOMove( screenPositionEnd, GameSettings.Instance.ui_particle_target_duration )
											.SetEase( GameSettings.Instance.ui_particle_target_ease ) );
		}
#endregion

#region Implementation
		void OnSequenceComplete()
		{
			onSequenceComplete?.Invoke();
			pool_ui_particle.ReturnEntity( this );
		}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
	}
}