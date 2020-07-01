using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver2D : MonoBehaviour
{
    public string tagTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == tagTarget)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
