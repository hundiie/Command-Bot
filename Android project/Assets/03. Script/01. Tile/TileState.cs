using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileState : MonoBehaviour
{
    // ���� ������
    [System.Serializable]
    public struct StaticData
    {
        public int Id;
        public string TileName;
    }

    // ���� ������
    [System.Serializable]
    public struct DynamicData
    {
        public int Tileheight;
    }

    public StaticData staticData;
    public DynamicData dynamicData;
}