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
			StartCoroutine(StartCoroutine());
        }

        private IEnumerator StartCoroutine()
		{
			yield return GameManager.instance.transitionsManager.TryToPlayTransitionInCoroutine();
			onFinishedStartCoroutine?.Invoke();
		}
    }
}