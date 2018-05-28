using System.Collections;
using System.Collections.Generic; // list 사용가능
using System; // namespace선언 직렬화 속성 사용 가능(어떻게 변수들이 에디터에 있는 인스펙터에 나타날지 수정 및 foldout을 통해 보이거나 숨김
using Random = UnityEngine.Random; // System과 unity Engine의 네임 스페이스 모두 랜덤이 존재
using UnityEngine;

public class BoardManager : MonoBehaviour {
    [Serializable] // Count 직렬화 선언
    public class Count
    {
        public int minimum; // 변수 선언
        public int maximum;

        public Count (int min, int max) // 값 할당
        {
            minimum = min; // 파라미터
            maximum = max;
        }
    }

    public int columns = 8; // 게임보드 크기 열
    public int rows = 8; // 게임보드 크기 행
    public Count wallCount = new Count(5, 9); //레벨마다 얼마나 많은 벽을 랜덤하게 생성할지 범위 특정 최소5개 최대 9개 벽
    public Count foodCount = new Count(1, 5); // 음식 아이템 개수 적용
    public GameObject exit;
    public GameObject[] floorTiles; // 소환하고 싶은 녀석을 바리에이션중 선택
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles; // 적
    public GameObject[] outerWallTiles;

    private Transform boardHolder; // Hierarchy를 깔끔하게 하기위한
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList() // 리스트초기와
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup () // 바깥벽과 게임보드의 바닥 생성 
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)]; // floor타일 배열로부터 랜덤하게 집어넣음
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition; // 랜덤공간에 오브젝트 생성
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) // 선택한 장소에 실제로 소환하는 함수
    {
        int objectCount = Random.Range(minimum, maximum + 1); // 우리가 주어진 오브젝트를 얼마나 스폰할지 조정

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup(); 
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f); // 랜덤한 수의 적을 생성 레벨 수에 따라, mathf는 float 형 리턴 정수형으로 변환
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity); //출구위치 고정
    }

}
