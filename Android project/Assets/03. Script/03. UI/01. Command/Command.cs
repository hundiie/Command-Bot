using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
    public static GameObject commandLayout { get; private set; }
    
    private void Awake()
    {
        // �̸����� �� ������Ʈ ã��
        commandLayout = transform.Find("CommandLayout").gameObject;
        Debug.Assert(commandLayout != null);
    }
}
