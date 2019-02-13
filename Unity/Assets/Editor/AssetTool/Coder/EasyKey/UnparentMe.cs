// unparents selected gameobject in hierarchy (by moving to grandparents if available)

using UnityEditor;

namespace UnityLibrary
{
    /// <summary>
    /// ���Լ��ĸ��������Լ�������ĸ�����
    /// </summary>
    public class UnparentMe
    {
        // https://docs.unity3d.com/ScriptReference/MenuItem.html
        // shift U shortcut key
        [MenuItem("AssetsTool/Coder/EasyKey/Unparent #u")]
        static void UnParent()
        {
            // TODO: add undo
            if (Selection.activeGameObject != null && Selection.activeGameObject.transform.parent != null)
            {
                Selection.activeGameObject.transform.parent = Selection.activeGameObject.transform.parent.parent;
            }
        }
    }
}
