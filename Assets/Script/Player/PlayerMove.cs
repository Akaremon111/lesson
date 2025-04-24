using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// Player�̈ړ����x
    /// </summary>
    [SerializeField]
    private float moveSpeed = 2.0f;

    /// <summary>
    /// �J�����̌����Ă������
    /// </summary>
    Vector3 cameraForward;

    /// <summary>
    /// Player�̓�������
    /// </summary>
    Vector3 moveForward;

    /// <summary>
    /// ���݂̃|�W�V����
    /// </summary>
    private Transform PlayerTransform;

    /// <summary>
    /// �P�t���[���O�̃|�W�V����
    /// </summary>
    private Vector3 previousPos;
    private float crrentVerocity;

    /// <summary>
    /// ��]����
    /// </summary>
    [SerializeField]
    private float smoothTime;

    /// <summary>
    /// ��]����ő�̃X�s�[�h
    /// </summary>
    [SerializeField]
    private float maxSpeed;

    /// <summary>
    /// InputManager����󂯎��
    /// </summary>
    // Move
    private Vector2 move;
    // Sprint
    private float sprint;

    /// <summary>
    /// Animation
    /// </summary>
    private Animator animator;

    /// <summary>
    /// �ړ���Animation
    /// </summary>
    private int pMoveAnimation = 0;
    private int getMoveAnimation;

    private void Awake()
    {
        // �A�j���[�V�����R���|�[�l���g�擾
        animator = GetComponent<Animator>();

        // transform�̎擾
        PlayerTransform = transform;

        // Player�̈ʒu�̕ۑ�
        previousPos = PlayerTransform.position;
    }

    private void Update()
    {
        getInput();

        // �ړ������̏���
        moveDirection();

        // Player�̊p�x�̌v�Z
        moveAngle();

        // �A�j���[�V�����𓮂���
        moveAnimation();
    }

    private void FixedUpdate()
    {
        // �ړ��̏���
        Move();
    }

    /// <summary>
    /// Player�̈ړ��̏���
    /// �ړ������̂���FixedUpdate����̌Ăяo��
    /// </summary>
    private void Move()
    {
        // �J�����̕�����O�Ƃ��Ĉړ����s��
        transform.position += moveForward * Time.deltaTime * moveSpeed;
    }

    /// <summary>
    /// InputManager����̎󂯎��
    /// </summary>
    private void getInput()
    {
        // Move���󂯎��
        move = InputManager.Instance.Move;

        // Sprint���󂯎��
        sprint = InputManager.Instance.Sprint;
    }

    /// <summary>
    /// Player�̈ړ�����
    /// </summary>
    private void moveDirection()
    {
        // �J�����̑O�����̎擾
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �擾�����J�����̕�����Player�̑O�����ɂ���
        moveForward = cameraForward * move.y + Camera.main.transform.right * move.x;

        // Player�X�s�[�h�̕ύX
        moveSpeed = sprint > 0 ? 5 : 2;
    }

    /// <summary>
    /// Player�̈ړ������̊p�x�v�Z
    /// </summary>
    private void moveAngle()
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
    private void moveAnimation()
    {
        // �ړ��L�[��������Ă��鎞
        if (move.magnitude > 0)
        {
            // �������̃A�j���[�V����
            pMoveAnimation = 1;

            // Shift�L�[��������Ă���Ƃ�
            if (sprint > 0)
            {
                // �����Ă��鎞�̏���
                pMoveAnimation = 2;
            }
        }
        else
        {
            // �A�C�h�����̃A�j���[�V����
            pMoveAnimation = 0;
        }
        // �l������Ȃ��Ƃ��ɓǂݍ���
        if (pMoveAnimation != getMoveAnimation)
        {
            // �l�𓯂��ɂ���
            getMoveAnimation = pMoveAnimation;

            // PlayerMoveAnimation�̒l��getComboAnimation�ɂ���
            animator.SetInteger("PlayerMoveAnimation", getMoveAnimation);
        }
    }
}