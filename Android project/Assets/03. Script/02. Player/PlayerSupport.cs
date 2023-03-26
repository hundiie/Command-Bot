using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSupport : MonoBehaviour
{
    public static LayerMask MapMask;

    public static GameObject PlayerForwardCheck(int Distance)
    {
        GameObject playerObject = PlayerCommand.PlayerObject;
        
        // ������ ����
        Ray ray;
        int NewDistance = Distance;
        // �Ÿ��� ����(������) �� ��
        if (Distance < 0)
        {
            ray = new Ray(playerObject.transform.position, -playerObject.transform.forward);
            NewDistance *= -1;
        }
        else
        {
            ray = new Ray(playerObject.transform.position, playerObject.transform.forward);
        }

        RaycastHit Hit;

        // ���� ����ũ üũ �ʿ� �ɸ��� ��
        if (Physics.Raycast(ray, out Hit, NewDistance, MapMask))
        {
            return Hit.collider.gameObject;
        }
        return null;
    }
    public static GameObject PlayerJumpCheck()
    {
        GameObject playerObject = PlayerCommand.PlayerObject;

        Vector3 PlayerPos = playerObject.transform.position;
        PlayerPos.y += 1;
        // ������ ����
        Ray ray;
        
        ray = new Ray(PlayerPos, playerObject.transform.forward);
        // ����ĳ��Ʈ ��Ʈ
        RaycastHit Hit;

        // ���� ����ũ üũ �ʿ� �ɸ��� ��
        if (Physics.Raycast(ray, out Hit, 1, MapMask))
        {
            return Hit.collider.gameObject;
        }
        return null;
    }

    public static GameObject PlayerUpCheck(int Distance)
    {
        GameObject playerObject = PlayerCommand.PlayerObject;
        
        // ������ ���� 
        int NewDistance = Distance;

        Ray ray;
        if (Distance < 0)
        {
            ray = new Ray(playerObject.transform.position, -playerObject.transform.up);
            NewDistance *= -1;
        }
        else
        {
            ray = new Ray(playerObject.transform.position, playerObject.transform.up);
        }

        // ����ĳ��Ʈ ��Ʈ
        RaycastHit Hit;

        // ���� ����ũ üũ �ʿ� �ɸ��� ��
        if (Physics.Raycast(ray, out Hit, NewDistance, MapMask))
        {
            return Hit.collider.gameObject;
        }
        return null;
    }
}
