using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

/**
    Klasse eines default Tile, auf dem weitere Gegenstände abgebildet werden
**/

public class GameObjectTile : Tile {
    private Sprite default_Sprite;
    private static UnityEngine.GameObject object_test;
    private static bool c = false;


    public override bool StartUp (Vector3Int position, ITilemap tilemap, UnityEngine.GameObject go) {
        Destroy(object_test);
        return true;
    }
    
    public override void GetTileData (Vector3Int position, ITilemap tilemap, ref TileData tileData) {
        this.default_Sprite = Resources.Load<Sprite>("Art/Sprites/Debug/Placeholder");
        tileData.sprite = this.default_Sprite;

        if (!c) {
            object_test = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
            c = true;
        }

        tileData.gameObject = object_test;
    }


}
