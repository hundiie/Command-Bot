using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// ����
public enum Direction
{
    East,   // �� = 0
    South,  // �� = 90
    West,   // �� = 180
    North,   // �� = 270
}
// ����
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
    // ���� �÷��̾� ������Ʈ
    public static GameObject PlayerObject { get; private set; }
    // ����� ���� �÷��̾� ����
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
        //Save�� ����
        SavePlayerState.Direction = state.Direction;
        SavePlayerState.X = state.X;
        SavePlayerState.Y = state.Y;
        SavePlayerState.Z = state.Z;

        InitPlayer();
    }
    public static void InitPlayer()
    {
        // ������ ����
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
        // ���� ��ġ
        Vector3 Current = PlayerObject.transform.position;

        // ���⿡ ���� ����
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
        // �� �б�뵵
        GameObject obj = null;
        Vector3 objCurrent = new Vector3(0, 0, 0);

        // ������ �� ���� ������Ʈ üũ
        if (distance > 0)
            obj = PlayerSupport.PlayerForwardCheck((int)distance);

        // �� �� �ִ� ������Ʈ���� üũ
        if (obj != null && obj.GetComponent<TileState>().staticData.Id == 2)
        {
            // ������Ʈ �տ� ���� ������ �о���
            if (TileSupport.TileDirectionCheck(direction,obj, 1) == null)
            {
                objCurrent = obj.transform.position;
                // ������Ʈ�� �и� ���� ����
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
            // �и� ������Ʈ �ٴ� üũ�ؼ� ����߸�
        }
        if (PlayerMove)
        {
            // ��ġ�� �̵�
            PlayerObject.transform.DOMove(Current, MotionSpeed);
            return true;
        }

        return false;
    }
    public static void MoveUp(float distance, float MotionSpeed)
    {
        // ���� ��ġ
        Vector3 Current = PlayerObject.transform.position;

        Current.y += distance;

        // ��ġ�� �̵�
        PlayerObject.transform.DOMove(Current, MotionSpeed);
    }
    private static void Rotate(Attendants attendants, float MotionSpeed)
    {
        // ���� ���� ������
        Vector3 Current = PlayerObject.transform.rotation.eulerAngles;

        // �ٲ� ���� ��� �� ���� �Է�
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
                    // ���� ����ó��
                    if (CurrentDir != 0) { NextDir = (CurrentDir - 1) % 4; }
                    else { NextDir = 3; }
                }
                break;
            default:
                break;
        }

        // ȸ�� �� ���� ����
        playerState.Direction = (Direction)NextDir;
        // ���⿡ ���� ȸ��
        PlayerObject.transform.DORotate(Current, MotionSpeed);
    }

    private static void Jump(Direction direction, float MotionSpeed)
    {
        // ���� ��ġ
        Vector3 Current = PlayerObject.transform.position;
        Current.y += 1;

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

        // ��ġ�� ����
        PlayerObject.transform.DOJump(Current, 0.5f, 1, MotionSpeed);
    }
}