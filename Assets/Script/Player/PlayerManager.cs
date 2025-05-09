using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player�̏��
/// </summary>
public enum PlayerState
{
    /// <summary>
    /// �������Ă��Ȃ����
    /// </summary>
    None,

    /// <summary>
    /// �W�����v��
    /// </summary>
    Jumping,

    /// <summary>
    /// �U����
    /// </summary>
    Attacking,

    /// <summary>
    /// �z�o�[�{�[�h�ɏ���Ă���Ƃ�
    /// </summary>
    OnBoard,
}

public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// PlayerState
    /// </summary>
    public PlayerState state { get; set; }


    /* HP,MP�Ȃǎ����\�� */


    private void Awake()
    {
        // �����͉������Ă��Ȃ���Ԃɂ���
        state = PlayerState.None;
    }

    private void Update()
    {
        Debug.Log(state);
    }
}
