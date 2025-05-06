using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// HoverboardController���V���A���C�Y
    /// </summary>
    [SerializeField]
    private HoverboardController HBcontroller;
   

    /// <summary>
    /// E�L�[�̔���
    /// </summary>
    private bool EKey;

    /// <summary>
    /// �z�o�[�{�[�h�ɏ��
    /// </summary>
    private bool isOnHoverboard;

    /// <summary>
    /// Ray�̌��_
    /// </summary>
    private Vector3 origin;

    /// <summary>
    /// Ray�̍ő勗��
    /// </summary>
    private float MaxDistance = 10.0f;

    /// <summary>
    /// Ray�Ƀq�b�g�����I�u�W�F�N�g�̏��
    /// </summary>
    RaycastHit hit;

    private void Awake()
    {
        
    }

    private void Update()
    {
        // �L�[���͂̔���
        ActionInput();

        // �J�����Ō����I�u�W�F�N�g�ɉ������������s��
        LookObject();
    }

    private void ActionInput()
    {
        // E�L�[�̓��͔���
        EKey = InputManager.Instance.ActionE;

        if(EKey)
        {
            HBcontroller.IsBoardMove = true;
        }
    }

    /// <summary>
    /// �J�����Ō���(Ray�ɓ�������)�I�u�W�F�N�g�ɉ������������s��
    /// </summary>
    private void LookObject()
    {
        // ���ݒn�̍X�V
        origin = transform.position;

        if (Physics.Raycast(origin, transform.forward, out hit, MaxDistance))
        {

            if (hit.collider.CompareTag("Hoverboard"))
            {
                Debug.Log("�J�����Ō��Ă��܂���");
            }
        }
        Vector3 direction = transform.forward * MaxDistance;
        Debug.DrawRay(origin, direction, Color.red);

    }
}
