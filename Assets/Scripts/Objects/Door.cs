using UnityEngine;

namespace GGJ_2026
{
	public class Door : MyMonoBehaviour
	{
		[SerializeField] private Animator animator;

		private const string OPEN_ANIMATION_NAME = "Open";
		private const string CLOSE_ANIMATION_NAME = "Close";

		public void Open()
		{
			animator.PlayAnimationFromStart(OPEN_ANIMATION_NAME);
		}

		public void Close()
		{
			animator.PlayAnimationFromStart(CLOSE_ANIMATION_NAME);
		}
	}
}