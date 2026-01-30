using System;
using UnityEngine;

namespace GGJ_2026
{
	public class LevelData : MyMonoBehaviour
	{
		[SerializeField] public SceneField sceneField;

        public int GetIndex()
		{
			return GameManager.instance.levelsManager.GetLevelDataIndex(this);
		}
    }
}