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
    #region �̱���
    public static CSVData instance = null;

    private void Awake()
    {
        if (instance == null) //instance�� null. ��, �ý��ۻ� �����ϰ� ���� ������
        {
            instance = this; //���ڽ��� instance�� �־��ݴϴ�.
            DontDestroyOnLoad(gameObject); //OnLoad(���� �ε� �Ǿ�����) �ڽ��� �ı����� �ʰ� ����
        }
        else
        {
            if (instance != this) //instance�� ���� �ƴ϶�� �̹� instance�� �ϳ� �����ϰ� �ִٴ� �ǹ�
                Destroy(this.gameObject); //�� �̻� �����ϸ� �ȵǴ� ��ü�̴� ��� AWake�� �ڽ��� ����
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
