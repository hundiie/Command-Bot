using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static GameObject thisObject;

    [SerializeField]
    private List<GameObject> StageList = new List<GameObject>();

    private static List<GameObject> Stage = new List<GameObject>();

    public static GameObject CurrentStage;
    private void Awake()
    {
        thisObject = this.gameObject;
        Stage = StageList;
    }
    public static void InitStage(int StageNumber)
    {
        if (StageNumber < 0)
            return;
        if (CurrentStage != null)
            Destroy(CurrentStage);

        GameObject CopyStage = Instantiate(Stage[StageNumber], thisObject.transform);
        CurrentStage = CopyStage;
    }
}
