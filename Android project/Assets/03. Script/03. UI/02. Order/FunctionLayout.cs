using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionLayout : MonoBehaviour
{
    [Header("Value")]
    public int OrderValue;

    private List<GameObject> OrderList = new List<GameObject>();

    private List<CommandData> commandData = new List<CommandData>();
    public int CurrentOrder { get; private set; } = 0;

    private bool FunctionDisabled = false;

    public int orderCount { get; private set; }

    public void InitFunctionData()
    {
        // 전부 초기화
        for (int i = 0; i < OrderList.Count; i++)
        {
            Destroy(OrderList[i]);
        }
        OrderList.Clear();
        commandData.Clear();

        // 새로 만들기
        for (int i = 0; i < OrderValue; i++)
        {
            //버튼 복사
            GameObject Ins = Instantiate(Order.OrderButtonPrefab, transform);

            // 버튼 세팅
            int num = i;
            Ins.GetComponent<Button>().onClick.AddListener(() => SetCurrentChoice(num, Color.blue));

            // 복사한 버튼 저장
            OrderList.Add(Ins);
            commandData.Add(null);
        }
        FunctionDisabled = false;
        CurrentOrder = 0;
    }
    public void SetCurrentChoice(int Value, Color color)
    {
        // 예외처리
        if (Value < 0) { Value = OrderList.Count - 1; }
        Value %= OrderList.Count;

        // 이전 색 되돌리기
        OrderList[CurrentOrder].GetComponent<Image>().color = Color.black * (100f / 255f);

        // 선택 색 바꾸기
        CurrentOrder = Value;
        OrderList[Value].GetComponent<Image>().color = color * (100f / 255f);

        // 메인쪽 버튼 디시블 시킴
        OrderLayout ord = Order.orderLayout.GetComponent<OrderLayout>();
        ord.DisabledChoice();

        FunctionDisabled = true;
    }
    public void PlayCurrentChoice(int Value, Color color)
    {
        // 예외처리
        if (Value < 0) { Value = OrderList.Count - 1; }
        Value %= OrderList.Count;

        // 이전 색 되돌리기
        OrderList[CurrentOrder].GetComponent<Image>().color = Color.black * (100f / 255f);

        // 선택 색 바꾸기
        CurrentOrder = Value;
        OrderList[Value].GetComponent<Image>().color = color * (100f / 255f);
    }
    public void DisabledChoice()
    {
        OrderList[CurrentOrder].GetComponent<Image>().color = Color.black * (100f / 255f);
        FunctionDisabled = false;
    }
    public void SetCurrentOrder(CommandData Data)
    {
        // 오더를 조작할 수 있는 상태인지 체크
        if (!FunctionDisabled) { return; }
        if (Data.Id >= 100) { return; }

        Image CurrentOrderImage = OrderList[CurrentOrder].transform.GetChild(0).GetComponent<Image>();

        // 데이터 저장
        commandData[CurrentOrder] = Data;

        // ID 체크해서 스프라이트 수정
        if (Data.Id != 0)
        {
            CurrentOrderImage.sprite = Data.Sprite;
            CurrentOrderImage.color = Color.white * 255;
            // 현재 선택을 다음으로 이동
            SetCurrentChoice(CurrentOrder + 1, Color.blue);
        }
        else
        {
            CurrentOrderImage.sprite = null;
            CurrentOrderImage.color = Color.white * 0;
        }
    }
    public IEnumerator SetPlay(float Delay)
    {
        // 오더 횟수 저장용
        int OrderCount = 0;
        for (int i = 0; i < commandData.Count; i++)
        {
            // 없거나 id 0 일때 실행없음
            if (commandData[i] == null || commandData[i].Id == 0)
            {
                continue;
            }

            // 현재 실행중인 버튼 색 바꿈
            PlayCurrentChoice(i, Color.yellow);

            if (!PlayerCommand.SetPlayerOrder(commandData[i].Id, Delay / 1.5f))
            {
                Debug.LogWarning(commandData[i].Id + "번 명령 실패(Function)");
            }

            // 딜레이 전 오더 카운트 +1
            OrderCount += 1;
            yield return new WaitForSeconds(Delay);


            // 몇 칸 떨어지는지 체크용
            int FallCount = 0;
            // 최대 낙하 높이 설정
            int MaxFall = 5;
            // 얼마나 공중에 있는지 체크
            while (PlayerSupport.PlayerUpCheck(-(FallCount + 1)) == null)
            {
                FallCount += 1;
                if (FallCount >= MaxFall)
                    break;
                yield return null;
            }
            // 공중에 있을시 떨어짐
            if (FallCount != 0)
            {
                Debug.Log(FallCount + "칸 떨어짐");
                PlayerCommand.MoveUp(-FallCount, Delay / 1.5f);
                yield return new WaitForSeconds(Delay);

                if (FallCount >= 2)
                {
                    Debug.Log("낙사");
                    break;
                }
            }

            // 바닥에 있는 타일코드 체크해서 이벤트 발생 (움직이는 중)
            GameObject tileObject_Move = PlayerSupport.PlayerUpCheck(-1);
            if (tileObject_Move != null)
            {
                TileSupport.TileMoveEvent(tileObject_Move);
            }
        }
    }
    public int GetOrderCount() { return orderCount; }
}
