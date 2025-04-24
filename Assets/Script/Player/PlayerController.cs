using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TimerController�N���X
    private TimerController timerController;

    /// <summary>
    /// �}�E�X�̓��͌��m
    /// </summary>
    // ���N���b�N�̓��͌��m
    private bool LeftButton;

    /// <summary>
    /// Animation
    /// </summary>
    private Animator animator;

    // �ړ���Animation
    private int pMoveAnimation = 0;
    private int getMoveAnimation;

    // �R���{�U����1�i��
    private bool isComboAttack1 = true;
    // �R���{�U����2�i��
    private bool isComboAttack2 = false;
    // �R���{�U����3�i��
    private bool isComboAttack3 = false;

    private void Awake()
    {
        // Player�̃A�j���[�V����
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Player�̃A�j���[�V����
    /// </summary>
    private void PlayerAnimation()
    {
        // ���N���b�N�������ꂽ�Ƃ�
        if(LeftButton)
        {
            if(isComboAttack1)
            {
                // 1�i�ڂ̍U�����ł��Ȃ��悤�ɂ���
                isComboAttack1 = false;
                // 1�i�ڂ̍U���A�j���[�V�������J�n
                animator.SetBool("PlayerAttackAnimation1", true);

                // �^�C�}�[���J�n
                timerController.IsStartTimer = true;

                // �U���A�j���[�V�����J�n����2�b�o��
                if (timerController.ElapsedTime >= 2)
                {
                    // ��i�ڂ̍U�����ēx�s����悤�ɂ���
                    isComboAttack1 |= true;
                    // �U���A�j���[�V������false�ɂ���
                    animator.SetBool("PlayerAttackAnimation1", false);
                }
                // ���̍U���̋����o��
                else isComboAttack2 = true;
            }
        }
    }
}
