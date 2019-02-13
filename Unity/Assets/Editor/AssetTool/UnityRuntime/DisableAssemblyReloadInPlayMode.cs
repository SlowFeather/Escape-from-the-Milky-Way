using UnityEditor;
/// <summary>
/// Unity运行中禁止加载代码
/// </summary>
//[InitializeOnLoad]
//public class DisableAssemblyReloadInPlayMode
//{
//    #if DisableAssemblyReloadInPlayMode
//    private static bool Locked;

//    static DisableAssemblyReloadInPlayMode()
//    {
//        EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
//    }

//    private static void OnPlaymodeStateChanged()
//    {
//        if (EditorApplication.isPlaying && !Locked)
//        {
//            EditorApplication.LockReloadAssemblies();
//            Locked = true;
//        }
//        else if (!EditorApplication.isPlaying && Locked)
//        {
//            EditorApplication.UnlockReloadAssemblies();
//            Locked = false;
//        }
//    }
//    #endif
//}
