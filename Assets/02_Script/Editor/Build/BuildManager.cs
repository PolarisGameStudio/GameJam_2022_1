using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager.UI;
#endif

public class BuildManager : MonoBehaviour
{
    /// <summary>
    /// Symbols that will be added to the editor
    /// </summary>
    public static readonly string[] DevSymbols = new string[]
    {
        "__DEV"
    };

#if UNITY_EDITOR

    private const string _devBuildPath = "Dev";
    private const string _productBuildPath = "Product";

    [MenuItem("Build/Build Dev Version")]
    public static void BuildDevelopmentVersion()
    {
        SRDebugger.Settings.Instance.IsEnabled = true;

        SetDefinesDev();

        Build(_devBuildPath, fileName: $"Dev_", BuildOptions.Development);
    }


    [MenuItem("Build/Build Product Version")]
    public static void BuildProductVersion()
    {
        SRDebugger.Settings.Instance.IsEnabled = false;

        SetDefinesProduct();

        Build(_productBuildPath, fileName: $"Product_");
    }

    public static void SetDefinesDev()
    {
        SRDebugger.Settings.Instance.IsEnabled = true;

        string definesString =
            PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        allDefines.AddRange(DevSymbols.Except(allDefines));

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }

    public static void SetDefinesProduct()
    {
        SRDebugger.Settings.Instance.IsEnabled = false;

        string definesString =
            PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        allDefines.RemoveAll(x => DevSymbols.Contains(x) == true);

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }


    public static void Build(string path, string fileName, BuildOptions buildOption = BuildOptions.None)
    {
        string buildPath = Application.persistentDataPath + $"/Build/{path}/{fileName}{PlayerSettings.Android.bundleVersionCode++}.apk";

        var scenes = EditorBuildSettings.scenes;

        var sceneList = new List<string>();

        foreach (var scene in scenes)
        {
            if (scene.enabled)

                sceneList.Add(scene.path);
        }

        var sceneArray = sceneList.ToArray();

        BuildPipeline.BuildPlayer(sceneArray, buildPath, BuildTarget.Android, buildOption);
    }

#endif
}