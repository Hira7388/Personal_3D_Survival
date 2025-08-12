using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;
//using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed; // 움직임 속도
    [SerializeField] private float jumpPower; // 점프 파워
    [SerializeField] private float airControlForce = 10f; // 공중에서 움직임
    [SerializeField] private float stickToGroundForce = 5f; // 땅에 붙어 있으려는 힘(조그마한 오브젝트를 지날 때도 InAir상태가 되는 것을 줄여준다)
    private Vector3 curMovement;
    public LayerMask groundLayerMask;

    [Header("Look")]
    [SerializeField] private float minXLook; // 카메라 최소 각도
    [SerializeField] private float maxXLook; // 카메라 최대 각도
    [SerializeField] private float lookSensitivity; // 마우스 민감도
    [SerializeField] private Transform cameraContainer; // 카메라 회전을 위한 컨테이너 가져오기
    private float curCamXRot; // 현재 카메라의 상하 회전을 위한 변수
    private float curPlayerYRot;
    private Vector2 mouseDelta; // 마우스의 회전 정보
    public bool canLook = true;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        // 마우스 커서 숨기기
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

    }

    // 물리 계산이 Update보다 더 자주 계산된다.
    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            MoveOnGround();
        }
        else
        {
            MoveInAir();
        }
    }

    private void LateUpdate()
    {
        if (canLook) { Look(); }
    }

    private void MoveOnGround()
    {
        Vector3 dir = transform.forward * curMovement.y + transform.right * curMovement.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y; // Y축 속도는 보존
        _rigidbody.velocity = dir;

        _rigidbody.AddForce(Vector3.down * stickToGroundForce, ForceMode.Force);
    }

    private void MoveInAir()
    {
        // AddForce를 사용해서 공중에서 약간의 힘만 가합니다.
        // 이렇게 하면 발사체에서 받은 큰 속력을 덮어쓰지 않고 더해줍니다.
        Vector3 dir = transform.forward * curMovement.y + transform.right * curMovement.x;
        _rigidbody.AddForce(dir * airControlForce, ForceMode.Force);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 현재 받아온 context가 눌려져 있는 상황이라면
        if(context.phase == InputActionPhase.Performed)
        {
            // Vector2 value를 받아온다.
            curMovement = context.ReadValue<Vector2>();
        }
        // context가 떼져 있는 상황
        else if(context.phase == InputActionPhase.Canceled)
        {
            // 움직임을 멈춘다.
            curMovement = Vector2.zero;
        }
    }

    private void Look()
    {
        // 현재 카메라의 회전에 마우스 민감도에 맞춰서 회전값을 더 곱해준다.
        curCamXRot += mouseDelta.y * lookSensitivity;
        // 상하 회전의 제한 값을 설정한다.
        curCamXRot = Mathf.Clamp(curCamXRot, minXLook, maxXLook);
        // 실제 카메라를 회전한다.
        cameraContainer.localEulerAngles = new Vector3(-curCamXRot, 0, 0);

        // 플레이어의 회전을 민감도에 맞춰서 회전값을 더 곱한다.
        curPlayerYRot += mouseDelta.x * lookSensitivity;
        // 좌우는 캐릭터의 스프라이트도 같이 움직여야 하기 때문에 local을 사용하지 않음
        transform.eulerAngles = new Vector3(0, curPlayerYRot, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // 바라보려고 움직인 벡터 값
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("점프 입력 받음");
        if ((context.phase == InputActionPhase.Started) && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                //Debug.Log("땅에 붙어있음");
                return true;
            }
        }

        //Debug.Log("공중에 떠있음");
        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        // 키가 눌렸을 때 '한 번만' 호출되도록 Performed 단계에서 확인합니다.
        if (context.phase == InputActionPhase.Performed)
        {
            // UIManager를 통해 InventoryUI의 Toggle 함수를 호출합니다.
            UIManager.Instance.inventoryUI.Toggle();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    public void SetCanLook(bool value)
    {
        canLook = value;
    }
}
