using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileState : MonoBehaviour
{
    // 정적 데이터
    [System.Serializable]
    public struct StaticData
    {
        public int Id;
        public string TileName;
    }

    // 동적 데이터
    [System.Serializable]
    public struct DynamicData
    {
        public int Tileheight;
    }

    public StaticData staticData;
    public DynamicData dynamicData;
}