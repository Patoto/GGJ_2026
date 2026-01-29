using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PortalRollerCoaster
{
    public class ScenesManager : Manager
    {
        public const string GAME_START_SCENE_NAME = "GameStart";

        public void ResetScene()
        {
            GameManager.instance.StopAllCoroutines();
            GoToScene(GetActiveScenePath());
        }

        public string GetActiveScenePath()
        {
            return SceneManager.GetActiveScene().path;
        }

        public void GoToScene(string sceneNameOrPath)
        {
            SceneManager.LoadScene(sceneNameOrPath);
        }

        public void TransitionToCurrentLevelCounterScene()
        {
            StartCoroutine(TransitionToCurrentLevelCounterSceneCoroutine());
        }

        private IEnumerator TransitionToCurrentLevelCounterSceneCoroutine()
        {
            yield return GameManager.instance.transitionsManager.PlayTransitionOutCoroutine();
            GoToCurrentLevelCounterScene();
        }

        public void GoToCurrentLevelCounterScene()
        {
            GoToScene(GameManager.instance.levelsManager.GetCurrentLevelCounterLevelData().sceneField);
        }
    }
}