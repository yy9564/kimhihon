using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public Sprite dmgSprite; // 플레이어가 벽을 한번 때렸을 때 보여줄 스프라이트
    public int hp = 3; // 벽의 체력을 위해 hp선언

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void DamageWall (int loss)
    {
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0) // hp값이 0이하라면 게임오브젝트 비활성화
            gameObject.SetActive(false);
    }
}
