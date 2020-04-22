
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
	private Transform mainCam;
	public float multiplier = 1f;
	
	public bool ignoreY = true;
	public bool boundY = true;
	public Vector2 textureVBounds;
	public Vector2 cameraYBounds;
	public float yRatio;

	private Vector2 target;
	private float targetX;
	private float targetY;

	private List<Transform> bgs;
	private List<Material> mats;

	private void Awake () {
		mainCam = Camera.main.transform;

		bgs = new List<Transform>();
		mats = new List<Material>();

		foreach (Transform child in transform) {
			if (child.position.z < 0f) {
				Debug.LogError("Invalid child position on " + child.name + ". Z value must be >= 0f");
				child.position = new Vector3(child.position.x, child.position.y, 0f);
			} else {
				bgs.Add(child);
				mats.Add(child.GetComponent<MeshRenderer>().material);
			}
		}

		yRatio = (textureVBounds.y - textureVBounds.x) / (cameraYBounds.y - cameraYBounds.x);
	}

	private void Update () {
		for (int i = 0; i < bgs.Count; i++) {
			targetX = mainCam.position.x * multiplier * 1 / (bgs[i].position.z + 1);
			targetY = (ignoreY ? 0 : (mainCam.position.y - cameraYBounds.x) * yRatio) * 1 / (bgs[i].position.z + 1);
			targetY = Mathf.Clamp(targetY, textureVBounds.x, textureVBounds.y);
			target = new Vector2(targetX, targetY);

			if (bgs[i].GetComponent<ParallaxAutoOffset>()) {
				target += bgs[i].GetComponent<ParallaxAutoOffset>().offset;
			}
			mats[i].SetTextureOffset("_MainTex", target);
		}
	}
}