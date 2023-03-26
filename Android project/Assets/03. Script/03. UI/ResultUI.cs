using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public static GameObject TrueUI;
    public static Text PlayText;
    public static Text BestText;

    public static Button True_MenuButton;
    public static Button True_NextButton;
    public static Button True_RetryButton;

    public static GameObject FalseUI;
    public static Button False_MenuButton;
    public static Button False_RetryButton;


    private void Awake()
    {
        TrueUI = transform.Find("True").gameObject;
        Debug.Assert(TrueUI != null);
        PlayText = TrueUI.transform.Find("PlayText").GetComponent<Text>();
        Debug.Assert(PlayText != null);
        BestText = TrueUI.transform.Find("BestText").GetComponent<Text>();
        Debug.Assert(BestText != null);
        True_MenuButton = TrueUI.transform.Find("MenuButton").GetComponent<Button>();
        Debug.Assert(True_MenuButton != null);
        True_NextButton = TrueUI.transform.Find("NextButton").GetComponent<Button>();
        Debug.Assert(True_NextButton != null);
        True_RetryButton = TrueUI.transform.Find("RetryButton").GetComponent<Button>();
        Debug.Assert(True_RetryButton != null);

        FalseUI = transform.Find("False").gameObject;
        Debug.Assert(FalseUI != null);
        False_MenuButton = FalseUI.transform.Find("MenuButton").GetComponent<Button>();
        Debug.Assert(False_MenuButton != null);
        False_RetryButton = FalseUI.transform.Find("RetryButton").GetComponent<Button>();
        Debug.Assert(False_RetryButton != null);

        True_MenuButton.onClick.AddListener(() => GameManager.SetScene("Lobby"));
        True_NextButton.onClick.AddListener(() => GameManager.SetMain(CSVData.GoStage + 1));
        True_RetryButton.onClick.AddListener(() => SetRetry());

        False_MenuButton.onClick.AddListener(() => GameManager.SetScene("Lobby"));
        False_RetryButton.onClick.AddListener(() => SetRetry());

        ActiveFalseResult();

    }
    private void SetRetry()
    {
        PlayerCommand.InitPlayer();
        StageManager.InitStage(CSVData.GoStage);
        ActiveFalseResult();
    }

    public static void ActiveFalseResult()
    {
        TrueUI.SetActive(false);
        FalseUI.SetActive(false);
    }
    public static void SetResult(bool result, int Stage, int MoveCount)
    {
        if (result)
        {
            if (PlayerPrefs.GetInt("Stage") < Stage + 1)
                PlayerPrefs.SetInt("Stage", Stage + 1);
            
            PlayText.text = MoveCount + "ȸ �̵����� Ŭ����";

            TrueUI.SetActive(true);
            // ����Ʈ ��� üũ
            if (!PlayerPrefs.HasKey("S" + Stage))
            {
                // ������ ���� ��� ����Ʈ
                PlayerPrefs.SetInt("S" + Stage, MoveCount);
                BestText.text = "BEST : " + PlayerPrefs.GetInt("S" + Stage);
            }
            else
            {
                // ������ üũ�ؼ� ����Ʈ ����
                if (PlayerPrefs.GetInt("S" + Stage) > MoveCount)
                {
                    PlayerPrefs.SetInt("S" + Stage, MoveCount);
                    BestText.text = "BEST : " + PlayerPrefs.GetInt("S" + Stage);
                }
                else { BestText.text = "BEST : " + PlayerPrefs.GetInt("S" + Stage); }
            }
            // ������ ������������ üũ�ϰ� NEXT��ư ����
            if (CSVData.stageInfo.Count <= Stage + 1)
                True_NextButton.gameObject.SetActive(false);
            else
                True_NextButton.gameObject.SetActive(true);
        }
        else
        {
            FalseUI.SetActive(true);
        }
    }
}
