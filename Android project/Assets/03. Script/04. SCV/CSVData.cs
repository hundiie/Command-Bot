using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StageInfo
{
    public int Id;
    public string StageName;
    public List<int> Commands;
    public List<int> Functions;
}

public class CSVData : MonoBehaviour
{
    #region 싱글톤
    public static CSVData instance = null;

    private void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
        }

        ReadStageData();
        ReadStageInfoData();
    }
    #endregion

    public static List<PlayerState> StageData = new List<PlayerState>();
    public static List<StageInfo> stageInfo = new List<StageInfo>();
    public static int GoStage { get; set; }

    private void ReadStageInfoData()
    {
        List<Dictionary<string, object>> ReadData = CSVReader.Read("StageInfo");

        for (int i = 0; i < ReadData.Count; i++)
        {
            StageInfo NewStageInfo = new StageInfo();
            NewStageInfo.Id = (int)ReadData[i]["Id"];
            NewStageInfo.StageName = (string)ReadData[i]["StageName"];

            NewStageInfo.Commands = new List<int>();
            for (int j = 0; j < 8; j++)
            {
                NewStageInfo.Commands.Add((int)ReadData[i][((PlayerOrder)j).ToString()]);
            }

            NewStageInfo.Functions = new List<int>();
            for (int k = 1; k < 4; k++)
            {
                NewStageInfo.Functions.Add((int)ReadData[i]["F" + k]);
            }

            stageInfo.Add(NewStageInfo);
        }
    }
    private void ReadStageData()
    {
        List<Dictionary<string, object>> ReadData = CSVReader.Read("Stage");

        for (int i = 0; i < ReadData.Count; i++)
        {
            PlayerState NewStageData = new PlayerState();
            NewStageData.Direction = (Direction)ReadData[i]["Direction"];
            NewStageData.X = (int)ReadData[i]["X"];
            NewStageData.Y = (int)ReadData[i]["Y"];
            NewStageData.Z = (int)ReadData[i]["Z"];

            StageData.Add(NewStageData);
        }
    }
}
