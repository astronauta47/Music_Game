using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    float counterColor;

    void Update()
    {
        BackgroundColorChanger();
    }

    void BackgroundColorChanger()
    {
        counterColor += Time.deltaTime;

        if (counterColor > 20) counterColor = 0;

        Camera.main.backgroundColor = Color.HSVToRGB(counterColor / 20, .33f, 1);
    }
}
