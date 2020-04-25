using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE: DO NOT USE

public class BackgroundSelector : MonoBehaviour {
    public enum Background {
        Forest1,
    }

    public Background background;
    private const string bgResourcePath = "Prefabs/Backgrounds/";

    void OnValidate () {
        foreach(Transform child in transform) {
            DestroyImmediate(child);
        }
        
        string filename = bgResourcePath;
        switch (background) {
            case Background.Forest1:
                filename += "Forest1Background";
                break;
        }

        Object obj = Instantiate(Resources.Load(filename), transform.position + new Vector3(-100f, 100f), Quaternion.identity, transform);
        obj.name = obj.name.Replace("(Clone)", "");
    }
}