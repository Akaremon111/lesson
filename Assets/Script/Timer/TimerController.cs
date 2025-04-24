using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// タイマークラス
/// </summary>
public class TimerController : MonoBehaviour
{
    // TimerControllerのインスタンス
    public static TimerController Instance { get; private set; }

    /// <summary>
    /// 経過時間
    /// </summary>
    public float ElapsedTime{ get; private set; } = 0;

    /// <summary>
    /// タイマー開始
    /// </summary>
    public bool IsStartTimer { get; set; } = false;

    /// <summary>
    /// タイマーをリセット
    /// </summary>
    public bool IsResetTimer { get; set; } = false;

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
    }

    private void Update()
    {
        // Timer開始
        if (IsStartTimer) Timer();

        // タイムをリセットする
        if (IsResetTimer) TimeReset();
    }

    /// <summary>
    /// 時間の計測
    /// isStartTimerがtrueの信号を受けたときにメソッドを読み込む
    /// </summary>
    private void Timer()
    {
        // タイムを計測する
        ElapsedTime += Time.deltaTime;
    }

    /// <summary>
    /// タイムをリセットする
    /// isResetTimerがtrueの信号を受けたときに読み込む
    /// </summary>
    private void TimeReset()
    {
        // 経過時間を0に戻す
        ElapsedTime = 0;
        
        // 一度だけ読み込まれればいのでfalseにする
        IsResetTimer = false;
    }
}
