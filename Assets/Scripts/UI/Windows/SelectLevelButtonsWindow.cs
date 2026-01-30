using System;
using UnityEngine;
using UnityEngine.UI;

namespace PortalRollerCoaster
{
	public class SelectLevelButtonsWindow : MyMonoBehaviour
	{
		[SerializeField] private VerticalLayoutGroup contentVerticalLayoutGroup;
		[SerializeField] private SelectLevelButton selectLevelButtonPrefab;

        private void Start()
        {
            CreateSelectLevelButtons();
			contentVerticalLayoutGroup.Update();
        }

        private void CreateSelectLevelButtons()
        {
            GameManager.instance.levelsManager.levelDatasList.ForEach(iLevelData => CreateSelectLevelButton(iLevelData));
        }

        private void CreateSelectLevelButton(LevelData levelData)
        {
            SelectLevelButton.CreateSelectLevelButton(selectLevelButtonPrefab, contentVerticalLayoutGroup.gameObject, levelData);
        }
    }
}