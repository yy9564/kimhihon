using System.Collections;
using System.Collections.Generic; // 네임 스페이스 선언 -> 적을 계속 추적하기 위해 사용할 리스트 자료 구조 사용 가능
using UnityEngine;
using UnityEngine.UI; // 네임 스페이스를 더함

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2.0f; // 레벨이 시작되기 전에 초 단위로 대기할 시간
    public float turnDelay = 0.1f; // 턴 사이에 게임이 얼마 동안 대기할지
    // 게임매니저를 싱글턴으로 만듬
    // static은 변수가 인스턴스가 아니라 클래스 자체에 속해 있단 것을 의미
    // 게임매니저의 public함수와 변수들은 어떤 스크립트에서도 접근 가능
    public static GameManager instance = null; // instance를 public으로 선언은 클래스 바깥에서도 접근가능
    public BoardManager boardScript;
    public int playerFoodPoints = 100; // 
    [HideInInspector] public bool playersTurn = true; // Hide = 변수가 public이지만 에디터에서 숨긴다

    private Text levelText; // 현재 레벨 숫자를 표시할 텍스트 (Day1)
    private GameObject levelImage; // 레벨 이미지의 레퍼런스를 저장하여 이미지를 보이거나 숨기려고 할 때 활성화하고 비활성화 할 수 있음
    private int level = 3; // 레벨2부터 적이 등장해서 3으로 테스트
    private List<Enemy> enemies; // 적들의 위치를 계속 추적하고 움직이도록 명령을 전달하기 위해 사용
    private bool enemiesMoving;
    private bool doingSetup; // 게임 보드를 만드는 중인지 체크하고 보드를 만드는 중에는 플레이어가 움직이는것을 방지


	// Use this for initialization
	void Awake () // 보드 매니저 스크립트로 컴포넌트들을 레퍼런스로 들고와 저장
    {
        if (instance == null) // instance가 null이라면 this(자기 자신의 위치 반환)를 할당
            instance = this;
        else if (instance != this) // null도 아니고 this도 아니라면 Destory 함수로 파괴해서 실수로 2개의 게임매니저가 생성되지 않게함
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // 우리가 새로운 신을 로드할때 모든 게임오브젝트가 소멸되는데 신이 넘어가도 점수 계산이 계속되야하니 신이 넘어가면서도 유지하게 함

        enemies = new List<Enemy>();

        boardScript = GetComponent<BoardManager>();

        InitGame();
	}

    private void OnLevelWasLoaded(int index) // OnLevelWasLoaded = 유니티 API 기본 제공 함수, 씬이 로드 될 때 마다 호출, 현재 레벨 숫자를 더하고 새 레벨이 로드됬을 때 initGame 함수를 호출하는데 사용
    {
        level++;

        InitGame(); // UI요소를 관리하고 각 레벨을 구성하는데 사용
    }

    void InitGame() // boardScript의 setupscene 함수를 불러옴
    {
        doingSetup = true; // 타이틀 카드가 뜨는 동안 플레이어는 움직일 수 없음
        
        levelImage = GameObject.Find("LevelImage"); // LevelImage의 레퍼런스를 가져옴, 이름으로 찾아내서 일치하는지 확실이 해야함

        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        // levelText.text = "Day " + level; // 현재 레벨 숫자

        levelImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay);
        
        enemies.Clear(); // 게임이 시작될 때 적 리스트 초기화

        boardScript.SetupScene(level); // 보드스크립트에 지금 구성하는 씬의 레벨을 알려줌
    }

    private void HideLevelImage() // 레벨을 시작할 준비가 됬을 때 레벨 이미지를 끄는 함수 Invoke(지연 시간 후에 함수 호출)
    {
        levelImage.SetActive(false); // 레벨 이미지 비활성화
        doingSetup = false; // 플레이어가 움직일 수 있음
    }
	
    public void GameOver() // 게임 매니저 비활성화
    {
        levelText.text = "GameOver"; // 게임 오버 메세지
        levelImage.SetActive(true); // 검은 배경 활성화
        enabled = false;
    }

	// Update is called once per frame
	void Update () {
        if (playersTurn || enemiesMoving || doingSetup) // playerTurn 혹은 enemiesMoving이 참인지 체크
            return;

        StartCoroutine(MoveEnemies());
	}

    public void AddEnemyToList(Enemy script) // 적들이 자신을 게임 매니저에 등록하도록 해서 게임 매니저가 적들이 움직이돌고 명령할수 있게 함
    {
        enemies.Add(script); // 적 리스트의 Add 함수를 사용해서 입력 함수인 script를 입력해 넣는다
    }

    IEnumerator MoveEnemies() // 연속적으로 한번에 하나씩 적을 옮기는데 사용
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay); // (turnDelay = 0.1초) turnDelay만큼 기다림
        if (enemies.Count == 0) // 적들이 아무도 없는지 체크 = 첫 레벨, 적 리스트의 길이 체크 = enemies.Count
        {
            yield return new WaitForSeconds(turnDelay); // 대기하는 적이 없지만 플레이어가 기다리게 함
        }

        for (int i = 0; i < enemies.Count; i++) // 대기시간이 끝났으면 for루프를 사용해 적 리스트 만큼 루프
        {
            enemies[i].MoveEnemy(); // Enemy 스크립트에 있는 MoveEnemy함수를 호출해 적들이 움직이도록 명령
            yield return new WaitForSeconds(enemies[i].moveTime); // 다음 녀석을 호출하기 전 yiel키워드와 적의 moveTime 변수를 입력해 기다림
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}

// gamemanager는 레벨을 로드와 플레이어 스코어를 관리해서 이 오브젝트가 하나 이상 필요없다