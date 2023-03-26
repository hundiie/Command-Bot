using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    public static Button StartButton;
    public static Button OptionButton;
    public static Button ExitButton;

    private void Awake()
    {
        StartButton = transform.Find("StartButton").GetComponent<Button>();
        Debug.Assert(StartButton != null);
        OptionButton = transform.Find("OptionButton").GetComponent<Button>();
        Debug.Assert(OptionButton != null);
        ExitButton = transform.Find("ExitButton").GetComponent<Button>();
        Debug.Assert(ExitButton != null);
    }
    private void Start()
    {
        StartButton.onClick.AddListener(() => GameManager.SetScene("Lobby"));
    }
}
