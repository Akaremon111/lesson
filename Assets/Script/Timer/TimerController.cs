using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// �^�C�}�[�N���X
/// </summary>
public class TimerController : MonoBehaviour
{
    // TimerController�̃C���X�^���X
    public static TimerController Instance { get; private set; }

    /// <summary>
    /// �o�ߎ���
    /// </summary>
    public float ElapsedTime{ get; private set; } = 0;

    /// <summary>
    /// �^�C�}�[�J�n
    /// </summary>
    public bool IsStartTimer { get; set; } = false;

    /// <summary>
    /// �^�C�}�[�����Z�b�g
    /// </summary>
    public bool IsResetTimer { get; set; } = false;

    private void Awake()
    {
        // �C���X�^���X������������͂��ꂪ�C���X�^���X�ł͂Ȃ��ꍇ
        if (Instance != null && Instance != this)
        {
            // GameObj�̍폜
            Destroy(gameObject);
            return;
        }
        // ������C���X�^���X�Ƃ���
        Instance = this;
        // �V�[�����܂����ł��ێ�
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Timer�J�n
        if (IsStartTimer) Timer();

        // �^�C�������Z�b�g����
        if (IsResetTimer) TimeReset();
    }

    /// <summary>
    /// ���Ԃ̌v��
    /// isStartTimer��true�̐M�����󂯂��Ƃ��Ƀ��\�b�h��ǂݍ���
    /// </summary>
    private void Timer()
    {
        // �^�C�����v������
        ElapsedTime += Time.deltaTime;
    }

    /// <summary>
    /// �^�C�������Z�b�g����
    /// isResetTimer��true�̐M�����󂯂��Ƃ��ɓǂݍ���
    /// </summary>
    private void TimeReset()
    {
        // �o�ߎ��Ԃ�0�ɖ߂�
        ElapsedTime = 0;
        
        // ��x�����ǂݍ��܂��΂��̂�false�ɂ���
        IsResetTimer = false;
    }
}
