using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class LevelSceneWindow : EditorWindow {

    public static LevelSceneWindow window;

    private int world;
    private int level;

    private Axis defaultX;
    private Axis defaultY;

    [MenuItem("File/New Level Scene %&n", priority = 1)]
    public static void Init () {
        GetWindow(typeof(LevelSceneWindow));
    }

    private void OnGUI () {
        world = EditorGUILayout.IntField("World", world);
        level = EditorGUILayout.IntField("Level", level);
        defaultX = (Axis)EditorGUILayout.EnumPopup("X Property", defaultX);
        defaultY = (Axis)EditorGUILayout.EnumPopup("Y Property", defaultY);

        if (GUILayout.Button("Create")) {
            CreateScene();
        }
    }

    private void CreateScene () {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        
        string templatePath = "Assets/Scenes/Levels/_Template/_Template.unity";
        string newName = "Level" + world.ToString() + "-" + level.ToString() + ".unity";
        string newPath = "Assets/Scenes/Levels/" + newName;
        try {
            FileUtil.CopyFileOrDirectory(templatePath, newPath);
            Debug.Log(newName + " created at " + newPath);
        } catch {
            Debug.Log(newPath + " already exists!");
            EditorSceneManager.OpenScene(newPath);
        }

        Close();
    }
}