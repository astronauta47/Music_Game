using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoElement : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position -= new Vector3(0, speed * 60* Time.deltaTime, 0);
    }
}
