using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorTools : MonoBehaviour
{
    [MenuItem("Assets/Copy Name")]
    private static void CopyName()
    {
        TextEditor te = new TextEditor();
        te.text = Selection.activeObject.name;
        te.OnFocus();
        te.Copy();
        Debug.Log(string.Format("Copy name complete : <color=#ffd400>{0}</color>", te.text));
    }
    /// <summary>
    /// 增加一个SlowTool菜单下的printName选项，并且只有选中物体时才可以
    /// </summary>
    [MenuItem("AssetsTool/Show SelectedName")]
    static void PrintName()
    {
        Debug.Log(string.Format("Selected Transform name is : <color=#ffd400>{0}</color>",Selection.activeTransform.gameObject.name));
    }
    [MenuItem("AssetsTool/Show SelectedName", true)]
    static bool Validate()
    {
        return Selection.activeTransform != null;
    }
    /// <summary>
    /// 复制选中场景的名字
    /// </summary>
    [MenuItem("AssetsTool/Copy Name")]
    static void CopySelectName()
    {
        TextEditor te = new TextEditor();
        te.text = Selection.activeTransform.gameObject.name;
        te.OnFocus();
        te.Copy();
        Debug.Log(string.Format("Copy name complete : <color=#ffd400>{0}</color>", te.text));
    }
    [MenuItem("AssetsTool/Copy Name", true)]
    static bool ValidateCopy()
    {
        return Selection.activeTransform != null;
    } 
}
