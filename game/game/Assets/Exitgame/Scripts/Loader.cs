using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {
    public GameObject gameManager; // 게임 매니저 인스턴스화 체크 없다면 프리펩으로 부터 인스턴스화

	// Use this for initialization
	void Awake () {
        if (GameManager.instance == null)
            Instantiate(gameManager); // null과 같다면 게임 매니저 프리펩 인스턴스화
	}
	
}
