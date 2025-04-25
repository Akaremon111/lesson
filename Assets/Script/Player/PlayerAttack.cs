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

    // コンボが繋がる猶予時間
    private float comboInterval = 1.0f;

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

        // ComboTimerのカウントダウン
        if (ComboTimer > 0)
        {
            ComboTimer -= Time.deltaTime;
        }

        // タイムが切れたらコンボリセット
        if (ComboTimer <= 0 && AttackComboCount > 0)
        {
            AttackTimerReset();
            AttackComboCount = 0;

            // アニメーション状態もリセット
            animator.SetBool("PlayerAttackAnimation1", false);
            animator.SetBool("PlayerAttackAnimation2", false);
            animator.SetBool("PlayerAttackAnimation3", false);
        }
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

            // もう一度押さないとだめにする
            isClick = false;

            // コンボのカウントを進める
            AttackComboCount++;

            // Timerをリスタート
            ComboTimer = comboInterval;

            // 攻撃アニメーション
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

            // Timerを開始（外部タイマーが必要ならここで）
            TimerController.Instance.IsStartTimer = true;
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
