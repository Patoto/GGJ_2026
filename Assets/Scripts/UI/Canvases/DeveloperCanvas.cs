using UnityEngine;

namespace GGJ_2026
{
	public class DeveloperCanvas : MyMonoBehaviour
	{
        private void Start()
        {
            gameObject.SetActive(Utils.IsInDevelopmentBuild());
        }
    }
}