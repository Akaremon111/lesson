using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDate : MonoBehaviour
{
    public float PlayerSpeed;
    // Start is called before the first frame update
    void Start()
    {
        PlayerSpeed = PlayerPrefs.GetFloat("Speed", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
