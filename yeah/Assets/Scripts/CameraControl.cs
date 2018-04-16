using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public GameObject player;
    //private Vector2 velocity;
    //public float smoothTimeX;
    public static float rotY;

    // Use this for initialization
    void Start () {
        //player = GameObject.FindGameObjectWithTag
        rotY = gameObject.transform.rotation.eulerAngles.y;
    }
	
	// Update is called once per frame
	void Update () {
        float posX = player.transform.position.x;
        rotY = gameObject.transform.rotation.eulerAngles.y;

        //transform.position = new Vector3(posX, 0, -10);

        if (Input.GetKeyDown(KeyCode.LeftArrow) && (rotY==0)){
            transform.Rotate(new Vector3(0, 180, 0));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && (rotY == 180)) {
            transform.Rotate(new Vector3(0,-180,0));
        }
    }
}
