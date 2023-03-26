using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public static Button PlayButton { get; private set; }
    public static Button ResetButton { get; private set; }
    public static Button MenuButton { get; private set; }

    private void Awake()
    {
        // 이름으로 각 오브젝트 찾기
        PlayButton = transform.Find("PlayButton").GetComponent<Button>();
        Debug.Assert(PlayButton != null);
        ResetButton = transform.Find("ResetButton").GetComponent<Button>();
        Debug.Assert(ResetButton != null);
        MenuButton = transform.Find("MenuButton").GetComponent<Button>();
        Debug.Assert(MenuButton != null);

        MenuButton.onClick.AddListener(() => GameManager.SetScene("Lobby"));
    }
}
