#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class DefaultSceneLoader
{
    static DefaultSceneLoader()
    {
        EditorApplication.playModeStateChanged += LoadDefaultScene;
    }

    static void LoadDefaultScene(PlayModeStateChange state)
    {
        if(Utilities.BuildUtilities.IsClientBuild())
            return;
        
        Debug.Log("Loading default scene...");
        
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        
        Scene currentScene = SceneManager.GetActiveScene();
        // Scene defaultScene = SceneManager.GetSceneAt(0);
        
        // Check if the current scene is not the default scene
        if (currentScene.name == "MainMenuScene") // && currentScene.path == defaultScene.path)
            return;

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            SceneManager.LoadScene(0);
        }
    }
}
#endif