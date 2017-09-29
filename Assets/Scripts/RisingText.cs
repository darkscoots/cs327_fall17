﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RisingText : MonoBehaviour {
    [SerializeField]
    Text textRising;
    [SerializeField]
    float risingSpeed;
    [SerializeField]
    float fadingSpeed;

    // Use this for initialization
    void Start () {
        risingSpeed = 100f * Time.deltaTime;
        fadingSpeed = -1f * Time.deltaTime;
    }

    // Update is called once per frame
    void Update () {
        float x = textRising.transform.localPosition.x;
        float y = textRising.transform.localPosition.y + risingSpeed;
        float z = textRising.transform.localPosition.z;
        
        textRising.transform.localPosition = new Vector3(x, y, z);

        float r = textRising.color.r;
        float g = textRising.color.g;
        float b = textRising.color.b;
        float a = textRising.color.a + fadingSpeed;
        
        textRising.color = new Color(r, g, b, a);

        if (a < 0f)
        {
            Destroy(gameObject);
        }
        
    }

    void SetText(string str)
    {
        textRising.text = str;
    }
}
