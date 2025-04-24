using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// Playerの移動速度
    /// </summary>
    [SerializeField]
    private float moveSpeed = 2.0f;

    /// <summary>
    /// カメラの向いている方向
    /// </summary>
    Vector3 cameraForward;

    /// <summary>
    /// Playerの動く方向
    /// </summary>
    Vector3 moveForward;

    /// <summary>
    /// 現在のポジション
    /// </summary>
    private Transform PlayerTransform;

    /// <summary>
    /// １フレーム前のポジション
    /// </summary>
    private Vector3 previousPos;
    private float crrentVerocity;

    /// <summary>
    /// 回転時間
    /// </summary>
    [SerializeField]
    private float smoothTime;

    /// <summary>
    /// 回転する最大のスピード
    /// </summary>
    [SerializeField]
    private float maxSpeed;

    /// <summary>
    /// InputManagerから受け取る
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
    /// 移動のAnimation
    /// </summary>
    private int pMoveAnimation = 0;
    private int getMoveAnimation;

    private void Awake()
    {
        // アニメーションコンポーネント取得
        animator = GetComponent<Animator>();

        // transformの取得
        PlayerTransform = transform;

        // Playerの位置の保存
        previousPos = PlayerTransform.position;
    }

    private void Update()
    {
        getInput();

        // 移動方向の処理
        moveDirection();

        // Playerの角度の計算
        moveAngle();

        // アニメーションを動かす
        moveAnimation();
    }

    private void FixedUpdate()
    {
        // 移動の処理
        Move();
    }

    /// <summary>
    /// Playerの移動の処理
    /// 移動処理のためFixedUpdateからの呼び出し
    /// </summary>
    private void Move()
    {
        // カメラの方向を前として移動を行う
        transform.position += moveForward * Time.deltaTime * moveSpeed;
    }

    /// <summary>
    /// InputManagerからの受け取り
    /// </summary>
    private void getInput()
    {
        // Moveを受け取る
        move = InputManager.Instance.Move;

        // Sprintを受け取る
        sprint = InputManager.Instance.Sprint;
    }

    /// <summary>
    /// Playerの移動処理
    /// </summary>
    private void moveDirection()
    {
        // カメラの前方向の取得
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 取得したカメラの方向をPlayerの前方向にする
        moveForward = cameraForward * move.y + Camera.main.transform.right * move.x;

        // Playerスピードの変更
        moveSpeed = sprint > 0 ? 5 : 2;
    }

    /// <summary>
    /// Playerの移動方向の角度計算
    /// </summary>
    private void moveAngle()
    {
        // 現在の位置
        Vector3 nowPos = PlayerTransform.position;

        // 移動量の計算
        Vector3 delta = nowPos - previousPos;

        // 前の位置の更新
        previousPos = nowPos;

        // 静止状態の時は回転しない
        if (delta == Vector3.zero)
        {
            return;
        }

        // 回転方向
        Quaternion targetRot = Quaternion.LookRotation(delta, Vector3.up);

        // 現在の方向と進行方向との角度を計算
        float diffAngle = Vector3.Angle(PlayerTransform.forward, delta);

        // 回転時間とスピードの計算
        float rotAngle = Mathf.SmoothDampAngle(0, diffAngle, ref crrentVerocity, smoothTime, maxSpeed);

        // 現在の角度からtargetRotに向かって回転
        Quaternion nextRot = Quaternion.RotateTowards(PlayerTransform.rotation, targetRot, rotAngle);
        PlayerTransform.rotation = nextRot;
    }

    /// <summary>
    /// Playerのアニメーション
    /// </summary>
    private void moveAnimation()
    {
        // 移動キーが押されている時
        if (move.magnitude > 0)
        {
            // 歩く時のアニメーション
            pMoveAnimation = 1;

            // Shiftキーが押されているとき
            if (sprint > 0)
            {
                // 走っている時の処理
                pMoveAnimation = 2;
            }
        }
        else
        {
            // アイドル時のアニメーション
            pMoveAnimation = 0;
        }
        // 値が合わないときに読み込む
        if (pMoveAnimation != getMoveAnimation)
        {
            // 値を同じにする
            getMoveAnimation = pMoveAnimation;

            // PlayerMoveAnimationの値をgetComboAnimationにする
            animator.SetInteger("PlayerMoveAnimation", getMoveAnimation);
        }
    }
}