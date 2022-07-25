/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using DG.Tweening;

namespace FFStudio
{
	public class UIEntity : MonoBehaviour
	{
#region Fields
		public RectTransform uiTransform;
		public RectTransform destinationTransform;
		[ HideInInspector ] public Vector3 startPosition;
		[ HideInInspector ] public Vector3 startScale;

		RecycledTween recycledTween = new RecycledTween();
#endregion

#region UnityAPI
		public virtual void Start()
		{
			startPosition = uiTransform.position;
			startScale    = uiTransform.localScale;
		}
#endregion

#region API
		public virtual Tween GoToTargetPosition()
		{
			recycledTween.Recycle( uiTransform.DOMove( destinationTransform.position, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) );
			return recycledTween.Tween;
		}

		public virtual Tween GoToStartPosition()
		{
			recycledTween.Recycle( uiTransform.DOMove( startPosition, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) );
			return recycledTween.Tween;
		}

		public virtual Tween Appear()
		{
			recycledTween.Recycle( uiTransform.DOScale( startScale, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );
			return recycledTween.Tween;
		}

		public virtual Tween Disappear()
		{
			recycledTween.Recycle( uiTransform.DOScale( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );
			return recycledTween.Tween;
		}
#endregion
	}
}