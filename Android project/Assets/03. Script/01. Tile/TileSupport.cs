using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSupport : MonoBehaviour
{
    public static LayerMask MapMask;

    public static GameObject TileDirectionCheck(Direction direction, GameObject Object,int Distance)
    {
        Vector3 Current = new Vector3(0,0,0);
        // 레이저 방향
        Ray ray;
        // 방향에 따라 설정
        switch (direction)
        {
            case Direction.East:
                Current.z += 1;
                break; 
            case Direction.West:
                Current.z -= 1;
                break;
            case Direction.South:
                Current.x += 1;
                break;
            case Direction.North:
                Current.x -= 1;
                break;
            default:
                break;
        }
        int NewDistance = Distance;
        
        // 거리가 음수(역방향) 일 때
        if (Distance < 0)
            NewDistance *= -1;

        RaycastHit Hit;

        ray = new Ray(Object.transform.position, Current);
        // 레이 마스크 체크 맵에 걸리면 들어감
        if (Physics.Raycast(ray, out Hit, NewDistance, MapMask))
        {
            return Hit.collider.gameObject;
        }
        return null;
    }
    public static GameObject TileUpCheck(GameObject Object, int Distance)
    {
        // 레이저 방향
        Ray ray;
        int NewDistance = Distance;

        // 거리가 음수(역방향) 일 때
        if (Distance < 0)
        {
            ray = new Ray(Object.transform.position, -Object.transform.up);
            NewDistance *= -1;
        }
        else
        {
            ray = new Ray(Object.transform.position, Object.transform.up);
        }
        RaycastHit Hit;

        // 레이 마스크 체크 맵에 걸리면 들어감
        if (Physics.Raycast(ray, out Hit, NewDistance, MapMask))
        {
            return Hit.collider.gameObject;
        }
        return null;
    }
    // 오브젝트 투명화 결정
    public static bool SetActiveObject(GameObject Object, bool Value)
    {
        if (Object == null)
            return false;
        // 자식이 없으면 false
        GameObject child = Object.transform.GetChild(0).gameObject;
        if (child == null)
            return false;

        child.SetActive(Value);
        return true;
    }
    public static void TileMoveEvent(GameObject Tile)
    {
        if (Tile == null)
            return;

        TileState tile = Tile.GetComponent<TileState>();
        if (tile == null)
            return;

        switch (tile.staticData.Id)
        {
            case 4:
                {
                    Debug.Log("열쇠 획득");
                    // 열쇠 지우기
                    SetActiveObject(Tile, false);
                    // 문 열기
                    Tile.transform.GetChild(0).GetComponent<KeyLink>().Door.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
    public static void TileEndEvent(GameObject Tile)
    {
        if (Tile == null)
            return;

        TileState tile = Tile.GetComponent<TileState>();
        if (tile == null)
            return;

        switch (tile.staticData.Id)
        {
            case 0:
                {
                    Debug.Log("왕관 획득");
                    SetActiveObject(Tile, false);
                }
                break;
            default:
                break;
        }
    }
}
