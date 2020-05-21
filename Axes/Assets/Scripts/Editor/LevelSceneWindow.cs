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
        // defaultX = (Axis)EditorGUILayout.EnumPopup("X Property", defaultX);
        // defaultY = (Axis)EditorGUILayout.EnumPopup("Y Property", defaultY);

        if (GUILayout.Button("Create")) {
            CreateScene();
        }

        EditorGUILayout.LabelField("Instructions for Level creation:");
        EditorGUILayout.LabelField("1. Enter an unused World and Level above.");
        EditorGUILayout.LabelField("2. Create.");
        EditorGUILayout.LabelField("3. Unfocus from the Unity Editor and refocus (to fix no-show bug).");
        EditorGUILayout.LabelField("4. If the level hasn't already opened, navigate to Assets/Scenes/Levels/Level<World>-<Level>.unity");
        EditorGUILayout.LabelField("5. In the Project View, Navigate to Assets/Resources/Prefabs/Levels");
        EditorGUILayout.LabelField("6. In the Scene Hierarchy, find _TemplateGrid, and click and drag it into the Levels folder.");
        EditorGUILayout.LabelField("7. Choose Prefab Variant, and rename it to Level<World>-<Level>");
        EditorGUILayout.LabelField("8. To edit the level at any time, open the level prefab you just created.");
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");

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
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorSceneManager.OpenScene(newPath, OpenSceneMode.Single);
        Close();
    }
}