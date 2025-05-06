using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// HoverboardControllerをシリアライズ
    /// </summary>
    [SerializeField]
    private HoverboardController HBcontroller;
   

    /// <summary>
    /// Eキーの判定
    /// </summary>
    private bool EKey;

    /// <summary>
    /// ホバーボードに乗る
    /// </summary>
    private bool isOnHoverboard;

    /// <summary>
    /// Rayの原点
    /// </summary>
    private Vector3 origin;

    /// <summary>
    /// Rayの最大距離
    /// </summary>
    private float MaxDistance = 10.0f;

    /// <summary>
    /// Rayにヒットしたオブジェクトの情報
    /// </summary>
    RaycastHit hit;

    private void Awake()
    {
        
    }

    private void Update()
    {
        // キー入力の判定
        ActionInput();

        // カメラで見たオブジェクトに応じた処理を行う
        LookObject();
    }

    private void ActionInput()
    {
        // Eキーの入力判定
        EKey = InputManager.Instance.ActionE;

        if(EKey)
        {
            HBcontroller.IsBoardMove = true;
        }
    }

    /// <summary>
    /// カメラで見た(Rayに当たった)オブジェクトに応じた処理を行う
    /// </summary>
    private void LookObject()
    {
        // 現在地の更新
        origin = transform.position;

        if (Physics.Raycast(origin, transform.forward, out hit, MaxDistance))
        {

            if (hit.collider.CompareTag("Hoverboard"))
            {
                Debug.Log("カメラで見ていますよ");
            }
        }
        Vector3 direction = transform.forward * MaxDistance;
        Debug.DrawRay(origin, direction, Color.red);

    }
}
