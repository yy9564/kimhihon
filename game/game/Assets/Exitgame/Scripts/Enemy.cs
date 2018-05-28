using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int playerDamage; // 적이 플레이어를 공격할때 뺄셈할 음식 포인트

    private Animator animator;
    private Transform target; // 플레이어의 위치 저장 및 적이 어디로 향할지 알려줌
    private bool skipMove; // 적이 턴마다 움직이게 하는데 사용

	// Use this for initialization
	protected override void Start ()
    {
        GameManager.instance.AddEnemyToList(this); // Enemy 스크립트가 자기 스스로를 게임 매니저의 적 리스트에 더함
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start(); // 부모 클래스에 있는 Start 호출
         
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove) // 참이면 적의 턴을 스킵
        {
            skipMove = false;
            return; 
        } // 매번 턴이 돌아올 때만 움직일 수 있게 함  
        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy() // 적들이 움직이려 할 때 게임 매니저에 의해 호출
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) // 우리의 적과 플레이어가 같은 열에 속한다는 의미
        yDir = target.position.y > transform.position.y ? 1 : -1; // 같은 열에 있다면 target 위치의 y좌표가 transform위치의 y좌표보다 큰지 체크 만약 그렇다면 플레이어를 향해 위로 이동 아니라면 아래로 이동
        
        else
            xDir = target.position.x > transform.position.x ? 1 : -1; // 1 오른쪽 -1 왼쪽

        AttemptMove<Player>(xDir, yDir); // player를 일반형으로 입력 이동할 x방향과 y방향도 입력
    }

    protected override void OnCantMove<T>(T component) // OnCantMove는 플레이어가 점거중인 공간으로 적이 이동하려고 시도 할 때 호출
    {
        Player hitPlayer = component as Player; // Player타입의 변수 hitPlayer를 선언하고 입력한 컴포넌트를 Player로 형변환 해서 =연산

        animator.SetTrigger("enemyAttack"); // enemyAttack 트리거를 발동하기 위해 SetTrigger 함수를 사용

        hitPlayer.LoseFood(playerDamage); // 플레이어의 LoseLife를 호출하고 적의 공격으로 인해 잃어버릴 음식 점수가 될 playerDamage를 입력

    }
}
