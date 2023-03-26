using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSupport : MonoBehaviour
{
    public static LayerMask MapMask;

    public static GameObject TileDirectionCheck(Direction direction, GameObject Object,int Distance)
    {
        Vector3 Current = new Vector3(0,0,0);
        // ������ ����
        Ray ray;
        // ���⿡ ���� ����
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
        
        // �Ÿ��� ����(������) �� ��
        if (Distance < 0)
            NewDistance *= -1;

        RaycastHit Hit;

        ray = new Ray(Object.transform.position, Current);
        // ���� ����ũ üũ �ʿ� �ɸ��� ��
        if (Physics.Raycast(ray, out Hit, NewDistance, MapMask))
        {
            return Hit.collider.gameObject;
        }
        return null;
    }
    public static GameObject TileUpCheck(GameObject Object, int Distance)
    {
        // ������ ����
        Ray ray;
        int NewDistance = Distance;

        // �Ÿ��� ����(������) �� ��
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

        // ���� ����ũ üũ �ʿ� �ɸ��� ��
        if (Physics.Raycast(ray, out Hit, NewDistance, MapMask))
        {
            return Hit.collider.gameObject;
        }
        return null;
    }
    // ������Ʈ ����ȭ ����
    public static bool SetActiveObject(GameObject Object, bool Value)
    {
        if (Object == null)
            return false;
        // �ڽ��� ������ false
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
                    Debug.Log("���� ȹ��");
                    // ���� �����
                    SetActiveObject(Tile, false);
                    // �� ����
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
                    Debug.Log("�հ� ȹ��");
                    SetActiveObject(Tile, false);
                }
                break;
            default:
                break;
        }
    }
}
