using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // InputManagerのインスタンス
    public static InputManager Instance { get; private set; }

    /// <summary>
    /// プレイヤーインプット
    /// </summary>
    private PlayerInput playerInput;

    /// <summary>
    /// 移動アクション
    /// </summary>
    private InputAction moveAction;
    /// <summary>
    /// 視点アクション
    /// </summary>
    private InputAction lookAction;
    /// <summary>
    /// ダッシュアクション
    /// </summary>
    private InputAction sprintAction;
    /// <summary>
    /// ジャンプアクション
    /// </summary>
    private InputAction jumpAction;
    /// <summary>
    /// 攻撃アクション
    /// </summary>
    private InputAction attackAction;

    /// <summary>
    /// 歩きの入力
    /// </summary>
    public Vector2 Move { get; private set; }

    /// <summary>
    /// 視点の入力
    /// </summary>
    public Vector2 Look { get; private set; }

    /// <summary>
    /// ダッシュの入力
    /// </summary>
    public float Sprint { get; private set; }

    /// <summary>
    /// ジャンプの入力
    /// </summary>
    public float Jump { get; private set; }

    /// <summary>
    /// 攻撃の入力
    /// </summary>
    public float Attack {  get; private set; }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        // インスタンスがあるもしくはこれがインスタンスではない場合
        if (Instance != null && Instance != this)
        {
            // GameObjの削除
            Destroy(gameObject);
            return;
        }
        // これをインスタンスとする
        Instance = this;
        // シーンをまたいでも維持
        DontDestroyOnLoad(gameObject);

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        InputUpdate();
    }

    /// <summary>
    /// 入力の更新
    /// </summary>
    private void InputUpdate()
    {
        Move = moveAction.ReadValue<Vector2>();
        Look = lookAction.ReadValue<Vector2>();
        Sprint = sprintAction.ReadValue<float>();
        Jump = jumpAction.ReadValue<float>();
        Attack = attackAction.ReadValue<float>();
    }
}
