﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision) {
        
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Door") {
            if (Input.GetKey(KeyCode.UpArrow)) {
                SceneManager.LoadScene("_Complete-Game");
            }

        }
    }
}
