﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveScene : MonoBehaviour {
    public static float posX = -20f;
    public static float posZ;
    public static string state = null;

    // Use this for initialization
    void Start () {
        transform.position = new Vector3(posX, -3.354491f, 0);
	}
	
	// Update is called once per frame
	void Update () {
        posX = transform.position.x;
        posZ = transform.position.z;

        /*if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            transform.position = new Vector3(posX, -3.351106f, -5);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            transform.position = new Vector3(posX, -3.351106f, 5);
        }*/
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Door") {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                SceneManager.LoadScene("Main");
            }
        }
        if (collision.gameObject.tag == "GO") {
            if (Input.GetKey(KeyCode.UpArrow)) {
                int SceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (SceneManager.sceneCountInBuildSettings > SceneIndex) {
                    SceneManager.LoadScene(SceneIndex);
                }
            }
        }
        if (collision.gameObject.tag == "BACK")
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                int SceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
                if (SceneManager.sceneCountInBuildSettings > SceneIndex)
                {
                    SceneManager.LoadScene(SceneIndex);
                }
            }
        }
        if (collision.gameObject.tag == "move3dgo")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SceneManager.LoadScene("mainHall2-center");
            }
        }
        if (collision.gameObject.tag == "move3dback")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SceneManager.LoadScene("mainHall2-center");
            }
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "move3d")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("collision");
                SceneManager.LoadScene("mainHallmainHall2-center");
            }
        }
    }  
    */
}
