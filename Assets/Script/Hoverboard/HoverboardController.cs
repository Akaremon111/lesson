using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverboardController : MonoBehaviour
{
    /// <summary>
    /// BoxCast�̌��_
    /// </summary>
    private Vector3 origin;

    private void Awake()
    {
        
    }

    private void Update()
    {
        BoardIdle();
    }

    private void BoardIdle()
    {
        // BoxCast�̌��_
        origin = transform.position;


    }
}
