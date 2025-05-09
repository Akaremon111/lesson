using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playerの状態
/// </summary>
public enum PlayerState
{
    /// <summary>
    /// 何もしていない状態
    /// </summary>
    None,

    /// <summary>
    /// ジャンプ中
    /// </summary>
    Jumping,

    /// <summary>
    /// 攻撃中
    /// </summary>
    Attacking,

    /// <summary>
    /// ホバーボードに乗っているとき
    /// </summary>
    OnBoard,
}

public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// PlayerState
    /// </summary>
    public PlayerState state { get; set; }


    /* HP,MPなど実装予定 */


    private void Awake()
    {
        // 初期は何もしていない状態にする
        state = PlayerState.None;
    }

    private void Update()
    {
        Debug.Log(state);
    }
}
