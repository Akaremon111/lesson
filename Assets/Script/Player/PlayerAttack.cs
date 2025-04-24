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
    /// InputManagerから受け取る
    /// </summary>
    private float attack;

    // 押した瞬間だけ反応するようにする
    private bool isClick = true;

    /// <summary>
    /// コンボ攻撃の回数
    /// </summary>
    private float AttackComboCount = 0;

    /// <summary>
    /// コンボ攻撃ができる時間の計測
    /// </summary>
    private float ComboTimer = 0;

    private void Awake()
    {
        // アニメーターのコンポーネント取得
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 入力判定
        getInput();

        // 攻撃時のアニメーション
        AttackAnimation();
    }

    /// <summary>
    /// InputManagerから攻撃時の入力判定を受け取る
    /// </summary>
    private void getInput()
    {
        // Attackの受け取り
        attack = InputManager.Instance.Attack;
    }

    /// <summary>
    /// コンボ攻撃のアニメーション処理
    /// </summary>
    private void AttackAnimation()
    {
        // 左クリックの入力を検知したとき
        if (attack > 0 && isClick)
        {
            Debug.Log(AttackComboCount);

            // もう一度押さないとだめにする
            isClick = false;

            // 1段目の攻撃アニメーション
            if(AttackComboCount == 0)
            {
                animator.SetBool("PlayerAttackAnimation1", true);
                Debug.Log("攻撃1回目！");
            }
            else if (AttackComboCount == 1)
            {
                animator.SetBool("PlayerAttackAnimation2", true);
                Debug.Log("攻撃2回目！");
            }
            else if (AttackComboCount == 2)
            {
                animator.SetBool("PlayerAttackAnimation3", true);
                Debug.Log("攻撃3回目！");
            }

            // Timerを開始
            TimerController.Instance.IsStartTimer = true;
        }
        // 1秒以内に次の攻撃の入力を受け取ったら
        if(TimerController.Instance.ElapsedTime < 1.0f || AttackComboCount < 2)
        {
            // Timerのストップとリセット
            AttackTimerReset();

            // コンボのカウントを進める
            AttackComboCount++;
        }
        // タイムが2秒以上もしくはAttackカウントは3以上になれば
        if (TimerController.Instance.ElapsedTime >= 1.0f || AttackComboCount >= 3)
        {
            // Timerのストップとリセット
            AttackTimerReset();

            // AttackComboCountを0に戻す
            AttackComboCount = 0;

            // アタックアニメーションをfalseにする
            animator.SetBool("PlayerAttackAnimation1", false);

            animator.SetBool("PlayerAttackAnimation2", false);

            animator.SetBool("PlayerAttackAnimation3", false);
        }

        // ボタン離したら次のクリックを許可する
        if (attack == 0)
        {
            isClick = true;
        }
    }

    /// <summary>
    /// Timerのストップとリセット
    /// </summary>
    private void AttackTimerReset()
    {
        // タイマーを止める
        TimerController.Instance.IsStartTimer = false;

        // タイマーのリセット
        TimerController.Instance.IsResetTimer = true;
    }
}
