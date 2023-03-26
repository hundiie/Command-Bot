using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CommandLayout : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject CommandButton;

    [Header("Data")]
    public CommandData[] commandData = new CommandData[0];
    public CommandData[] functionData = new CommandData[0];

    private List<GameObject> CommandList = new List<GameObject>();
    private List<int> CommandIdList = new List<int>();
    
    private void Start()
    {
        InitCommandData(CSVData.GoStage);
    }

    // �ʱ�ȭ �Լ�
    private void InitCommandData(int StageNumber)
    {
        // ������Ʈ �ʱ�ȭ
        for (int i = 0; i < CommandList.Count; i++)
        {
            Destroy(CommandList[i]);
        }
        CommandList.Clear();

        // id �ʱ�ȭ
        CommandIdList.Clear();
        
        // �ٽ� ���� // �ϴ� �ӽ÷� ��ü ����
        for (int i = 0; i < commandData.Length; i++)
        {
            if (CSVData.stageInfo[StageNumber].Commands[i] == 1)
            {
                SetCommandPrefab(i);
            }
        }
        int FunCount = 0;
        for (int i = 0; i < functionData.Length; i++)
        {
            if (CSVData.stageInfo[StageNumber].Functions[i] == 1)
            {
                SetFunctionPrefab(i);
                FunCount++;
            }
        }
        if (FunCount != 0)
        {
            FunctionInit(0);
        }
    }

    public void SetCommandPrefab(int Value)
    {
        // ������Ʈ ����
        GameObject ins = Instantiate(CommandButton, transform);
        // ������ ������Ʈ�� ������ ����
        ins.transform.GetChild(0).GetComponent<Image>().sprite = commandData[Value].Sprite;
        ins.transform.GetChild(1).GetComponent<Text>().text = commandData[Value].CommandName;

        // ��ư�� ��Ȱ �ο�
        ins.GetComponent<Button>().onClick.AddListener(() => SetCommandButtonKey(Value));

        // �� Ŀ�ǵ� ������ ����Ʈ�� ����
        CommandList.Add(ins);
        CommandIdList.Add(commandData[Value].Id);
    }

    private void SetCommandButtonKey(int Value)
    {
        // ���ο� �ֱ�
        OrderLayout ord = Order.orderLayout.GetComponent<OrderLayout>();
        ord.SetCurrentOrder(commandData[Value]);
        
        // �Լ��� �ֱ�
        Function fun = Order.function.GetComponent<Function>();
        if (fun.FunctionLayoutList.Count != 0)
        {
            fun.FunctionLayoutList[fun.CurrentFunction].SetCurrentOrder(commandData[Value]);
        }
    }


    public void SetFunctionPrefab(int Value)
    {
        // ������Ʈ ����
        GameObject ins = Instantiate(CommandButton, transform);
        // ������ ������Ʈ�� ������ ����
        ins.transform.GetChild(0).GetComponent<Image>().sprite = functionData[Value].Sprite;
        ins.transform.GetChild(1).GetComponent<Text>().text = functionData[Value].CommandName;

        // ��ư�� ��Ȱ �ο�
        ins.GetComponent<Button>().onClick.AddListener(() => Order.orderLayout.GetComponent<OrderLayout>().SetCurrentOrder(functionData[Value]));

        // �� Ŀ�ǵ� ������ ����Ʈ�� ����
        CommandList.Add(ins);
        CommandIdList.Add(functionData[Value].Id);
        // ���ο� �Լ� ������� ����
        Order.function.GetComponent<Function>().AddFunction(functionData[Value].Sprite);
    }

    public void FunctionInit(int Value)
    {
        Order.function.GetComponent<Function>().SetActiveFunction(Value);
    }
}