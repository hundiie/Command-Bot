using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    public static GameObject CameraObject, ThisObject;

    public static GameObject CameraTarget;

    private void Awake()
    {
        ThisObject = this.gameObject;
        CameraObject = Camera.main.gameObject;
    }

    private Vector3 touchedPos;
    private bool touchOn;

    private void Update()
    {
        if (CameraTarget != null)
            transform.position = CameraTarget.transform.position;
        if (Input.touchCount > 0)
        {    //터치가 1개 이상이면.
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (!EventSystem.current.IsPointerOverGameObject(i))
                {
                    MoveTarget();

                    //Touch tempTouchs = Input.GetTouch(i);
                    //if (tempTouchs.phase == TouchPhase.Began)
                    //{    //해당 터치가 시작됐다면.
                    //    touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.
                    //    touchOn = true;

                    //    break;   //한 프레임(update)에는 하나만.
                    //}
                }
            }
        }
    }

    private Vector2 nowPos, prePos;
    private Vector3 movePos;
    private float Speed = 0.1f;
    private void MoveTarget()
    {
        if (CameraTarget == null)
            return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * Speed;
                
                prePos = touch.position - touch.deltaPosition;
                
            }
           // prePos.y = Mathf.Clamp(prePos.y, -70f, 10f);
            transform.eulerAngles = new Vector3(-prePos.y, prePos.x, 0);
        }

        //MouseX += Input.GetAxis("Mouse X");// * MouseSpeed;
        //MouseY += Input.GetAxis("Mouse Y");// * MouseSpeed;
        //MouseY prePos.y = Mathf.Clamp(prePos.y, -90f, 20f);
        //Camera.main.transform.eulerAngles = new Vector3(-prePos.y, prePos.x, 0);
    }
}
