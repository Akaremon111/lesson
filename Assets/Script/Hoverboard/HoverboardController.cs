using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class HoverboardController : MonoBehaviour
{
    /// <summary>
    /// Rigidbodyのコンポーネント
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// BoxCastの原点
    /// </summary>
    private Vector3 origin;

    /// <summary>
    /// Rayを出している原点から当たったところまでの距離
    /// </summary>
    private float RayDistance;

    /// <summary>
    /// BoxCastのサイズ
    /// </summary>
    private Vector3 BoxCastSize;

    /// <summary>
    /// BoxCastの最大距離
    /// </summary>
    private float MaxDistance = 0.5f;

    /// <summary>
    /// Rayが当たった時のオブジェクトの情報格納
    /// </summary>
    RaycastHit hit;

    /// <summary>
    /// Idle時の動きを行う
    /// </summary>
    private bool isIdle = true;

    /// <summary>
    /// Idle時のIdleの動きをスタートさせるポジション
    /// </summary>
    private Vector3 StartPos;

    /// <summary>
    /// 2.0fにπをかけてちょうど1秒で1往復のサイクルを作る
    /// </summary>
    private const float TwoPi = 2.0f * Mathf.PI;

    /// <summary>
    /// 上下運動する周期(1往復にかかる時間)
    /// </summary>
    private float IdlePeriod = 2.0f;

    /// <summary>
    /// 上下運動する周波数(〇秒間に何往復するか)
    /// </summary>
    private float IdleFrequency;

    /// <summary>
    /// 移動量の計算を格納
    /// </summary>
    private float IdleSinPos;

    /// <summary>
    /// 上下運動の振れ幅
    /// </summary>
    private float IdleAmplitude = 0.5f;

    /// <summary>
    /// ホバーボードを動かす
    /// </summary>
    public bool isBoardMove { get; set; } = false;

    private void Awake()
    {
        // Rigidbodyのコンポーネント取得
        rb = GetComponent<Rigidbody>();

        StartPos = transform.position;
    }

    private void Update()
    {
        // Idle時のホバーボードの動き
        BoardIdle();

        // ホバーボードの移動
        BoardMove();
    }

    private void BoardIdle()
    {
        origin = transform.position;
        if (Physics.Raycast(origin, Vector3.down, out hit))
        {
            RayDistance = hit.distance;
        }

        // isIdleがtrueになったとき
        if (isIdle)
        {
            // 1秒間に何往復するかを求める
            IdleFrequency = 1 / IdlePeriod;

            // 上下運動の計算
            IdleSinPos = Mathf.Sin(TwoPi * IdleFrequency * Time.time) * IdleAmplitude;

            // 上下運動の移動処理
            transform.position = StartPos + new Vector3(0.0f, IdleSinPos, 0.0f);
        }
    }

    private void BoardMove()
    {
        // isBoardMoveがtrueになったとき(Playerがホバーボードに乗った時)
        if(isBoardMove)
        {
         // BoxCastの原点
        origin = transform.position;

        // BoxCastのサイズをオブジェクトと同じ大きさにする
        BoxCastSize = transform.localScale * 0.5f;

            // BoxCastを飛ばす
            if (Physics.BoxCast(origin, BoxCastSize, new Vector3(0, -0.2f, 0), out hit, Quaternion.identity, MaxDistance))
            {

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, -0.2f, 0), transform.localScale);

        Gizmos.DrawRay(transform.position, Vector3.down);
    }
}
