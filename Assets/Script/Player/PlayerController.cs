using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TimerControllerクラス
    private TimerController timerController;

    /// <summary>
    /// マウスの入力検知
    /// </summary>
    // 左クリックの入力検知
    private bool LeftButton;

    /// <summary>
    /// Animation
    /// </summary>
    private Animator animator;

    // 移動のAnimation
    private int pMoveAnimation = 0;
    private int getMoveAnimation;

    // コンボ攻撃の1段目
    private bool isComboAttack1 = true;
    // コンボ攻撃の2段目
    private bool isComboAttack2 = false;
    // コンボ攻撃の3段目
    private bool isComboAttack3 = false;

    private void Awake()
    {
        // Playerのアニメーション
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Playerのアニメーション
    /// </summary>
    private void PlayerAnimation()
    {
        // 左クリックを押されたとき
        if(LeftButton)
        {
            if(isComboAttack1)
            {
                // 1段目の攻撃ができないようにする
                isComboAttack1 = false;
                // 1段目の攻撃アニメーションを開始
                animator.SetBool("PlayerAttackAnimation1", true);

                // タイマーを開始
                timerController.IsStartTimer = true;

                // 攻撃アニメーション開始から2秒経過
                if (timerController.ElapsedTime >= 2)
                {
                    // 一段目の攻撃を再度行えるようにする
                    isComboAttack1 |= true;
                    // 攻撃アニメーションをfalseにする
                    animator.SetBool("PlayerAttackAnimation1", false);
                }
                // 次の攻撃の許可を出す
                else isComboAttack2 = true;
            }
        }
    }
}
