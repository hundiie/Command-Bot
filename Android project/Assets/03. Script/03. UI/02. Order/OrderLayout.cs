using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderLayout : MonoBehaviour
{
    [Header("Value")]
    public int OrderValue;

    private List<GameObject> OrderList = new List<GameObject>();

    private List<CommandData> commandData = new List<CommandData>();
    public int CurrentOrder { get; private set; } = 0;
    
    private bool OrderDisabled = true;

    private void Start()
    {
        InitOrderData();
        StageButton.PlayButton.onClick.AddListener(() => SetPlay(commandData, 0.5f));
    }
    // 초기화 함수
    public void InitOrderData()
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
        // 0번째 버튼부터 시작
        OrderDisabled = true;
        SetCurrentChoice(0, Color.blue);
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
        
        // 함수쪽 버튼 디시블 시킴
        Function fun = Order.function.GetComponent<Function>();
        
        for (int i = 0; i < fun.FunctionLayoutList.Count; i++)
        {
            fun.FunctionLayoutList[i].DisabledChoice();
        }
        
        OrderDisabled = true;
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
        OrderDisabled = false;
        
    }
    public void SetCurrentOrder(CommandData Data)
    {
        // 오더를 조작할 수 있는 상태인지 체크
        if (!OrderDisabled) { return; }

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

    //------------------- 아마 임시 코드
    private void SetPlay(List<CommandData> commandDatas, float Delay)
    {
        StartCoroutine(_SetPlay(commandDatas, Delay));
    }

    private IEnumerator _SetPlay(List<CommandData> commandDatas, float Delay)
    {
        // 실행중 플레이 버튼 비활성화
        StageButton.PlayButton.interactable = false;
        StageButton.ResetButton.interactable = false;

        // 현재 눌러진 버튼 저장
        int CurrentChoice = CurrentOrder;
        // 오더 횟수 저장용
        int OrderCount = 0;
        for (int i = 0; i < commandDatas.Count; i++)
        {
            // 없거나 id 0 일때 실행없음
            if (commandDatas[i] == null || commandDatas[i].Id == 0)
            {
                continue;
            }
            // 현재 실행중인 버튼 색 바꿈
            PlayCurrentChoice(i, Color.yellow);

            // 각 id에 따라 실행
            if (commandDatas[i].Id >= 100)
            {
                int FunctionValue = commandDatas[i].Id - 100;
                
                Function fun = Order.function.GetComponent<Function>();
                fun.PlayChangeActiveFunction(FunctionValue);
                // 함수 실행
                yield return StartCoroutine(fun.FunctionLayoutList[FunctionValue].SetPlay(Delay));
                // 오더 카운터 추가
                OrderCount += fun.FunctionLayoutList[FunctionValue].GetOrderCount();
                continue;
            }
            else if (!PlayerCommand.SetPlayerOrder(commandDatas[i].Id, Delay / 1.5f))
            {
                Debug.LogWarning(commandDatas[i].Id + "번 명령 실패(Main)");
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

        // 실행 끝나고 다시 선택한 곳으로 이동
        Debug.Log(OrderCount + "회 실행");
        SetCurrentChoice(CurrentChoice, Color.blue);
        StageButton.PlayButton.interactable = true;
        StageButton.ResetButton.interactable = true;
        
        // 바닥에 있는 타일코드 체크해서 이벤트 발생 (오더 끝남)
        GameObject tileObject_End = PlayerSupport.PlayerUpCheck(-1);
        TileState tile_End;
        bool Result = false;
        if (tileObject_End != null)
        {
            tile_End = tileObject_End.GetComponent<TileState>();
            switch (tile_End.staticData.Id)
            {
                case 0:
                    {
                        TileSupport.SetActiveObject(tileObject_End, false);
                        Result = true;
                    }
                    break;
                default:
                    break;
            }
        }

        ResultUI.SetResult(Result, CSVData.GoStage, OrderCount);
    }
}