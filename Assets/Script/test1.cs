using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    private float hoge;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.A)) return;
        inputKey();
    }

    private void inputKey()
    {
        Debug.Log("AƒL[‚ª‰Ÿ‚³‚ê‚Ü‚µ‚½B");
    }
}
