using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] float speed;

    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * -speed);
    }
}
