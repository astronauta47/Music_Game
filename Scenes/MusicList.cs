using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicList : MonoBehaviour
{
    string MusicListTxtURL = "https://drive.google.com/uc?export=download&id=1kNcic5BEDJnlpUphWn765AmxCiOWngT7";
    public List<Music> musicList = new List<Music>();

    public static string musicListInString;
    public List<string> stringList = new List<string>();

    [SerializeField] GameObject musicBlock;
    [SerializeField] Transform musicBlockParent;

    int startPosString;

    public static string ActualSelectMusicName;

    public Sprite HardSprite, MediumSprite, EasySprite;

    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        musicListInString = www.text;

        TxtInterpreter();
        LoadMusicList();
        StressedNames();
    }

    public void DownLoadList()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        WWW www = new WWW(MusicListTxtURL);

        StartCoroutine(WaitForRequest(www));
    }

    private void Start()
    {
        if (musicListInString == null)
        {
            WWW www = new WWW(MusicListTxtURL);

            StartCoroutine(WaitForRequest(www));
        }
        else
        {
            Debug.Log("Music");
            TxtInterpreter();
            LoadMusicList();
            StressedNames();
        }


    }

    void StressedNames()
    {
        for (int i = 0; i < musicBlockParent.childCount; i++)
        {
            Transform text = musicBlockParent.GetChild(i).GetChild(0).GetChild(0);

            for (int j = 0; j < SaveLoad.LoadedSongList.Count; j++)
            {
                if (SaveLoad.LoadedSongList[j].name == text.GetComponent<Text>().text)
                {
                    text.GetChild(0).gameObject.SetActive(true);
                }
            }
            
        }
    }

    void TxtInterpreter()
    {

        for (int i = 0; i < musicListInString.Length; i++)
        {
            if(musicListInString[i] == ';')
            {
                Debug.Log(i);
                Debug.Log(startPosString);
                char[] chars = new char[i - startPosString];
                

                for (int j = 0; j < i - startPosString; j++)
                {
                    chars[j] = musicListInString[startPosString + j];
                }

                stringList.Add(new string(chars));
                startPosString = i+2;
            }
        }

        int length = stringList.Count;

        for (int i = 0; i < length / 5; i++)
        {
            musicList.Add(new Music(stringList[0], stringList[1], stringList[2], stringList[3], stringList[4]));

            for (int j = 0; j < 5; j++)
            {
                stringList.Remove(stringList[0]);
                
            }
        }
        
        
    }

    void LoadMusicList()
    {
        for (int i = 0; i < musicList.Count; i++)
        {
            Rect rect = musicBlockParent.GetComponent<RectTransform>().rect;
            musicBlockParent.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height + 177.5f);
            musicBlockParent.GetComponent<RectTransform>().position = new Vector2(musicBlockParent.GetComponent<RectTransform>().position.x, -10000);

            GameObject G = Instantiate(musicBlock, musicBlockParent);

            G.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = musicList[i].name;
            G.transform.GetChild(2).GetComponent<Text>().text = musicList[i].time;


            if (musicList[i].dificultLevel == "Hard")
                G.transform.GetChild(0).GetComponent<Image>().sprite = HardSprite;

            else if (musicList[i].dificultLevel == "Medium")
                G.transform.GetChild(0).GetComponent<Image>().sprite = MediumSprite;

            else if (musicList[i].dificultLevel == "Easy")
                G.transform.GetChild(0).GetComponent<Image>().sprite = EasySprite;
        }
    }



    public void FindSong()
    {
        for (int i = 0; i < musicList.Count; i++)
        {
            if(musicList[i].name == ActualSelectMusicName)
            {
                SaveLoad.ActualLoadMusic = musicList[i];
                break;
            }
        }
    }

    public struct Music
    {
        public string name;
        public string dificultLevel;
        public string time;
        public string musicURL;
        public string rateURL;

        public Music(string Name, string Time, string Level, string M_URL, string R_URL)
        {
            name = Name;
            dificultLevel = Level;
            time = Time;
            musicURL = M_URL;
            rateURL = R_URL;
        }
    }

}
