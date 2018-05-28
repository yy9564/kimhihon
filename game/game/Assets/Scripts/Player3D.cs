using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player3D : MonoBehaviour {
    //float h, v;
    //public float rotateSpeed = 100.0f;
    public float speed = 10.0f;
    Vector3 lookDirection;
    Transform tr;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        /*
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 moveDir;

        moveDir = (Vector3.forward * v) + (Vector3.right * h);
        moveDir.Normalize();
        tr.Translate(moveDir * moveSpeed * Time.deltaTime);
        //tr.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * Input.GetAxis("Mouse X"));
        */
        if (Input.GetKey(KeyCode.UpArrow) ||
           Input.GetKey(KeyCode.DownArrow) ||
           Input.GetKey(KeyCode.RightArrow) ||
           Input.GetKey(KeyCode.LeftArrow))
        {

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            lookDirection = x * Vector3.right + z * Vector3.forward;

            transform.rotation = Quaternion.LookRotation(lookDirection);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Move")
        {
            Debug.Log("collision");
            SceneManager.LoadScene("mainHall1-3");
        }
        if (collision.gameObject.tag == "move3dback")
        {
            Debug.Log("collision");
            SceneManager.LoadScene("mainHall1-3");
        }
        if (collision.gameObject.tag == "move3dgo")
        {
            Debug.Log("collision");
            SceneManager.LoadScene("mainHall1-3");
        }
    }
}
