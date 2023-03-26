using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSupport : MonoBehaviour
{
    public static LayerMask MapMask;

    public static GameObject PlayerForwardCheck(int Distance)
    {
        GameObject playerObject = PlayerCommand.PlayerObject;
        
        // 레이저 방향
        Ray ray;
        int NewDistance = Distance;
        // 거리가 음수(역방향) 일 때
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

        // 레이 마스크 체크 맵에 걸리면 들어감
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
        // 레이저 방향
        Ray ray;
        
        ray = new Ray(PlayerPos, playerObject.transform.forward);
        // 레이캐스트 히트
        RaycastHit Hit;

        // 레이 마스크 체크 맵에 걸리면 들어감
        if (Physics.Raycast(ray, out Hit, 1, MapMask))
        {
            return Hit.collider.gameObject;
        }
        return null;
    }

    public static GameObject PlayerUpCheck(int Distance)
    {
        GameObject playerObject = PlayerCommand.PlayerObject;
        
        // 레이저 방향 
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

        // 레이캐스트 히트
        RaycastHit Hit;

        // 레이 마스크 체크 맵에 걸리면 들어감
        if (Physics.Raycast(ray, out Hit, NewDistance, MapMask))
        {
            return Hit.collider.gameObject;
        }
        return null;
    }
}
