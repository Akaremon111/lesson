using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// PlayerManager�̎擾
    /// </summary>
    private PlayerManager playerManager;

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
    /// None�̏�Ԃɂ��邩�̃t���O
    /// </summary>
    private bool isNone = false;

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
    private Vector3 nowPos;

    /// <summary>
    /// �P�t���[���O�̃|�W�V����
    /// </summary>
    private Vector3 previousPos;

    /// <summary>
    /// ���݂�Verocity
    /// </summary>
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

    private void Awake()
    {
        // PlayerManager�̎擾
        playerManager = GetComponent<PlayerManager>();

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
        // Player�̏�Ԃ��z�o�[�{�[�h�ɏ���Ă��Ȃ����U�������Ă��Ȃ��Ƃ�
        if(playerManager.state != PlayerState.OnBoard && playerManager.state != PlayerState.Attacking)
        {
            // �n�ʂɂ���Ƃ�
            if (characterController.isGrounded)
            {
                // �n�ʂɂ���Ƃ������W�����v�ł���
                if (jump > 0f)
                {
                    // �W�����v��Ԃɂ���
                    playerManager.state = PlayerState.Jumping;

                    // �W�����v�̑��x�v����
                    Velocity.y = Mathf.Sqrt(JumpHight * -1f * Physics.gravity.y);

                    // isNone��false�ɂ���
                    isNone = false;
                }
            }
            else
            {
                // isNone��false�̎�
                if (!isNone)
                {
                    // isNone��true�ɂ���
                    isNone = true;

                    // �n�ʂɂ���Ƃ��͉������Ȃ���Ԃɂ���
                    playerManager.state = PlayerState.None;
                }
            }
            // �d�͂�^����
            Velocity.y += Physics.gravity.y * Time.deltaTime;

            // �ړ��̏������s��
            characterController.Move(new Vector3(0, Velocity.y, 0) * Time.deltaTime);
        }
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

        // ���݂̊p�x����targetRot�Ɍ������Ẳ�]�ʂ̌v�Z
        Quaternion nextRot = Quaternion.RotateTowards(PlayerTransform.rotation, targetRot, rotAngle);

        // ��]���s��
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