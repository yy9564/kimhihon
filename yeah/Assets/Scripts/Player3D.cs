using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player3D : MonoBehaviour {
    float h, v;
    public float moveSpeed = 0.1f;
    public float rotateSpeed = 100.0f;
    Transform tr;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 moveDir;

        moveDir = (Vector3.forward * v) + (Vector3.right * h);
        moveDir.Normalize();
        tr.Translate(moveDir * moveSpeed * Time.deltaTime);
        //tr.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * Input.GetAxis("Mouse X"));

    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Move")
        {
            Debug.Log("collision");
            SceneManager.LoadScene("mainHall2-1");
        }
    }
}
