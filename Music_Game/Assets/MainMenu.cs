using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }

    public void LoadGame(int SceneNumber)
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().Play("PlayAnim");

        if (MusicList.ActualSelectMusicName != null)
        {   
            StartCoroutine(Wait(SceneNumber));
        }
        
    }

    IEnumerator Wait(int sceneNumber)
    {

        yield return new WaitForSeconds(0.4f);

        GetComponent<MusicList>().FindSong();
        SceneManager.LoadScene(sceneNumber);
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

}
