using System;
using QFSW.QC;
using UnityEngine;

namespace GGJ_2026
{
    [CommandPrefix(Utils.PROJECT_NAME + ".")]
    public class DeveloperCommands : MyMonoBehaviour
    {
        [SerializeField] private QuantumConsole quantumConsole;

        public static Action onPressedF;

        private void Update()
        {
            CheckForCommands();
        }

        private void CheckForCommands()
        {
            if (!quantumConsole.IsActive)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    GameManager.instance.scenesManager.ResetScene();
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    Utils.PauseEditor();
                }
                if (Input.GetKeyDown(KeyCode.T))
                {
                    ToggleTimeScale();
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    onPressedF?.Invoke();
                }
            }
        }

        public static void ToggleTimeScale()
        {
            float newTimeScale = 1f;
            if (Time.timeScale.IsAlmostEqualToFloat(1f))
            {
                newTimeScale = 10f;
            }
            Time.timeScale = newTimeScale;
        }
    }
}