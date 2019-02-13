using UnityEngine;
using UnityEditor;

public class GenScriptableObject
{
    [MenuItem("AssetsTool/Coder/Create/Template Configuration Table")]
    static void Generate()
    {
		var asset = ScriptableObject.CreateInstance<ScriptableObject>();
        ProjectWindowUtil.CreateAsset(asset, "templatetable.asset");
    }
}