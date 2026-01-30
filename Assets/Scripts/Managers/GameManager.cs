using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ_2026
{
    public abstract class Manager : MyMonoBehaviour
    {
        public virtual void Setup() { }
    }

    public class GameManager : Manager
    {
        [Header("Managers")]
        [SerializeField] public AudioManager audioManager;
        [SerializeField] public PersistentDataManager persistentDataManager;
        [SerializeField] public TagsManager tagsManager;
        [SerializeField] public ScenesManager scenesManager;
        [SerializeField] public TransitionsManager transitionsManager;
        [SerializeField] public UIManager uiManager;
        [SerializeField] public LevelsManager levelsManager;
        [Header("References")]
        [SerializeField] public EventSubscriber eventSubscriber;

        private static GameManager _instance;

        public static GameManager instance {
            get
            {
                GameManager instance = _instance;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    instance = Extensions.AssetDatabaseExtensions.LoadFirstAssetOfTypeAtPathAndItsSubpaths<GameManager>("Assets/Prefabs/Others/Others");
                }
#endif
                return instance;
            }
            private set
            {
                _instance = value;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SceneManager.sceneLoaded += OnSceneManagerSceneLoaded;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SceneManager.sceneLoaded -= OnSceneManagerSceneLoaded;
        }

        public override void Setup()
        {
            instance = this;
            SetupGeneralGameSettings();
            eventSubscriber.Setup();
            gameObject.GetAllChildrenWithComponent<Manager>().ForEach(iManager => iManager.Setup());
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(true);
        }

        private void SetupGeneralGameSettings()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void OnSceneManagerSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            TryToInitializeAllMyMonoBehaviours();
        }

        private static void TryToInitializeAllMyMonoBehaviours()
        {
            FindObjectsByType<MyMonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList().ForEach(iMyMonoBehaviour => iMyMonoBehaviour.TryToInitialize());
        }
    }
}