using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageLayout : MonoBehaviour
{
    public GameObject StageButtonPrefab;

    private void Awake()
    {
        int Check = 0;
        if (CSVData.stageInfo.Count >= CSVData.StageData.Count)
            Check = CSVData.StageData.Count;
        else
            Check = CSVData.stageInfo.Count;

        // 더 적은 크기의 CSV를 참조
        for (int i = 0; i < Check; i++)
        {
            SetStageButton(i);
        }
    }
    public void SetStageButton(int Value)
    {
        //PlayerPrefs.SetInt("Stage", 0);
        // 없을때 0부터 시작
        if (!PlayerPrefs.HasKey("Stage"))
            PlayerPrefs.SetInt("Stage", 0);

        int CurrentStage = PlayerPrefs.GetInt("Stage");

        GameObject Ins = Instantiate(StageButtonPrefab, this.transform);
        Button InsButton = Ins.GetComponent<Button>();

        Ins.transform.GetChild(1).GetComponent<Text>().text = (Value + 1).ToString();

        SetButton(InsButton, Value);

        if (CurrentStage < Value)
            SetInteractable(InsButton, false);
        else
            SetInteractable(InsButton, true);
    }
    public void SetButton(Button button, int Value)
    {
        button.onClick.AddListener(() => GameManager.SetMain(Value));
    }

    public void SetInteractable(Button button, bool Value)
    {
        if (!Value)
        {
            button.interactable = false;
            button.transform.GetChild(0).GetComponent<Image>().color = Color.gray * 0;
        }
        else
        {
            button.interactable = true;
            button.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
        }
    }

}
