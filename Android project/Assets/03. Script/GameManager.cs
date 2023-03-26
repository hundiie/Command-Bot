using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static void SetScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public static void SetMain(int Value)
    {
        InitStage(Value);
        SetScene("Main");
    }

    public static void InitStage(int Value)
    {
        CSVData.GoStage = Value;
    }

}
