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
    [Header("移動設定")]
    /// <summary>
    /// Playerの移動速度
    /// </summary>
    [SerializeField]
    private float moveSpeed = 2.0f;

    /// <summary>
    /// ジャンプの高さ
    /// </summary>
    [SerializeField]
    private float JumpHight;

    /// <summary>
    /// ジャンプの最大の高さ
    /// </summary>
    [SerializeField]
    private float MaxJump = 3;

    /// <summary>
    /// Playerにかける重力
    /// </summary>
    [SerializeField]
    private float Gravity;

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

    [Header("その他設定")]
    // SphereCastの半径
    [SerializeField]
    private float sphereRadius;
    /// <summary>
    /// SphereCastの最大の距離
    /// </summary>
    [SerializeField]
    private float maxDistance;

    // SphereCastの原点
    private Vector3 origin;

    /// <summary>
    /// 現在地面にいるのかどうかの判定
    /// </summary>
    private bool isGround = true;

    /// <summary>
    /// カメラの向いている方向
    /// </summary>
    Vector3 cameraForward;

    /// <summary>
    /// Playerの動く方向
    /// </summary>
    Vector3 moveForward;

    /// <summary>
    /// Transgorm
    /// </summary>
    private Transform PlayerTransform;

    /// <summary>
    /// 現在のポジション
    /// </summary>
    Vector3 nowPos;

    /// <summary>
    /// １フレーム前のポジション
    /// </summary>
    private Vector3 previousPos;
    private float crrentVerocity;

    /// <summary>
    /// 垂直方向の速度
    /// </summary>
    private Vector3 Velocity;

    /// <summary>
    /// InputManagerから受け取る
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
    /// 移動のAnimation
    /// </summary>
    private int pMoveAnimation = 0;
    private int getMoveAnimation;

    /// <summary>
    /// 
    /// </summary>
    private Vector3 velocity;

    private void Awake()
    {
        // アニメーションコンポーネント取得
        animator = GetComponent<Animator>();

        // キャラクターコントローラーのコンポーネント取得
        characterController = GetComponent<CharacterController>();

        Velocity = Vector3.zero;

        // transformの取得
        PlayerTransform = transform;

        // Playerの位置の保存
        previousPos = PlayerTransform.position;
    }

    private void Update()
    {
        // 入力を受け取る
        getInput();

        // 移動方向の処理
        moveDirection();

        // Playerの角度の計算
        moveAngle();

        // アニメーションを動かす
        moveAnimation();

        // ジャンプ
        PlayerJump();
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

        // Jumpを受け取る
        jump = InputManager.Instance.Jump;
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
    /// Playerのジャンプの処理を行う
    /// </summary>
    private void PlayerJump()
    {
        // isGroundをfalseにする
        isGround = false;

        // Rayの原点
        origin = transform.position + new Vector3(0.0f, 0.3f, 0.0f);

        // Rayが当たったオブジェクトの情報格納
        RaycastHit hit;

        // Ray（SphereCast）を出す
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
            // 地面にいるときだけジャンプできる
            if (jump > 0f)
            {
                Velocity.y = Mathf.Sqrt(JumpHight * -1f * Physics.gravity.y); // ジャンプ初速度を計算
            }
        }

        if (!isGround)
        {
            // 常に重力を適用

        }
        Velocity.y += Physics.gravity.y * Time.deltaTime;

        characterController.Move(new Vector3(0, Velocity.y, 0) * Time.deltaTime);

        Debug.Log("地面にいるでしょうか:" + isGround);
    }

    /// <summary>
    /// SphereCastの可視化
    /// </summary>
    private void OnDrawGizmos()
    {
        // カラーは青
        Gizmos.color = Color.blue;

        // Rayの位置と可視化させる場所を一致させる
        Gizmos.DrawWireSphere(origin, sphereRadius);
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
        nowPos = PlayerTransform.position;

        // 移動量の計算
        Vector3 Alldelta = nowPos - previousPos;

        // 必要な軸の情報だけを取り出す（Jumpなどに影響しないようにする）
        Vector3 delta = new Vector3(Alldelta.x, 0.0f, Alldelta.z);

        // 前の位置の更新
        previousPos = nowPos;

        // 静止状態の時は回転しない
        if (delta == Vector3.zero) return;

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