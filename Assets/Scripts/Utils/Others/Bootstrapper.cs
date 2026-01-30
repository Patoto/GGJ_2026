#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ_2026
{
    public static class Bootstrapper
    {
        public static string firstLoadedScenePath;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void GoToGameStartScene()
        {
            firstLoadedScenePath = SceneManager.GetActiveScene().path;
            if (!StartedGameInGameStartScene())
            {
                SceneManager.LoadScene(ScenesManager.GAME_START_SCENE_NAME);
            }
        }

        public static bool StartedGameInGameStartScene()
        {
            return Utils.ScenePathToSceneName(firstLoadedScenePath).Equals(ScenesManager.GAME_START_SCENE_NAME);
        }

        public static bool FirstLoadedSceneIsCurrentScene()
        {
            return firstLoadedScenePath == SceneManager.GetActiveScene().path;
        }
    }
}
#endif