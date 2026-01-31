using System;
using System.Collections;
using UnityEngine;

namespace GGJ_2026
{
	public class LevelHandler : MyMonoBehaviour
	{
        public static Action onFinishedStartCoroutine;

        private void Start()
        {
			StartCoroutine(MyStartCoroutine());
        }

        private IEnumerator MyStartCoroutine()
		{
			yield return GameManager.instance.transitionsManager.TryToPlayTransitionInCoroutine();
			onFinishedStartCoroutine?.Invoke();
		}
    }
}