using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneChecker
{
    [InitializeOnLoad]
    public class EditorInit
    {
        // private static readonly string[] IgnoreScenes = new string[]
        // {
        //     "_SkillTool",
        //     "_DamageTextTool",
        //     "_UI",
        //     "_UI2",
        //     "DataSimulator",
        //     "DataSimulatorSimple"
        // };
        //
        // private static bool CheckIgnoreScene(string sceneName)
        // {
        //     return IgnoreScenes.Contains(sceneName);
        // }
        //
        // static EditorInit()
        // {
        //     EditorSceneManager.sceneOpened += SetStartScene;
        // }
        //
        // private static void SetStartScene(Scene scene, OpenSceneMode mode)
        // {
        //     if (CheckIgnoreScene(scene.name))
        //     {
        //         SetStartScene(scene.path);
        //         return;
        //     }
        //     
        //     var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        //     SetStartScene(pathOfFirstScene);
        // }
        //
        // private static void SetStartScene(string path)
        // {
        //     var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
        //     EditorSceneManager.playModeStartScene = sceneAsset;
        //     
        //     Debug.LogError(sceneAsset.name + "씬에서 시작합니다.");
        // }
    }
}
