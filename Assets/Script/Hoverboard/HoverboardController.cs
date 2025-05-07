using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class HoverboardController : MonoBehaviour
{
    /// <summary>
    /// Playerのオブジェクト取得
    /// </summary>
    [SerializeField]
    private GameObject Player;

    /// <summary>
    /// ホバーボードのオブジェクト取得
    /// </summary>
    [SerializeField]
    private GameObject Hoverboard;

    /// <summary>
    /// Playerオブジェクトのポジション
    /// </summary>
    private Vector3 PlayerPos;

    /// <summary>
    /// ホバーボードオブジェクトのポジション
    /// </summary>
    private Vector3 HBpos;

    /// <summary>
    /// BoxCastの原点
    /// </summary>
    private Vector3 origin;

    /// <summary>
    /// Rayを出している原点から当たったところまでの距離
    /// </summary>
    private float RayDistance;

    /// <summary>
    /// SphereCastの半径
    /// </summary>
    [SerializeField]
    private float SphereRadius;

    /// <summary>
    /// BoxCastのサイズ
    /// </summary>
    private Vector3 BoxCastSize;

    /// <summary>
    /// Rayの最大距離
    /// </summary>
    private float MaxDistance = 0.1f;

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
    public bool IsBoardMove { get; set; } = false;

    private void Awake()
    {
        StartPos = transform.position;
    }

    private void Update()
    {
        // 各Rayで使用するRayの原点
        origin = transform.position;

        // Idle時のホバーボードの動き
        BoardIdle();

        // ホバーボードに乗ることのできるエリア
        ActiveArea();

        // ホバーボードの移動
        BoardMove();
    }

    /// <summary>
    /// ホバーボードのIdle状態の動き
    /// </summary>
    private void BoardIdle()
    {

        // Rayの発射
        if (Physics.Raycast(origin, Vector3.down, out hit))
        {
            // Rayの原点から当たってる場所までの距離
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

    /// <summary>
    /// ホバーボードに乗ることのできる範囲にSphereCastを飛ばす
    /// </summary>
    private void ActiveArea()
    {
        // SphereCastの発射
        if(Physics.SphereCast(origin, SphereRadius, Vector3.up, out hit, MaxDistance))
        {
            Debug.Log("範囲内に入りました。");
        }
        
    }

    /// <summary>
    /// ホバーボードの動き
    /// </summary>
    private void BoardMove()
    {
        // IsBoardMoveがtrueになったとき(Playerがホバーボードに乗った時)
        if(IsBoardMove)
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

    /// <summary>
    /// Debug用
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireCube(transform.position + new Vector3(0, -0.2f, 0), transform.localScale);

        Gizmos.DrawWireSphere(transform.position + transform.up * MaxDistance, SphereRadius);
    }
}
