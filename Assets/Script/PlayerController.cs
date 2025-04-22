using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Playerの移動スピード
    private float PlayerSpeed;

    // 移動キーの入力
    private float InputX;
    private float InputZ;

    /// <summary>
    /// PlayerMoveで使用
    /// </summary>

    //カメラの向いている方向
    Vector3 cameraForward;
    // Playerの動く方向
    Vector3 moveForward;


    /// <summary>
    /// PlayerAngleで使用
    /// </summary>

    // 現在のポジション
    private Transform PlayerTransform;
    // １フレーム前のポジション
    private Vector3 previousPos;

    float crrentVerocity;

    // 回転時間
    [SerializeField]
    private float smoothTime;
    // 回転する最大のスピード
    [SerializeField]
    private float maxSpeed;


    /// <summary>
    /// キーボードの入力検知
    /// </summary>
    // Wキーの入力
    private bool wKey;
    // Aキーの入力
    private bool aKey;
    // Sキーの入力
    private bool sKey;
    // Dキーの入力
    private bool dKey;
    // shiftキーの入力
    private bool shiftKey;

    /// <summary>
    /// マウスの入力検知
    /// </summary>
    // 左クリックの入力検知
    private bool leftButton;

    /// <summary>
    /// Animation
    /// </summary>
    private Animator animator;

    // 移動のAnimation
    private int pMoveAnimation = 0;
    private int getMoveAnimation;

    private void Awake()
    {
        // Playerのアニメーション
        animator = GetComponent<Animator>();

        PlayerTransform = transform;
        // 
        previousPos = PlayerTransform.position;
    }

    private void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        // Playerの動き
        PlayerMove();

        // Playerの角度
        PlayerAngle();

        // 入力検知
        KeyInput();

        // タイマー関数

    }


    /// <summary>
    /// Playerの動き
    /// </summary>
    private void PlayerMove()
    {
        // カメラの前方向の取得
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        // 取得したカメラの方向をPlayerの前方向にする
        moveForward = cameraForward * InputZ + Camera.main.transform.right * InputX;

        // Playerスピードの変更
        PlayerSpeed = shiftKey ? 5 : 2;
    }

    private void FixedUpdate()
    {
        // カメラの方向を前として移動を行う
        transform.position += moveForward * Time.deltaTime * PlayerSpeed;
    }

    /// <summary>
    /// Playerの移動方向の角度計算
    /// </summary>
    private void PlayerAngle()
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
    void PlayerAnimation()
    {
        // 移動キーが押されている時
        if((wKey || aKey || sKey || dKey )&& !shiftKey)
        {
            // 歩く時のアニメーション
            pMoveAnimation = 1;
            Debug.Log(pMoveAnimation);
        }
        if(!(wKey || aKey || sKey || dKey))
        {
            // アイドル時のアニメーション
            pMoveAnimation = 0;
            Debug.Log(pMoveAnimation);
        }
        if ((wKey || aKey || sKey || dKey) && shiftKey)
        {
            // 走っている時の処理
            pMoveAnimation = 2;
            Debug.Log(pMoveAnimation);
        }
        // 値が合わないときに読み込む
        if(pMoveAnimation != getMoveAnimation)
        {
            // 値を同じにする
            getMoveAnimation = pMoveAnimation;

            // PlayerMoveAnimationの値をgetComboAnimationにする
            animator.SetInteger("PlayerMoveAnimation", getMoveAnimation);
        }

        // 攻撃のアニメーション
        if (leftButton)
        {
            animator.SetTrigger("PlayerFastAttackAnimation");
            animator.SetTrigger("PlayerSecondAttackAnimation");
            animator.SetTrigger("PlayerThirdAttackAnimation");
        }
    }

    /// <summary>
    /// 入力の検知
    /// </summary>
    private void KeyInput()
    {
        // Wキーの入力検知
        wKey = Input.GetKey(KeyCode.W) ? true : false;
        // Aキーの入力検知
        aKey = Input.GetKey(KeyCode.A) ? true : false;
        // Sキーの入力検知
        sKey = Input.GetKey(KeyCode.S) ? true : false;
        // Dキーの入力検知
        dKey = Input.GetKey(KeyCode.D) ? true : false;

        // 左クリックの入力検知
        leftButton = Input.GetMouseButtonDown(0) ? true : false;

        // Shiftキーの入力検知
        shiftKey = Input.GetKey(KeyCode.LeftShift) ? true : false;

        // キー入力でアニメーションを動かす
        PlayerAnimation();
    }

    void JoyStickInput()
    {

    }
}
