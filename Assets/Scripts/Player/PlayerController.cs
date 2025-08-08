using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed; // ������ �ӵ�
    [SerializeField] private float jumpPower; // ���� �Ŀ�
    private Vector3 curMovement;

    [Header("Look")]
    [SerializeField] private float minXLook; // ī�޶� �ּ� ����
    [SerializeField] private float maxXLook; // ī�޶� �ִ� ����
    [SerializeField] private float lookSensitivity; // ���콺 �ΰ���
    [SerializeField] private Transform cameraContainer; // ī�޶� ȸ���� ���� �����̳� ��������
    private float curCamXRot; // ���� ī�޶��� ���� ȸ���� ���� ����
    private float curPlayerYRot;
    private Vector2 mouseDelta; // ���콺�� ȸ�� ����

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        // ���콺 Ŀ�� �����
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

    }

    // ���� ����� Update���� �� ���� ���ȴ�.
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Move()
    {
        // ���� ���ϱ�
        Vector3 dir = transform.forward * curMovement.y + transform.right * curMovement.x;

        // �ӵ� ���ϱ�
        dir = dir * moveSpeed;

        // y�� �����ϱ�
        dir.y = _rigidbody.velocity.y;

        // �÷��̾� �̵��ϱ�
        _rigidbody.velocity = dir;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        // ���� �޾ƿ� context�� ������ �ִ� ��Ȳ�̶��
        if(context.phase == InputActionPhase.Performed)
        {
            // Vector2 value�� �޾ƿ´�.
            curMovement = context.ReadValue<Vector2>();
        }
        // context�� ���� �ִ� ��Ȳ
        else if(context.phase == InputActionPhase.Canceled)
        {
            // �������� �����.
            curMovement = Vector2.zero;
        }
    }

    private void Look()
    {
        // ���� ī�޶��� ȸ���� ���콺 �ΰ����� ���缭 ȸ������ �� �����ش�.
        curCamXRot += mouseDelta.y * lookSensitivity;
        // ���� ȸ���� ���� ���� �����Ѵ�.
        curCamXRot = Mathf.Clamp(curCamXRot, minXLook, maxXLook);
        // ���� ī�޶� ȸ���Ѵ�.
        cameraContainer.localEulerAngles = new Vector3(-curCamXRot, 0, 0);


        curPlayerYRot += mouseDelta.x * lookSensitivity;
        // �¿�� ĳ������ ��������Ʈ�� ���� �������� �ϱ� ������ local�� ������� ����
        transform.eulerAngles = new Vector3(0, curPlayerYRot, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // �ٶ󺸷��� ������ ���� ��
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump()
    {

    }
}
