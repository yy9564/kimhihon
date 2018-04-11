using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public GameObject player;
    //private Vector2 velocity;
    //public float smoothTimeX;

	// Use this for initialization
	void Start () {
        //player = GameObject.FindGameObjectWithTag
	}
	
	// Update is called once per frame
	void Update () {
        float posX = player.transform.position.x;

        transform.position = new Vector3(posX, 0, -10);
	}
}
