using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PortalRollerCoaster
{
    public class SelectLevelButton : MyMonoBehaviour//, IEventSubscriberDeclarator
    {
/*		[SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;

        private LevelData levelData;

        public void SubscribeToEnableEvents()
        {
            Button.onPointerClick += OnButtonPointerClick;
        }

        public void UnsubscribeFromEnableEvents()
        {
            Button.onPointerClick -= OnButtonPointerClick;
        }

        public static SelectLevelButton CreateSelectLevelButton(SelectLevelButton originalSelectLevelButton, GameObject parentGameObject, LevelData levelData)
        {
            SelectLevelButton selectLevelButton = Instantiate(originalSelectLevelButton, parentGameObject.transform);
            selectLevelButton.Setup(levelData);
            return selectLevelButton;
        }

        private void Setup(LevelData levelData)
        {
            this.levelData = levelData;
            UpdateText();
        }

        private void UpdateText()
        {
            SetLevelNumber(GameManager.instance.levelsManager.GetLevelDataNumber(levelData));
        }

        private void SetLevelNumber(int houseNumber)
        {
            text.text = $"LEVEL {houseNumber}";
        }

        private void OnButtonPointerClick(PointerListener pointerListener, PointerEventData pointerEventData)
        {
            if (pointerListener == button)
            {
                GameManager.instance.persistentDataManager.persistentData.SetCurrentLevelCounter(levelData.GetIndex());
                GameManager.instance.scenesManager.GoToCurrentLevelCounterScene();
            }
        }*/
    }
}