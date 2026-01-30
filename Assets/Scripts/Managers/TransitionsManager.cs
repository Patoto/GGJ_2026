using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ_2026
{
    public class TransitionsManager : Manager
    {
        [SerializeField] private Transform defaultTransitionParent;
        [SerializeField] private List<Transition> transitionPrefabsList = new();

        private Transition currentTransition;

        public IEnumerator PlayTransitionOutCoroutine(float seconds = 0.5f, Transition.Type transitionType = Transition.Type.InvertedMask, Transform transitionParent = null)
        {
            Transition transitionPrefab = GetTransitionPrefabOfType(transitionType);
            if (transitionParent == null)
            {
                transitionParent = defaultTransitionParent;
            }
            TryToDestroyCurrentTransition();
            currentTransition = Instantiate(transitionPrefab, transitionParent);
            yield return currentTransition.PlayTransitionOutCoroutine(seconds);
        }

        public IEnumerator TryToPlayTransitionIn()
        {
            return StartCoroutine(TryToPlayTransitionInCoroutine());
        }

        public IEnumerator TryToPlayTransitionInCoroutine()
        {
            if (currentTransition != null)
            {
                yield return currentTransition.PlayTransitionInCoroutine();
            }
        }

        private void TryToDestroyCurrentTransition()
        {
            if (currentTransition != null)
            {
                Destroy(currentTransition);
            }
        }

        private Transition GetTransitionPrefabOfType(Transition.Type transitionType)
        {
            return transitionPrefabsList.FirstOrDefault(iTransition => iTransition.type == transitionType);
        }

        public void PlayTransitionOutAndGoToScene(string sceneNameOrPath, float seconds = 0.5f, Transition.Type transitionType = Transition.Type.InvertedMask)
        {
            StartCoroutine(PlayTransitionOutAndGoToSceneCoroutine(sceneNameOrPath, seconds, transitionType));
        }

        private IEnumerator PlayTransitionOutAndGoToSceneCoroutine(string sceneNameOrPath, float seconds = 0.5f, Transition.Type transitionType = Transition.Type.InvertedMask)
        {
            yield return PlayTransitionOutCoroutine(seconds: seconds, transitionType: transitionType);
            GameManager.instance.scenesManager.GoToScene(sceneNameOrPath);
        }
    }
}