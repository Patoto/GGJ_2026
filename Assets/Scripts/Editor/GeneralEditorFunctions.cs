#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace PortalRollerCoaster
{
    public static class GeneralEditorFunctions
    {
        [MenuItem(EditorUtils.MENU_ITEM_PATH_PREFIX + nameof(DeletePersistentData))]
        public static void DeletePersistentData()
        {
            FileUtil.DeleteFileOrDirectory(Application.persistentDataPath);
        }
    }
}
#endif