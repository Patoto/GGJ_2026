using UnityEngine;

namespace GGJ_2026
{
    public class GameStartHandler : MyMonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        private void Start()
        {
            gameManager.Setup();
            GoToNextScene();
        }

        private void GoToNextScene()
        {
#if UNITY_EDITOR
            if (Bootstrapper.StartedGameInGameStartScene())
            {
                GoToCurrentLevelCounterScene();
            }
            else
            {
                gameManager.scenesManager.GoToScene(Bootstrapper.firstLoadedScenePath);
            }
#else
            GoToCurrentLevelCounterScene();
#endif
        }

        private void GoToCurrentLevelCounterScene()
        {
            //GameManager.instance.scenesManager.GoToCurrentLevelCounterScene();
            GameManager.instance.scenesManager.GoToScene(GameManager.instance.scenesManager.testingSceneSceneField);
        }
    }
}