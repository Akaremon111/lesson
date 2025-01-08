using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private Vector3 playerPos;

    float CameraSpeed = 200.0f;
    float mouseInputX;
    float mouseInputY;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Player.transform.position - playerPos;
        playerPos = Player.transform.position;

        // �}�E�X�̓��͌��m
        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");

        // Playr�𒆐S�Ƃ����J�����̉�]
        transform.RotateAround(playerPos, Vector3.up, mouseInputX * Time.deltaTime * CameraSpeed);
        transform.RotateAround(playerPos, -transform.right, mouseInputY * Time.deltaTime * CameraSpeed);
    }
}
