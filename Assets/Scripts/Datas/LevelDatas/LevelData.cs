using System;
using UnityEngine;

namespace PortalRollerCoaster
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