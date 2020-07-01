using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongBlock : MonoBehaviour
{
    static Image LastSelectedImage;

    public void SelectSong()
    {
        if(LastSelectedImage != null) LastSelectedImage.color = Color.white;
        LastSelectedImage = transform.GetChild(0).GetComponent<Image>();

        LastSelectedImage.color = new Color(0.9656668f, 1, 0.514151f);
        

        MusicList.ActualSelectMusicName = transform.GetChild(0).GetChild(0).GetComponent<Text>().text;
    }
}
