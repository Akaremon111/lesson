using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerMove : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    /// <summary>
    /// Player�̈ړ����x
    /// </summary>
    [SerializeField]
    private float moveSpeed = 2.0f;

    /// <summary>
    /// �W�����v�̍���
    /// </summary>
    [SerializeField]
    private float JumpHight;

    /// <summary>
    /// �W�����v�̍ő�̍���
    /// </summary>
    [SerializeField]
    private float MaxJump = 3;

    /// <summary>
    /// Player�ɂ�����d��
    /// </summary>
    [SerializeField]
    private float Gravity;

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

    [Header("���̑��ݒ�")]
    // SphereCast�̔��a
    [SerializeField]
    private float sphereRadius;
    /// <summary>
    /// SphereCast�̍ő�̋���
    /// </summary>
    [SerializeField]
    private float maxDistance;

    // SphereCast�̌��_
    private Vector3 origin;

    /// <summary>
    /// ���ݒn�ʂɂ���̂��ǂ����̔���
    /// </summary>
    private bool isGround = true;

    /// <summary>
    /// �J�����̌����Ă������
    /// </summary>
    Vector3 cameraForward;

    /// <summary>
    /// Player�̓�������
    /// </summary>
    Vector3 moveForward;

    /// <summary>
    /// Transgorm
    /// </summary>
    private Transform PlayerTransform;

    /// <summary>
    /// ���݂̃|�W�V����
    /// </summary>
    Vector3 nowPos;

    /// <summary>
    /// �P�t���[���O�̃|�W�V����
    /// </summary>
    private Vector3 previousPos;
    private float crrentVerocity;

    /// <summary>
    /// ���������̑��x
    /// </summary>
    private Vector3 Velocity;

    /// <summary>
    /// InputManager����󂯎��
    /// </summary>
    // Move
    private Vector2 move;
    // Sprint
    private float sprint;
    // Jump
    private float jump;

    /// <summary>
    /// Animation
    /// </summary>
    private Animator animator;

    /// <summary>
    /// CharacterController
    /// </summary>
    private CharacterController characterController;

    /// <summary>
    /// �ړ���Animation
    /// </summary>
    private int pMoveAnimation = 0;
    private int getMoveAnimation;

    /// <summary>
    /// 
    /// </summary>
    private Vector3 velocity;

    private void Awake()
    {
        // �A�j���[�V�����R���|�[�l���g�擾
        animator = GetComponent<Animator>();

        // �L�����N�^�[�R���g���[���[�̃R���|�[�l���g�擾
        characterController = GetComponent<CharacterController>();

        Velocity = Vector3.zero;

        // transform�̎擾
        PlayerTransform = transform;

        // Player�̈ʒu�̕ۑ�
        previousPos = PlayerTransform.position;
    }

    private void Update()
    {
        // ���͂��󂯎��
        getInput();

        // �ړ������̏���
        moveDirection();

        // Player�̊p�x�̌v�Z
        moveAngle();

        // �A�j���[�V�����𓮂���
        moveAnimation();

        // �W�����v
        PlayerJump();
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

        // Jump���󂯎��
        jump = InputManager.Instance.Jump;
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
    /// Player�̃W�����v�̏������s��
    /// </summary>
    private void PlayerJump()
    {
        // isGround��false�ɂ���
        isGround = false;

        // Ray�̌��_
        origin = transform.position + new Vector3(0.0f, 0.3f, 0.0f);

        // Ray�����������I�u�W�F�N�g�̏��i�[
        RaycastHit hit;

        // Ray�iSphereCast�j���o��
        if (Physics.SphereCast(origin, sphereRadius, Vector3.down, out hit, maxDistance))
        {
            // 
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                isGround = true;
            }
        }

        if (characterController.isGrounded || isGround)
        {
            // �n�ʂɂ���Ƃ������W�����v�ł���
            if (jump > 0f)
            {
                Velocity.y = Mathf.Sqrt(JumpHight * -1f * Physics.gravity.y); // �W�����v�����x���v�Z
            }
        }

        if (!isGround)
        {
            // ��ɏd�͂�K�p

        }
        Velocity.y += Physics.gravity.y * Time.deltaTime;

        characterController.Move(new Vector3(0, Velocity.y, 0) * Time.deltaTime);

        Debug.Log("�n�ʂɂ���ł��傤��:" + isGround);
    }

    /// <summary>
    /// SphereCast�̉���
    /// </summary>
    private void OnDrawGizmos()
    {
        // �J���[�͐�
        Gizmos.color = Color.blue;

        // Ray�̈ʒu�Ɖ���������ꏊ����v������
        Gizmos.DrawWireSphere(origin, sphereRadius);
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
        nowPos = PlayerTransform.position;

        // �ړ��ʂ̌v�Z
        Vector3 Alldelta = nowPos - previousPos;

        // �K�v�Ȏ��̏�񂾂������o���iJump�Ȃǂɉe�����Ȃ��悤�ɂ���j
        Vector3 delta = new Vector3(Alldelta.x, 0.0f, Alldelta.z);

        // �O�̈ʒu�̍X�V
        previousPos = nowPos;

        // �Î~��Ԃ̎��͉�]���Ȃ�
        if (delta == Vector3.zero) return;

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