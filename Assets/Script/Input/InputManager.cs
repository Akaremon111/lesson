using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // InputManager�̃C���X�^���X
    public static InputManager Instance { get; private set; }

    /// <summary>
    /// �v���C���[�C���v�b�g
    /// </summary>
    private PlayerInput playerInput;

    /// <summary>
    /// �ړ��A�N�V����
    /// </summary>
    private InputAction moveAction;
    /// <summary>
    /// ���_�A�N�V����
    /// </summary>
    private InputAction lookAction;
    /// <summary>
    /// �_�b�V���A�N�V����
    /// </summary>
    private InputAction sprintAction;
    /// <summary>
    /// �W�����v�A�N�V����
    /// </summary>
    private InputAction jumpAction;
    /// <summary>
    /// �U���A�N�V����
    /// </summary>
    private InputAction attackAction;

    /// <summary>
    /// �����̓���
    /// </summary>
    public Vector2 Move { get; private set; }

    /// <summary>
    /// ���_�̓���
    /// </summary>
    public Vector2 Look { get; private set; }

    /// <summary>
    /// �_�b�V���̓���
    /// </summary>
    public float Sprint { get; private set; }

    /// <summary>
    /// �W�����v�̓���
    /// </summary>
    public float Jump { get; private set; }

    /// <summary>
    /// �U���̓���
    /// </summary>
    public float Attack {  get; private set; }

    /// <summary>
    /// ������
    /// </summary>
    private void Awake()
    {
        // �C���X�^���X������������͂��ꂪ�C���X�^���X�ł͂Ȃ��ꍇ
        if (Instance != null && Instance != this)
        {
            // GameObj�̍폜
            Destroy(gameObject);
            return;
        }
        // ������C���X�^���X�Ƃ���
        Instance = this;
        // �V�[�����܂����ł��ێ�
        DontDestroyOnLoad(gameObject);

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        InputUpdate();
    }

    /// <summary>
    /// ���͂̍X�V
    /// </summary>
    private void InputUpdate()
    {
        Move = moveAction.ReadValue<Vector2>();
        Look = lookAction.ReadValue<Vector2>();
        Sprint = sprintAction.ReadValue<float>();
        Jump = jumpAction.ReadValue<float>();
        Attack = attackAction.ReadValue<float>();
    }
}
