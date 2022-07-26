/* Created by and for usage of FF Studios (2021). */

using DG.Tweening;

namespace FFStudio
{
	public class RecycledSequence
	{
		public int ID;

		UnityMessage onComplete;
		Sequence sequence;

		public Sequence Sequence => sequence;

		public RecycledSequence()
		{
			ID = 0;
		}

		public RecycledSequence( int id )
		{
			ID = id;
		}

		public Sequence Recycle( UnityMessage onComplete )
		{
			sequence = sequence.KillProper();

			this.onComplete = onComplete;

			sequence = DOTween.Sequence();
			sequence.OnComplete( OnComplete_Safe );

#if UNITY_EDITOR
			sequence.SetId( "_ff_RecycledSequence" );
#endif

			return sequence;
		}

		public Sequence Recycle()
		{
			sequence = sequence.KillProper();

			sequence = DOTween.Sequence();
			sequence.OnComplete( OnComplete_Safe );

#if UNITY_EDITOR
			sequence.SetId( "_ff_RecycledSequence" );
#endif
			
			return sequence;
		}

		public bool IsPlaying()
		{
			return sequence != null && sequence.IsPlaying();
		}

		public void Kill()
		{
			sequence = sequence.KillProper();
		}

		void OnComplete_Safe()
		{
			sequence = null;
			onComplete?.Invoke();
		}
	}
}