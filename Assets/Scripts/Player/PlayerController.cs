using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed; // 움직임 속도
    [SerializeField] private float jumpPower; // 점프 파워
    private Vector3 curMovement;

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
        Move();
    }

    private void LateUpdate()
    {
        if(canLook) Look(); 
    }

    private void Move()
    {
        // 방향 구하기
        Vector3 dir = transform.forward * curMovement.y + transform.right * curMovement.x;

        // 속도 곱하기
        dir = dir * moveSpeed;

        // y축 고정하기
        dir.y = _rigidbody.velocity.y;

        // 플레이어 이동하기
        _rigidbody.velocity = dir;
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

    public void OnJump()
    {

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
}
