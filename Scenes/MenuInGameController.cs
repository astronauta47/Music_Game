using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInGameController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject PauseUICanvas;

    public void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            audioSource.Pause();
            Time.timeScale = 0;
        }
        else
        {
            audioSource.Play();
            Time.timeScale = 1;
        }

        PauseUICanvas.SetActive(!PauseUICanvas.activeInHierarchy);
    }

    public void BackToMenuInGame()
    {
        Time.timeScale = 1;
        PathFollower.Isstart = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        MainMenu.LoadMenu();
    }
}
