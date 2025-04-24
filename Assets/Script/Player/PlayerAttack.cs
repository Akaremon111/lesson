using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    /// <summary>
    /// Animation
    /// </summary>
    private Animator animator;

    /// <summary>
    /// InputManager����󂯎��
    /// </summary>
    private float attack;

    // �������u�Ԃ�����������悤�ɂ���
    private bool isClick = true;

    /// <summary>
    /// �R���{�U���̉�
    /// </summary>
    private float AttackComboCount = 0;

    /// <summary>
    /// �R���{�U�����ł��鎞�Ԃ̌v��
    /// </summary>
    private float ComboTimer = 0;

    private void Awake()
    {
        // �A�j���[�^�[�̃R���|�[�l���g�擾
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // ���͔���
        getInput();

        // �U�����̃A�j���[�V����
        AttackAnimation();
    }

    /// <summary>
    /// InputManager����U�����̓��͔�����󂯎��
    /// </summary>
    private void getInput()
    {
        // Attack�̎󂯎��
        attack = InputManager.Instance.Attack;
    }

    /// <summary>
    /// �R���{�U���̃A�j���[�V��������
    /// </summary>
    private void AttackAnimation()
    {
        // ���N���b�N�̓��͂����m�����Ƃ�
        if (attack > 0 && isClick)
        {
            Debug.Log(AttackComboCount);

            // ������x�����Ȃ��Ƃ��߂ɂ���
            isClick = false;

            // 1�i�ڂ̍U���A�j���[�V����
            if(AttackComboCount == 0)
            {
                animator.SetBool("PlayerAttackAnimation1", true);
                Debug.Log("�U��1��ځI");
            }
            else if (AttackComboCount == 1)
            {
                animator.SetBool("PlayerAttackAnimation2", true);
                Debug.Log("�U��2��ځI");
            }
            else if (AttackComboCount == 2)
            {
                animator.SetBool("PlayerAttackAnimation3", true);
                Debug.Log("�U��3��ځI");
            }

            // Timer���J�n
            TimerController.Instance.IsStartTimer = true;
        }
        // 1�b�ȓ��Ɏ��̍U���̓��͂��󂯎������
        if(TimerController.Instance.ElapsedTime < 1.0f || AttackComboCount < 2)
        {
            // Timer�̃X�g�b�v�ƃ��Z�b�g
            AttackTimerReset();

            // �R���{�̃J�E���g��i�߂�
            AttackComboCount++;
        }
        // �^�C����2�b�ȏ��������Attack�J�E���g��3�ȏ�ɂȂ��
        if (TimerController.Instance.ElapsedTime >= 1.0f || AttackComboCount >= 3)
        {
            // Timer�̃X�g�b�v�ƃ��Z�b�g
            AttackTimerReset();

            // AttackComboCount��0�ɖ߂�
            AttackComboCount = 0;

            // �A�^�b�N�A�j���[�V������false�ɂ���
            animator.SetBool("PlayerAttackAnimation1", false);

            animator.SetBool("PlayerAttackAnimation2", false);

            animator.SetBool("PlayerAttackAnimation3", false);
        }

        // �{�^���������玟�̃N���b�N��������
        if (attack == 0)
        {
            isClick = true;
        }
    }

    /// <summary>
    /// Timer�̃X�g�b�v�ƃ��Z�b�g
    /// </summary>
    private void AttackTimerReset()
    {
        // �^�C�}�[���~�߂�
        TimerController.Instance.IsStartTimer = false;

        // �^�C�}�[�̃��Z�b�g
        TimerController.Instance.IsResetTimer = true;
    }
}
