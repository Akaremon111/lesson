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

    // �R���{���q����P�\����
    private float comboInterval = 1.0f;

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

        // ComboTimer�̃J�E���g�_�E��
        if (ComboTimer > 0)
        {
            ComboTimer -= Time.deltaTime;
        }

        // �^�C�����؂ꂽ��R���{���Z�b�g
        if (ComboTimer <= 0 && AttackComboCount > 0)
        {
            AttackTimerReset();
            AttackComboCount = 0;

            // �A�j���[�V������Ԃ����Z�b�g
            animator.SetBool("PlayerAttackAnimation1", false);
            animator.SetBool("PlayerAttackAnimation2", false);
            animator.SetBool("PlayerAttackAnimation3", false);
        }
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

            // ������x�����Ȃ��Ƃ��߂ɂ���
            isClick = false;

            // �R���{�̃J�E���g��i�߂�
            AttackComboCount++;

            // Timer�����X�^�[�g
            ComboTimer = comboInterval;

            // �U���A�j���[�V����
            if (AttackComboCount == 1)
            {
                animator.SetBool("PlayerAttackAnimation1", true);
            }
            else if (AttackComboCount == 2)
            {
                animator.SetBool("PlayerAttackAnimation2", true);
            }
            else if (AttackComboCount == 3)
            {
                animator.SetBool("PlayerAttackAnimation3", true);
            }

            // Timer���J�n�i�O���^�C�}�[���K�v�Ȃ炱���Łj
            TimerController.Instance.IsStartTimer = true;
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
