using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
    public static GameObject commandLayout { get; private set; }
    
    private void Awake()
    {
        // 이름으로 각 오브젝트 찾기
        commandLayout = transform.Find("CommandLayout").gameObject;
        Debug.Assert(commandLayout != null);
    }
}
