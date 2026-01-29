using UnityEngine;

namespace PortalRollerCoaster
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
            GameManager.instance.scenesManager.GoToCurrentLevelCounterScene();
        }
    }
}