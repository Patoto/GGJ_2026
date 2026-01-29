using UnityEngine;

namespace PortalRollerCoaster
{
	public class DeveloperCanvas : MyMonoBehaviour
	{
        private void Start()
        {
            gameObject.SetActive(Utils.IsInDevelopmentBuild());
        }
    }
}