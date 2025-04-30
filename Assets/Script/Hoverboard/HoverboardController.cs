using System.Collections;
using System.Collections.Generic;
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
    /// BoxCastのサイズ
    /// </summary>
    private Vector3 BoxCastSize;

    /// <summary>
    /// BoxCastの最大距離
    /// </summary>
    private float MaxDistance = 0.1f;
    /// <summary>
    /// Rayに当たったオブジェクトの情報を格納
    /// </summary>
    private RaycastHit hit;

    private void Awake()
    {
        // Rigidbodyのコンポーネント取得
        rb = GetComponent<Rigidbody>();

        // BoxCastのサイズをオブジェクトと同じ大きさにする
        BoxCastSize = Vector3.one;
    }

    private void Update()
    {
        BoardIdle();
    }

    private void BoardIdle()
    {
        // BoxCastの原点
        origin = transform.position + new Vector3(0.0f, -0.5f, 0.0f);

        // BoxCastを飛ばす
        if(Physics.BoxCast(origin,BoxCastSize, Vector3.down,out hit,Quaternion.identity, MaxDistance))
        {
            //Vector3 force = new Vector3(0.0f, 10.0f, 0.0f);
            //rb.AddForce(force);
            Debug.Log("当たっています。");
        }
    }

    private void OnDrawGizmos()
    {
        // BoxCastの原点と方向
        Vector3 originGizmo = transform.position + new Vector3(0.0f, -0.5f, 0.0f);
        Vector3 direction = Vector3.down;

        // BoxCastの半サイズ（BoxCastは半サイズ指定）
        Vector3 halfExtents = Vector3.one * 0.5f; // または BoxCastSize が正しければそれ

        // BoxCastが進む範囲の中心位置（視覚化用）
        Vector3 center = originGizmo + direction * (MaxDistance * 0.5f);

        // 回転（このコードでは回転してないから identity）
        Quaternion orientation = Quaternion.identity;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, halfExtents * 2);  // full size で描画
    }


}
