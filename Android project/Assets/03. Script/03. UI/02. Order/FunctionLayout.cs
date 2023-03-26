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
        FunctionDisabled = false;
        CurrentOrder = 0;
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

        // ������ ��ư ��ú� ��Ŵ
        OrderLayout ord = Order.orderLayout.GetComponent<OrderLayout>();
        ord.DisabledChoice();

        FunctionDisabled = true;
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
        FunctionDisabled = false;
    }
    public void SetCurrentOrder(CommandData Data)
    {
        // ������ ������ �� �ִ� �������� üũ
        if (!FunctionDisabled) { return; }
        if (Data.Id >= 100) { return; }

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
    public IEnumerator SetPlay(float Delay)
    {
        // ���� Ƚ�� �����
        int OrderCount = 0;
        for (int i = 0; i < commandData.Count; i++)
        {
            // ���ų� id 0 �϶� �������
            if (commandData[i] == null || commandData[i].Id == 0)
            {
                continue;
            }

            // ���� �������� ��ư �� �ٲ�
            PlayCurrentChoice(i, Color.yellow);

            if (!PlayerCommand.SetPlayerOrder(commandData[i].Id, Delay / 1.5f))
            {
                Debug.LogWarning(commandData[i].Id + "�� ��� ����(Function)");
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
    }
    public int GetOrderCount() { return orderCount; }
}
