using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Function : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject FunctionPrefab;

    public List<GameObject> FunctionObject = new List<GameObject>();
    public List<FunctionLayout> FunctionLayoutList = new List<FunctionLayout>();
    public List<Sprite> FunctionSprites = new List<Sprite>();

    public int CurrentFunction = 0;

    private void Awake()
    {
        InitFunction();
    }
    private void Start()
    {
        Order.functionChangeButton.onClick.AddListener(() => ChangeActiveFunction(CurrentFunction + 1));
    }
    public void InitFunction()
    {
        // 리스트 초기화
        for (int i = 0; i < FunctionObject.Count; i++)
        {
            Destroy(FunctionObject[i]);
        }
        FunctionObject.Clear();
        FunctionLayoutList.Clear();
        FunctionSprites.Clear();

        CurrentFunction = 0;
    }
    public void AddFunction(Sprite FunctionIcon)
    {
        // 함수 추가
        GameObject NewFunction = Instantiate(FunctionPrefab, this.transform);
        FunctionObject.Add(NewFunction);
        // 레이아웃 추가
        FunctionLayout NewFunctionLayout = NewFunction.AddComponent<FunctionLayout>();
        FunctionLayoutList.Add(NewFunctionLayout);

        FunctionSprites.Add(FunctionIcon);

        // 함수 기본값 추가
        NewFunctionLayout.OrderValue = 10;
        NewFunctionLayout.InitFunctionData();

        NewFunction.SetActive(false);

        if (!Order.functionChangeButton.gameObject.activeSelf)
        {
            Order.functionChangeButton.gameObject.SetActive(true);
        }
    }

    public void SetActiveFunction(int FunctionValue)
    {
        if (FunctionObject.Count == 0)
            return;

        FunctionObject[FunctionValue].SetActive(true);
        SetFunctionButtonImage(FunctionValue);
        CurrentFunction = FunctionValue;
    }

    public void ChangeActiveFunction(int Value)
    {
        // 예외처리
        if (Value >= FunctionObject.Count)
            Value %= FunctionObject.Count;
        // 오브젝트 온오프 설정
        FunctionObject[CurrentFunction].SetActive(false);
        FunctionObject[Value].SetActive(true);
        
        SetFunctionButtonImage(Value);
        FunctionLayoutList[Value].SetCurrentChoice(FunctionLayoutList[Value].CurrentOrder, Color.blue);
        CurrentFunction = Value;
    }
    public void PlayChangeActiveFunction(int Value)
    {
        // 예외처리
        if (Value >= FunctionObject.Count)
            Value %= FunctionObject.Count;
        // 오브젝트 온오프 설정
        FunctionObject[CurrentFunction].SetActive(false);
        FunctionObject[Value].SetActive(true);

        SetFunctionButtonImage(Value);
        CurrentFunction = Value;
    }

    private void SetFunctionButtonImage(int Value)
    {
        Order.functionChangeButton.transform.GetChild(0).GetComponent<Image>().sprite = FunctionSprites[Value];
    }
}