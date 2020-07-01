using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Xml.Serialization;
using UnityEngine.UI;
using System.Runtime.Serialization;
using System;

public class SaveLoad : MonoBehaviour
{

    public static MusicList.Music ActualLoadMusic;

    AudioClip ActualClip;
    protected string songName;

    public static List<LoadedAudio> LoadedSongList = new List<LoadedAudio>();
    static List<Queue<float>> LoadedRateList = new List<Queue<float>>();

    public Queue<float> TimePoints = new Queue<float>();

    public delegate void OnLoadedFiles();
    public event OnLoadedFiles OnLoadedGame;

    protected WWW w;

    public struct LoadedAudio
    {
        public string name;
        public AudioClip audioClip;

        public LoadedAudio(string Name, AudioClip audio)
        {
            name = Name;
            audioClip = audio;
        }
    }

    protected virtual void OnLoaded()
    {
        OnLoadedGame();
    }

    public struct Clip
    {
        public AudioClip audioClip;
        public int index;
    }

    bool IsLoaded()
    {
        for (int i = 0; i < LoadedSongList.Count; i++)
        {
            if (LoadedSongList[i].name == ActualLoadMusic.name)
            {

                ActualClip = LoadedSongList[i].audioClip;

                TimePoints.Clear();
                for (int j = 0; j < LoadedRateList[i].Count; j++)
                {
                    TimePoints.Enqueue(LoadedRateList[i].Peek());
                    LoadedRateList[i].Enqueue(LoadedRateList[i].Peek());
                    LoadedRateList[i].Dequeue();
                }
                
                return true;
            }
        }

        return false;
    }

    public void LoadGame(ref AudioSource audioSource)
    {
        

        if (IsLoaded())
        {
            songName = ActualLoadMusic.name;
            audioSource.clip = ActualClip;
            OnLoaded();
            return;
        }


        WWW www = new WWW(ActualLoadMusic.rateURL);

        StartCoroutine(WaitForRequest(www));

        w = new WWW(ActualLoadMusic.musicURL);//("file://" + FindPath() + PlayerPrefs.GetString("MusicPath") + ".wav");

        StartCoroutine(WaitForRequestplay(w, audioSource));
    }

    private IEnumerator WaitForRequestplay(WWW www, AudioSource audioSource)
    {
        
        yield return www;
        audioSource.clip = www.GetAudioClip(false, false, AudioType.WAV);
        LoadedSongList.Add(new LoadedAudio(ActualLoadMusic.name, audioSource.clip));
        songName = ActualLoadMusic.name;
        OnLoaded();

        yield return songName;
    }

    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;


        string path = Path.Combine(Application.persistentDataPath, "file.json");

        FileStream fileStream = new FileStream(path, FileMode.Create);


        for (int i = 0; i < www.bytes.Length; i++)
        {
            fileStream.WriteByte(www.bytes[i]);
        }

        fileStream.Seek(0, SeekOrigin.Begin);

        BinaryFormatter bf = new BinaryFormatter();

        int length = (int)bf.Deserialize(fileStream);
        
        for (int i = 0; i < length; i++)
        {
            TimePoints.Enqueue((float)bf.Deserialize(fileStream));
        }

        LoadedRateList.Add(TimePoints);
        yield return TimePoints;
        fileStream.Close();
    }

    string FindSoundName(string path)
    {
        char[] name = path.ToCharArray();
        
        int songNameCount = 0;

        for (int i = name.Length - 1; i >= 0; i--)
        {
            if(name[i] == '/')
            {
                songNameCount = i +1;
                break;
            }
        }

        char[] songNameElements = new char[name.Length - songNameCount];

        for (int i = 0; i < songNameElements.Length; i++)
        {
            songNameElements[i] = name[i + songNameCount];
        }

        return new string(songNameElements);
    }

    string FindPath()
    {
        char[] path = Application.dataPath.ToCharArray();
        char[] newPath = new char[path.Length - 6];

        for (int i = 0; i < path.Length - 6; i++)
        {
            newPath[i] = path[i];
        }

        return new string(newPath);
    }
}
