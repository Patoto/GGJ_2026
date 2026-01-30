using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ_2026
{
	public class LevelsManager : Manager
	{
		[SerializeField] public List<LevelData> levelDatasList = new();

		public LevelData GetCurrentLevelCounterLevelData()
		{
			return levelDatasList[GameManager.instance.persistentDataManager.persistentData.currentLevelCounter % levelDatasList.Count];
		}

        public int GetLevelDataNumber(LevelData levelData)
		{
			return GetLevelDataIndex(levelData) + 1;
		}

        public int GetLevelDataIndex(LevelData levelData)
		{
			return levelDatasList.IndexOf(levelData);
		}
    }
}