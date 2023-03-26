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
    // �ʱ�ȭ �Լ�
    public void InitOrderData()
    {
        // ���� �ʱ�ȭ
        for (int i = 0; i < OrderList.Count; i++)
        {
            Destroy(OrderList[i]);
        }
        OrderList.Clear();
        commandData.Clear();
        
        // ���� �����
        for (int i = 0; i < OrderValue; i++)
        {
            //��ư ����
            GameObject Ins = Instantiate(Order.OrderButtonPrefab, transform);
            
            // ��ư ����
            int num = i;
            Ins.GetComponent<Button>().onClick.AddListener(() => SetCurrentChoice(num, Color.blue));
            
            // ������ ��ư ����
            OrderList.Add(Ins);
            commandData.Add(null);
        }
        // 0��° ��ư���� ����
        OrderDisabled = true;
        SetCurrentChoice(0, Color.blue);
    }
    public void SetCurrentChoice(int Value, Color color)
    {
        // ����ó��
        if (Value < 0) { Value = OrderList.Count - 1; }
        Value %= OrderList.Count;
        
        // ���� �� �ǵ�����
        OrderList[CurrentOrder].GetComponent<Image>().color = Color.black * (100f / 255f);
        
        // ���� �� �ٲٱ�
        CurrentOrder = Value;
        OrderList[Value].GetComponent<Image>().color = color * (100f / 255f);
        
        // �Լ��� ��ư ��ú� ��Ŵ
        Function fun = Order.function.GetComponent<Function>();
        
        for (int i = 0; i < fun.FunctionLayoutList.Count; i++)
        {
            fun.FunctionLayoutList[i].DisabledChoice();
        }
        
        OrderDisabled = true;
    }
    public void PlayCurrentChoice(int Value, Color color)
    {
        // ����ó��
        if (Value < 0) { Value = OrderList.Count - 1; }
        Value %= OrderList.Count;

        // ���� �� �ǵ�����
        OrderList[CurrentOrder].GetComponent<Image>().color = Color.black * (100f / 255f);

        // ���� �� �ٲٱ�
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
        // ������ ������ �� �ִ� �������� üũ
        if (!OrderDisabled) { return; }

        Image CurrentOrderImage = OrderList[CurrentOrder].transform.GetChild(0).GetComponent<Image>();
        
        // ������ ����
        commandData[CurrentOrder] = Data;

        // ID üũ�ؼ� ��������Ʈ ����
        if (Data.Id != 0)
        {
            CurrentOrderImage.sprite = Data.Sprite;
            CurrentOrderImage.color = Color.white * 255;
            // ���� ������ �������� �̵�
            SetCurrentChoice(CurrentOrder + 1, Color.blue);
        }
        else
        {
            CurrentOrderImage.sprite = null;
            CurrentOrderImage.color = Color.white * 0;
        }
    }

    //------------------- �Ƹ� �ӽ� �ڵ�
    private void SetPlay(List<CommandData> commandDatas, float Delay)
    {
        StartCoroutine(_SetPlay(commandDatas, Delay));
    }

    private IEnumerator _SetPlay(List<CommandData> commandDatas, float Delay)
    {
        // ������ �÷��� ��ư ��Ȱ��ȭ
        StageButton.PlayButton.interactable = false;
        StageButton.ResetButton.interactable = false;

        // ���� ������ ��ư ����
        int CurrentChoice = CurrentOrder;
        // ���� Ƚ�� �����
        int OrderCount = 0;
        for (int i = 0; i < commandDatas.Count; i++)
        {
            // ���ų� id 0 �϶� �������
            if (commandDatas[i] == null || commandDatas[i].Id == 0)
            {
                continue;
            }
            // ���� �������� ��ư �� �ٲ�
            PlayCurrentChoice(i, Color.yellow);

            // �� id�� ���� ����
            if (commandDatas[i].Id >= 100)
            {
                int FunctionValue = commandDatas[i].Id - 100;
                
                Function fun = Order.function.GetComponent<Function>();
                fun.PlayChangeActiveFunction(FunctionValue);
                // �Լ� ����
                yield return StartCoroutine(fun.FunctionLayoutList[FunctionValue].SetPlay(Delay));
                // ���� ī���� �߰�
                OrderCount += fun.FunctionLayoutList[FunctionValue].GetOrderCount();
                continue;
            }
            else if (!PlayerCommand.SetPlayerOrder(commandDatas[i].Id, Delay / 1.5f))
            {
                Debug.LogWarning(commandDatas[i].Id + "�� ��� ����(Main)");
            }

            // ������ �� ���� ī��Ʈ +1
            OrderCount += 1;
            yield return new WaitForSeconds(Delay);

            // �� ĭ ���������� üũ��
            int FallCount = 0;
            // �ִ� ���� ���� ����
            int MaxFall = 5;
            // �󸶳� ���߿� �ִ��� üũ
            while (PlayerSupport.PlayerUpCheck(-(FallCount + 1)) == null)
            {
                FallCount += 1;
                if (FallCount >= MaxFall)
                    break;
                yield return null;
            }
            // ���߿� ������ ������
            if (FallCount != 0)
            {
                Debug.Log(FallCount + "ĭ ������");
                PlayerCommand.MoveUp(-FallCount, Delay / 1.5f);
                yield return new WaitForSeconds(Delay);

                if (FallCount >= 2)
                {
                    Debug.Log("����");
                    break;
                }
            }

            // �ٴڿ� �ִ� Ÿ���ڵ� üũ�ؼ� �̺�Ʈ �߻� (�����̴� ��)
            GameObject tileObject_Move = PlayerSupport.PlayerUpCheck(-1);
            if (tileObject_Move != null)
            {
                TileSupport.TileMoveEvent(tileObject_Move);
            }

        }

        // ���� ������ �ٽ� ������ ������ �̵�
        Debug.Log(OrderCount + "ȸ ����");
        SetCurrentChoice(CurrentChoice, Color.blue);
        StageButton.PlayButton.interactable = true;
        StageButton.ResetButton.interactable = true;
        
        // �ٴڿ� �ִ� Ÿ���ڵ� üũ�ؼ� �̺�Ʈ �߻� (���� ����)
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