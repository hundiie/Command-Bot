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

    // 초기화 함수
    private void InitCommandData(int StageNumber)
    {
        // 오브젝트 초기화
        for (int i = 0; i < CommandList.Count; i++)
        {
            Destroy(CommandList[i]);
        }
        CommandList.Clear();

        // id 초기화
        CommandIdList.Clear();
        
        // 다시 생성 // 일단 임시로 전체 생성
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
        // 오브젝트 복제
        GameObject ins = Instantiate(CommandButton, transform);
        // 복제된 오브젝트에 데이터 삽입
        ins.transform.GetChild(0).GetComponent<Image>().sprite = commandData[Value].Sprite;
        ins.transform.GetChild(1).GetComponent<Text>().text = commandData[Value].CommandName;

        // 버튼에 역활 부여
        ins.GetComponent<Button>().onClick.AddListener(() => SetCommandButtonKey(Value));

        // 각 커맨드 데이터 리스트에 저장
        CommandList.Add(ins);
        CommandIdList.Add(commandData[Value].Id);
    }

    private void SetCommandButtonKey(int Value)
    {
        // 메인에 넣기
        OrderLayout ord = Order.orderLayout.GetComponent<OrderLayout>();
        ord.SetCurrentOrder(commandData[Value]);
        
        // 함수에 넣기
        Function fun = Order.function.GetComponent<Function>();
        if (fun.FunctionLayoutList.Count != 0)
        {
            fun.FunctionLayoutList[fun.CurrentFunction].SetCurrentOrder(commandData[Value]);
        }
    }


    public void SetFunctionPrefab(int Value)
    {
        // 오브젝트 복제
        GameObject ins = Instantiate(CommandButton, transform);
        // 복제된 오브젝트에 데이터 삽입
        ins.transform.GetChild(0).GetComponent<Image>().sprite = functionData[Value].Sprite;
        ins.transform.GetChild(1).GetComponent<Text>().text = functionData[Value].CommandName;

        // 버튼에 역활 부여
        ins.GetComponent<Button>().onClick.AddListener(() => Order.orderLayout.GetComponent<OrderLayout>().SetCurrentOrder(functionData[Value]));

        // 각 커맨드 데이터 리스트에 저장
        CommandList.Add(ins);
        CommandIdList.Add(functionData[Value].Id);
        // 새로운 함수 저장장소 생성
        Order.function.GetComponent<Function>().AddFunction(functionData[Value].Sprite);
    }

    public void FunctionInit(int Value)
    {
        Order.function.GetComponent<Function>().SetActiveFunction(Value);
    }
}