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
    /// ホバーボードの親オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject Hoverboard;

    /// <summary>
    /// Playerオブジェクトのポジション
    /// </summary>
    private Vector3 PlayerPos;

    /// <summary>
    /// InputManagerから入力判定を受け取る
    /// </summary>
    private bool Ekey;

    /// <summary>
    /// BoxCastの原点
    /// </summary>
    private Vector3 origin;

    /// <summary>
    /// Rayを出している原点から当たったところまでの距離
    /// </summary>
    private float RayDistance;

    /// <summary>
    /// SphereCastAllの当たったオブジェクト情報の配列
    /// </summary>
    RaycastHit[] hits;

    /// <summary>
    /// SphereCastAllの半径
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

    /// <summary>
    /// Playerオブジェクトをホバーボードの子オブジェクトにするフラグ
    /// </summary>
    private bool isChild = false;

    /// <summary>
    /// PlayerManagerの取得
    /// </summary>
    private PlayerManager playerManager;

    private void Awake()
    {
        // PlayerManagerスクリプトの取得
        playerManager = Player.GetComponent<PlayerManager>();

        // ゲーム開始時のポジション
        StartPos = transform.position;
    }

    private void Update()
    {
        // 各Rayで使用するRayの原点
        origin = transform.position;

        // InputManagerから入力の判定を受け取る
        getInput();

        // Idle時のホバーボードの動き
        BoardIdle();

        // ホバーボードに乗ることのできるエリア
        ActiveArea();

        // ホバーボードの移動
        BoardMove();
    }

    /// <summary>
    /// InputManagerから入力の判定を受け取る
    /// </summary>
    private void getInput()
    {
        // Eキーの入力判定
        Ekey = InputManager.Instance.ActionE;
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
    /// ホバーボードに乗ることのできる範囲にSphereCastAllを飛ばす
    /// </summary>
    private void ActiveArea()
    {
        // SphereCastAllの発射
        hits = Physics.SphereCastAll(origin, SphereRadius, Vector3.down, MaxDistance);

        // hitしたオブジェクトの情報
        foreach (RaycastHit hit in hits)
        {
            // 当たったオブジェクトのタグがPlayerのとき
            if(hit.collider.CompareTag("Player"))
            {
                // Eキーが押されたときかつ攻撃中じゃないとき
                if(Ekey && playerManager.state != PlayerState.Attacking)
                {
                    // isChildがfalseなら
                    if(!isChild)
                    {
                        // Playerの状態をホバーボードに乗っている状態にする
                        playerManager.state = PlayerState.OnBoard;

                        // アイドル状態をやめる
                        isIdle = false;

                        // Playerオブジェクトをホバーボードの子オブジェクトにする
                        Player.transform.SetParent(Hoverboard.transform, true);

                        // Playerの位置をホバーボードの上にする
                        Player.transform.position = Hoverboard.transform.position + new Vector3(0.0f, 10.0f, 0.0f);

                        // 子オブジェクトになったのでtrueにする
                        isChild = true;
                    }
                    else
                    {
                        // Playerの状態を何もしていない状態に戻す
                        playerManager.state = PlayerState.None;

                        // アイドル状態にする
                        isIdle = true;

                        // 親子関係を解除する
                        Player.transform.SetParent(null, false);

                        // 親子関係が解除されたのでfalseにする
                        isChild = false;
                    }
                }
            }
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
