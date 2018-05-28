using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// MonoBehaviour 대신 MovingObject를 상속
public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;

    private Animator animator; // 애니메이터 컴포넌트의 레퍼런스를 가져오기 위해 사용
    private int food; // 해당 레벨 동안의 플레이어 스코어를 저장

    
	// Use this for initialization
	protected override void Start ()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints; // 푸드 값을 게임매니저에 있는 거로 설정, 레벨이 바뀔 때 게임매니저에 저장

        foodText.text = "Life: " + food; // foodText의 값을 현재 음식 점수로 할당

        base.Start(); // 부모크래스 movingObject의 start 함수
	}

    private void OnDisable() // 게임 오브젝트가 비활성화 되는 순간 호출
    {
        GameManager.instance.playerFoodPoints = food; // 레벨이 변환 될 때 게임매니저에 food 값 저장하는데 사용
    }

    // Update is called once per frame

    

    private void Update ()  // 게임매니저에서 생성한 boolean 변수로 현재 플레이어의 턴을B 체크
    { 
        if (!GameManager.instance.playersTurn) return; // 플레이어의 턴이 아니라면 return을 사용해 이하 코드들이 실행되지 않게 함

        // 수평이나 수직으로 움직이는 방향을 1이나 -1로서 저장해 사용
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0) // 수평으로 움직이는지 체크해서 그렇다면 vertical을 0으로
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0) // horizontal 혹은 vertical이 0이 아닌 값을 가졌는지 체크
        {
            AttemptMove<Wall>(horizontal, vertical); // 이는 플레이어가 상호작용할 수 있는 벽에 대면할지도 모른다는 의미
        }
         
    }

    // AttemptMove = 총 음식 점수 - 1
    
    protected override void AttemptMove<T>(int xDir, int yDir) // T = 움직이는 오브젝트가 마주칠 대상의 컴포넌트의 타입
    {
        food--;

        foodText.text = "Life: " + food;

        base.AttemptMove<T>(xDir, yDir);

        // RaycastHit2D hit; // 라인캐스트 충돌 결과의 레퍼런스 가져올 RayCastHit2D타입의 hit 선언

        CheckIfGameOver();

        GameManager.instance.playersTurn = false; // 플레이어의 턴이 끝났음을 알림
    }

    
    private void OnTriggerEnter2D(Collider2D other) // 충돌할 오브젝트의 태그 체크
    {
        if (other.tag == "Exit")
        {
            SceneManager.LoadScene("MainHall1-1");
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood; // 맞다면 음식점수 추가
            foodText.text = "+" + pointsPerFood + " Life :" + food; // 오브젝트를 먹었을 때 메세지
            other.gameObject.SetActive(false); // 우리가 충돌한 오브젝트 비활성화
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda +" Food :" + food; // 오브젝트를 먹었을 때 메세지
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component) // 이동하려는 공간에 벽이 있고 이에 막히는 경우의 행동
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage); // 플레이어가 벽에 대미지를 줄지 알리기 위해
        animator.SetTrigger("playerChop"); // 발동하고 싶은 트리거의 이름 입력
    }

    private void Restart() // 레벨을 다시 로드, 출구 오브젝트와 충돌했을 때 restart를 호출, 다음 레벨로 넘어간다는 의미
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        // Application.LoadLevel(Application.loadedLevel); // 마지막으로 로드된 신을 로드한다는 의미(main 신)
    }

    public void LoseFood (int loss) // 적이 플레이어를 공격할 때 호출, loss는 플레이어가 얼만큼 점수를 잃을지
    {
        animator.SetTrigger("playerHit");

        food -= loss;

        foodText.text = "-" + loss + " Food: " + food; // 얼마나 점수를 잃었는지 메세지

        CheckIfGameOver();   // 플레이어가 food를 잃었기 때문에 게임이 끝났는지 체크하기 위해 호출
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}
