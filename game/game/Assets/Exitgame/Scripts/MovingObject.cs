using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abstract = 우리가 클래스들과 클래스 멤버들을 만들 때 기능을 완성하지 않아도 되게하고 해당 클래스는 파생클래스로 삽입되야 함
public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f; // 수 초 동안 오브젝트를 움직이게 할 시간 단위
    public LayerMask blockingLayer; // 이동할 공간이 열려있고 그 곳으로 이동하려 할 때 충돌이 일어났는지 체크할 장소

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime; // 움직임을 더 효율적으로 계산하는데 쓰임

	// Use this for initialization
    // 자식 클래스가 덮어써서 재정의 가능(오버라이드)
	protected virtual void Start () {

        boxCollider = GetComponent<BoxCollider2D>();

        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime; // moveTime의 역수를 저장함으로서 효율적인 곱하기 사용 가능
	}

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit) // out 키워드는 입력을 레퍼런스로 받게 함 Move 함수가 두개 이상의 값을 리턴하기 위해 씀
    {
        Vector2 start = transform.position; // 현재 오브젝트의 위치를 저장하기 위해 사용

        Vector2 end = start + new Vector2(xDir, yDir); // 입력받은 방향 값을 기반으로 끝나는 위치 계산에 사용

        boxCollider.enabled = false; // ray를 사용 할 댸 자기 자신의 충돌체에 부딪치지 않게 하기 위해
        hit = Physics2D.Linecast(start, end, blockingLayer); // 시작지점에서 끝지점까지의 라인을 가져와 BlockingLayer와의 충돌 계산
        boxCollider.enabled = true;

        if (hit.transform == null) // 뭔가 부딫쳤는지 체크 null이면 라인을 검사한 공간이 열려있고 그곳으로 이동이 가능
        {
            StartCoroutine(SmoothMovement(end)); // 
            return true;
        }

        return false; // null이 아니면 이동 실패
    }
    // 일반형 입력T는 막혔을 때 유닛이 반응할 컴포넌트 타입 가리키기위해 사용
    protected virtual void AttemptMove<T>(int xDir, int yDir) // 적이면 플레이어 플레이어면 벽
        where T : Component // where이라는 키워드로 T가 컴포넌트 종류를 가리키게 함
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit); // 이동하는데 성공하면 canMove가 true 실패면 false

        if (hit.transform == null) // Move에서 라인 캐스트가 다른 것과 부딪치지 않았다면 리턴하고 이후 코드를 실행하지 않을 것
            return;

        T hitComponent = hit.transform.GetComponent<T>();//만약 부딪쳤다면 충돌한 오브젝트의 컴포넌트의 레퍼런스를 T타입 컴포넌트에 할당

        if (!canMove && hitComponent != null) // 움직이던 오브젝트가 막혔고 상호작용할 수 있는 오브젝트와 충돌을 의미
            OnCantMove(hitComponent);
    }

    // 어디로 이동할건지 표시할 end를 입력으로 받는것
    protected IEnumerator SmoothMovement(Vector3 end)// 코루틴 선언 유닛들을 한 공간에서 다른 곳으로 옮기는데 사용
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;// 이동할 남은 거리 계산 sqrMagintude(벡터 길이 제곱)

        while (sqrRemainingDistance > float.Epsilon) // Epsilon은 0에 가까운 엄청 작은 수
        {
            // moveTime에 기반해서 적절이 end에 가까운 새로운 위치를 찾음
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime); //
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null; // 루프를 갱신하기 전 다음 프레임을 기다림
        }
    }

    // 사용할 것들이 현재 존재하지 않거나, 불완전하게 만들어졌음을 의미(상속한 자식 클래스에서 완성하면 됨) 
    protected abstract void OnCantMove<T>(T component) // 상속한 자식 클래스의 함수로 덮어써서 완성(오버라이드), 추상화 함수라 괄호 사용 안함
        where T : Component; // 일반형(Generic)입력 T를 T형의 component라는 변수로 받아옴 

}
