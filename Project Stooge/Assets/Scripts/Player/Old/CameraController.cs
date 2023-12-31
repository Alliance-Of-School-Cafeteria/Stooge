using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera playerCamera;

    private Vector3 lookForward;
    private Vector3 lookRight;
    private Vector3 moveDir;

    //[Header("오브젝트 연결")]
    //[SerializeField]

    [Header("설정")]
    [Range(1f, 5f)]
    private float cameraSpeed = 2f;


    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * cameraSpeed, Input.GetAxis("Mouse Y") * cameraSpeed);   // 마우스 움직임
        Vector3 camAngle = transform.rotation.eulerAngles;    // 카메라 위치 값을 오일러 각으로 변환

        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)   // 위쪽 70도 제한
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else            // 아래쪽 25도 제한
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        transform.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);    // 새 회전 값
    }

    public Vector3 GetMoveDir(Vector2 moveInput)
    {
        lookForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized; // 정면 방향 저장
        lookRight = new Vector3(transform.right.x, 0f, transform.right.z).normalized;       // 좌우 방향 저장

        moveDir = (lookForward * moveInput.y) + (lookRight * moveInput.x); // 바라보는 방향 기준 이동 방향

        return moveDir;
    }
}
