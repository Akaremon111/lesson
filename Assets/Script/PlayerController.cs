using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player�̈ړ��X�s�[�h
    private float PlayerSpeed;

    // �ړ��L�[�̓���
    private float InputX;
    private float InputZ;

    /// <summary>
    /// PlayerMove�Ŏg�p
    /// </summary>

    //�J�����̌����Ă������
    Vector3 cameraForward;
    // Player�̓�������
    Vector3 moveForward;


    /// <summary>
    /// PlayerAngle�Ŏg�p
    /// </summary>

    // ���݂̃|�W�V����
    private Transform PlayerTransform;
    // �P�t���[���O�̃|�W�V����
    private Vector3 previousPos;

    float crrentVerocity;

    // ��]����
    [SerializeField]
    private float smoothTime;
    // ��]����ő�̃X�s�[�h
    [SerializeField]
    private float maxSpeed;


    /// <summary>
    /// �L�[�{�[�h�̓��͌��m
    /// </summary>
    // W�L�[�̓���
    private bool wKey;
    // A�L�[�̓���
    private bool aKey;
    // S�L�[�̓���
    private bool sKey;
    // D�L�[�̓���
    private bool dKey;
    // shift�L�[�̓���
    private bool shiftKey;

    /// <summary>
    /// �}�E�X�̓��͌��m
    /// </summary>
    // ���N���b�N�̓��͌��m
    private bool leftButton;

    /// <summary>
    /// Animation
    /// </summary>
    private Animator animator;

    // �ړ���Animation
    private int pMoveAnimation = 0;
    private int getMoveAnimation;

    private void Awake()
    {
        // Player�̃A�j���[�V����
        animator = GetComponent<Animator>();

        PlayerTransform = transform;
        // 
        previousPos = PlayerTransform.position;
    }

    private void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        // Player�̓���
        PlayerMove();

        // Player�̊p�x
        PlayerAngle();

        // ���͌��m
        KeyInput();

        // �^�C�}�[�֐�

    }


    /// <summary>
    /// Player�̓���
    /// </summary>
    private void PlayerMove()
    {
        // �J�����̑O�����̎擾
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        // �擾�����J�����̕�����Player�̑O�����ɂ���
        moveForward = cameraForward * InputZ + Camera.main.transform.right * InputX;

        // Player�X�s�[�h�̕ύX
        PlayerSpeed = shiftKey ? 5 : 2;
    }

    private void FixedUpdate()
    {
        // �J�����̕�����O�Ƃ��Ĉړ����s��
        transform.position += moveForward * Time.deltaTime * PlayerSpeed;
    }

    /// <summary>
    /// Player�̈ړ������̊p�x�v�Z
    /// </summary>
    private void PlayerAngle()
    {
        // ���݂̈ʒu
        Vector3 nowPos = PlayerTransform.position;
        // �ړ��ʂ̌v�Z
        Vector3 delta = nowPos - previousPos;
        // �O�̈ʒu�̍X�V
        previousPos = nowPos;

        // �Î~��Ԃ̎��͉�]���Ȃ�
        if (delta == Vector3.zero)
        {
            return;
        }

        // ��]����
        Quaternion targetRot = Quaternion.LookRotation(delta, Vector3.up);

        // ���݂̕����Ɛi�s�����Ƃ̊p�x���v�Z
        float diffAngle = Vector3.Angle(PlayerTransform.forward, delta);

        // ��]���ԂƃX�s�[�h�̌v�Z
        float rotAngle = Mathf.SmoothDampAngle(0, diffAngle, ref crrentVerocity, smoothTime, maxSpeed);

        // ���݂̊p�x����targetRot�Ɍ������ĉ�]
        Quaternion nextRot = Quaternion.RotateTowards(PlayerTransform.rotation, targetRot, rotAngle);
        PlayerTransform.rotation = nextRot;
    }

    /// <summary>
    /// Player�̃A�j���[�V����
    /// </summary>
    void PlayerAnimation()
    {
        // �ړ��L�[��������Ă��鎞
        if((wKey || aKey || sKey || dKey )&& !shiftKey)
        {
            // �������̃A�j���[�V����
            pMoveAnimation = 1;
            Debug.Log(pMoveAnimation);
        }
        if(!(wKey || aKey || sKey || dKey))
        {
            // �A�C�h�����̃A�j���[�V����
            pMoveAnimation = 0;
            Debug.Log(pMoveAnimation);
        }
        if ((wKey || aKey || sKey || dKey) && shiftKey)
        {
            // �����Ă��鎞�̏���
            pMoveAnimation = 2;
            Debug.Log(pMoveAnimation);
        }
        // �l������Ȃ��Ƃ��ɓǂݍ���
        if(pMoveAnimation != getMoveAnimation)
        {
            // �l�𓯂��ɂ���
            getMoveAnimation = pMoveAnimation;

            // PlayerMoveAnimation�̒l��getComboAnimation�ɂ���
            animator.SetInteger("PlayerMoveAnimation", getMoveAnimation);
        }

        // �U���̃A�j���[�V����
        if (leftButton)
        {
            animator.SetTrigger("PlayerFastAttackAnimation");
            animator.SetTrigger("PlayerSecondAttackAnimation");
            animator.SetTrigger("PlayerThirdAttackAnimation");
        }
    }

    /// <summary>
    /// ���͂̌��m
    /// </summary>
    private void KeyInput()
    {
        // W�L�[�̓��͌��m
        wKey = Input.GetKey(KeyCode.W) ? true : false;
        // A�L�[�̓��͌��m
        aKey = Input.GetKey(KeyCode.A) ? true : false;
        // S�L�[�̓��͌��m
        sKey = Input.GetKey(KeyCode.S) ? true : false;
        // D�L�[�̓��͌��m
        dKey = Input.GetKey(KeyCode.D) ? true : false;

        // ���N���b�N�̓��͌��m
        leftButton = Input.GetMouseButtonDown(0) ? true : false;

        // Shift�L�[�̓��͌��m
        shiftKey = Input.GetKey(KeyCode.LeftShift) ? true : false;

        // �L�[���͂ŃA�j���[�V�����𓮂���
        PlayerAnimation();
    }

    void JoyStickInput()
    {

    }
}
