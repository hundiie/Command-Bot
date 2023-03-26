using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 방위
public enum Direction
{
    East,   // 동 = 0
    South,  // 남 = 90
    West,   // 서 = 180
    North,   // 북 = 270
}
// 방향
public enum Attendants
{
    Right,
    Left,
}

public enum PlayerOrder
{
    Cancel = 0,
    Move_Front = 1,
    Move_Back = 2,
    Turn_Right = 3,
    Turn_Left = 4,
    Jump = 5,
    Dash = 6,
    Attack = 7,
}
public struct PlayerState
{
    public Direction Direction;
    public int X;
    public int Y;
    public int Z;
}
public class PlayerCommand : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject PlayerPrefab;
    private static GameObject playerPrefab;

    public LayerMask LayerMask;
    // 현재 플레이어 오브젝트
    public static GameObject PlayerObject { get; private set; }
    // 저장된 현재 플레이어 스탯
    private static PlayerState playerState = new PlayerState();
    private static PlayerState SavePlayerState = new PlayerState();

    public static int IsStage { get; private set; }
    private void Awake()
    {
        PlayerSupport.MapMask = LayerMask;
        TileSupport.MapMask = LayerMask;
        playerPrefab = PlayerPrefab;

    }
    private void Start()
    {
        FirstSetPlayer(CSVData.GoStage);
        StageManager.InitStage(CSVData.GoStage);
    }
    public static void FirstSetPlayer(int StageNumber)
    {
        IsStage = StageNumber;
        SetPlayer(CSVData.StageData[StageNumber]);
    }
    public static void SetPlayer(PlayerState state)
    {
        //Save로 저장
        SavePlayerState.Direction = state.Direction;
        SavePlayerState.X = state.X;
        SavePlayerState.Y = state.Y;
        SavePlayerState.Z = state.Z;

        InitPlayer();
    }
    public static void InitPlayer()
    {
        // 있으면 지움
        if (PlayerObject != null)
        {
            Destroy(PlayerObject);
        }
        PlayerObject = null;

        Vector3 dir = new Vector3(0, 0, 0);
        switch (SavePlayerState.Direction)
        {
            case Direction.East:
                dir = new Vector3(0, 0, 0);
                break;
            case Direction.South:
                dir = new Vector3(0, 90, 0);
                break;
            case Direction.West:
                dir = new Vector3(0, 180, 0);
                break;
            case Direction.North:
                dir = new Vector3(0, 270, 0);
                break;
            default:
                break;
        }

        GameObject Ins = Instantiate(playerPrefab, new Vector3(SavePlayerState.X, SavePlayerState.Y, SavePlayerState.Z), Quaternion.Euler(dir));
        PlayerObject = Ins;

        CameraMove.CameraTarget = Ins;

        playerState = SavePlayerState;
    }

    public static bool SetPlayerOrder(int OrderNumber, float MotionSpeed)
    {
        PlayerOrder Order = (PlayerOrder)OrderNumber;
        bool Check = true;

        switch (Order)
        {
            case PlayerOrder.Cancel: break;
            case PlayerOrder.Move_Front:
                {
                    GameObject obj = PlayerSupport.PlayerForwardCheck(1);
                    if (obj == null || obj.GetComponent<TileState>().staticData.Id == 2)
                    {
                        bool DoubleCheck = MoveFoward(playerState.Direction, 1, MotionSpeed);
                        if (!DoubleCheck)
                            Check = false;
                    }
                    else
                        Check = false;
                }
                break;
            case PlayerOrder.Move_Back:
                {
                    if (PlayerSupport.PlayerForwardCheck(-1) == null)
                        MoveFoward(playerState.Direction, -1, MotionSpeed);
                    else
                        Check = false;
                }
                break;
            case PlayerOrder.Turn_Right:
                {
                    Rotate(Attendants.Right, MotionSpeed);
                }
                break;
            case PlayerOrder.Turn_Left:
                {
                    Rotate(Attendants.Left, MotionSpeed);
                }
                break;
            case PlayerOrder.Jump:
                {
                    if (PlayerSupport.PlayerJumpCheck() == null && PlayerSupport.PlayerUpCheck(1) == null)
                        Jump(playerState.Direction, MotionSpeed);
                    else
                        Check = false;
                }
                break;
            case PlayerOrder.Dash:
                {
                    if (PlayerSupport.PlayerForwardCheck(2) == null)
                        MoveFoward(playerState.Direction, 2, MotionSpeed);
                    else
                        Check = false;
                }
                break;
            case PlayerOrder.Attack:
                {
                    GameObject obj = PlayerSupport.PlayerForwardCheck(1);
                    if (obj != null && obj.GetComponent<TileState>().staticData.Id == 3)
                    {
                        obj.SetActive(false);
                    }
                    else
                        Check = false;
                }
                break;
            default:
                break;
        }
        return Check;
    }

    private static bool MoveFoward(Direction direction, float distance, float MotionSpeed)
    {
        bool ObjectMove = false;
        bool PlayerMove = true;
        // 현재 위치
        Vector3 Current = PlayerObject.transform.position;

        // 방향에 따라 설정
        switch (direction)
        {
            case Direction.East:
                Current.z += distance;
                break;
            case Direction.West:
                Current.z -= distance;
                break;
            case Direction.South:
                Current.x += distance;
                break;
            case Direction.North:
                Current.x -= distance;
                break;
            default:
                break;
        }
        // 벽 밀기용도
        GameObject obj = null;
        Vector3 objCurrent = new Vector3(0, 0, 0);

        // 앞으로 갈 때만 오브젝트 체크
        if (distance > 0)
            obj = PlayerSupport.PlayerForwardCheck((int)distance);

        // 밀 수 있는 오브젝트인지 체크
        if (obj != null && obj.GetComponent<TileState>().staticData.Id == 2)
        {
            // 오브젝트 앞에 뭔가 없으면 밀어짐
            if (TileSupport.TileDirectionCheck(direction,obj, 1) == null)
            {
                objCurrent = obj.transform.position;
                // 오브젝트가 밀릴 방향 조절
                switch (direction)
                {
                    case Direction.East:
                        objCurrent.z += distance;
                        break;
                    case Direction.West:
                        objCurrent.z -= distance;
                        break;
                    case Direction.South:
                        objCurrent.x += distance;
                        break;
                    case Direction.North:
                        objCurrent.x -= distance;
                        break;
                    default:
                        break;
                }
                ObjectMove = true;
            }
            else
                PlayerMove = false;
        }

        if (ObjectMove)
        {
            obj.transform.DOMove(objCurrent, MotionSpeed);
            // 밀린 오브젝트 바닥 체크해서 떨어뜨림
        }
        if (PlayerMove)
        {
            // 위치로 이동
            PlayerObject.transform.DOMove(Current, MotionSpeed);
            return true;
        }

        return false;
    }
    public static void MoveUp(float distance, float MotionSpeed)
    {
        // 현재 위치
        Vector3 Current = PlayerObject.transform.position;

        Current.y += distance;

        // 위치로 이동
        PlayerObject.transform.DOMove(Current, MotionSpeed);
    }
    private static void Rotate(Attendants attendants, float MotionSpeed)
    {
        // 현재 각도 가져옴
        Vector3 Current = PlayerObject.transform.rotation.eulerAngles;

        // 바꿀 각도 계산 후 방향 입력
        int CurrentDir = (int)playerState.Direction;
        int NextDir = 0;

        switch (attendants)
        {
            case Attendants.Right:
                {
                    Current.y += 90;
                    NextDir = (CurrentDir + 1) % 4;
                }
                break;
            case Attendants.Left:
                {
                    Current.y -= 90;
                    // 음수 예외처리
                    if (CurrentDir != 0) { NextDir = (CurrentDir - 1) % 4; }
                    else { NextDir = 3; }
                }
                break;
            default:
                break;
        }

        // 회전 후 방향 설정
        playerState.Direction = (Direction)NextDir;
        // 방향에 따라 회전
        PlayerObject.transform.DORotate(Current, MotionSpeed);
    }

    private static void Jump(Direction direction, float MotionSpeed)
    {
        // 현재 위치
        Vector3 Current = PlayerObject.transform.position;
        Current.y += 1;

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

        // 위치로 점프
        PlayerObject.transform.DOJump(Current, 0.5f, 1, MotionSpeed);
    }
}