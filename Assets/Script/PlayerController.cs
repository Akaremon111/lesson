using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private int pAnimation = 0;
    private int setAnimation;

    // Playerの移動スピード
    private float PlayerSpeed;

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

    // Start is called before the first frame update
    void Start()
    {
        // Playerのアニメーション
        animator = GetComponent<Animator>();

        PlayerTransform = transform;
        // 
        previousPos = PlayerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        // Playerの動き
        PlayerMove();

        // Playerの角度
        PlayerAngle();

        KeyInput();
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

    void PlayerAnimation()
    {
        // 
        if((wKey || aKey || sKey || dKey )&& !shiftKey)
        {
            pAnimation = 1;
            Debug.Log(pAnimation);
        }
        if(!(wKey || aKey || sKey || dKey))
        {
            pAnimation = 0;
            Debug.Log(pAnimation);
        }
        if ((wKey || aKey || sKey || dKey) && shiftKey)
        {
            pAnimation = 2;
            Debug.Log(pAnimation);
        }
        if(pAnimation != setAnimation)
        {
            setAnimation = pAnimation;
            animator.SetInteger("PlayerAnimation", setAnimation);
        }
    }


    /// <summary>
    /// キー入力の検知
    /// </summary>
    void KeyInput()
    {
        // Wキーの入力検知
        wKey = Input.GetKey(KeyCode.W) ? true : false;
        // Aキーの入力検知
        aKey = Input.GetKey(KeyCode.A) ? true : false;
        // Sキーの入力検知
        sKey = Input.GetKey(KeyCode.S) ? true : false;
        // Dキーの入力検知
        dKey = Input.GetKey(KeyCode.D) ? true : false;

        // Shiftキーの入力検知
        shiftKey = Input.GetKey(KeyCode.LeftShift) ? true : false;

        // キー入力でアニメーションを動かす
        PlayerAnimation();
    }

    void JoyStickInput()
    {

    }
}
