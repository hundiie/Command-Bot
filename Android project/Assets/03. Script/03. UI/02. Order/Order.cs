using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject orderButtonPrefab;

    public static GameObject OrderButtonPrefab { get; private set; }
    
    public static Button functionChangeButton { get; private set; }
    public static GameObject orderLayout { get; private set; }
    public static GameObject function { get; private set; }

    private void Awake()
    {
        OrderButtonPrefab = orderButtonPrefab;

        // 이름으로 각 오브젝트 찾기
        orderLayout = transform.Find("OrderLayout").gameObject;
        Debug.Assert(orderLayout != null);
        function = transform.Find("Function").gameObject;
        Debug.Assert(orderLayout != null);

        functionChangeButton = transform.Find("FunctionChangeButton").GetComponent<Button>();
        Debug.Assert(functionChangeButton != null);
        functionChangeButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        StageButton.ResetButton.onClick.AddListener(() => InitOrder());
    }

    private void InitOrder()
    {
        Function fun = function.GetComponent<Function>();
        for (int i = 0; i < fun.FunctionLayoutList.Count; i++)
        {
            fun.FunctionLayoutList[i].InitFunctionData();
        }

        if (fun.FunctionLayoutList.Count != 0)
        {
            fun.ChangeActiveFunction(0);
        }
        orderLayout.GetComponent<OrderLayout>().InitOrderData();
    }
}
