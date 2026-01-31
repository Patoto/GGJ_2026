using System;
using UnityEngine;

namespace GGJ_2026
{
	public class CatchEvent : MyMonoBehaviour
	{
        public static Action onStartedWatching;
        public static Action onStoppedWatching;

        public void StartWatching()
		{
			onStartedWatching?.Invoke();
		}

		public void StopWatching()
		{			
			onStoppedWatching?.Invoke();
		}
	}
}