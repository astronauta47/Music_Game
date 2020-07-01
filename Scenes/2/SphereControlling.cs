using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SphereControlling : SaveLoad
{
    int dir = 1;
    public float timer;
    float beforeStartTime;
    public Queue<float> pointsList = new Queue<float>();
    static int PointsListLength;
    [SerializeField] AudioSource audioSource;
    static AudioClip audioClip;
    [SerializeField] AudioClip WinSound;
    [SerializeField] Text songNameDisplay;

    [SerializeField] Image progresBar;
    [SerializeField] Text progresBarText;

    int counter;
    [SerializeField] int pointsCount;

    [SerializeField] PathPlacer pathPlacer;

    [SerializeField] Scrollbar scrollbarMode;
    [SerializeField] Text RecordText;
    [SerializeField] Text AverageText;

    [SerializeField] Canvas MenuUICanvas;
    [SerializeField] Canvas GameUICanvas;

    public static bool IsGameOverMode;
    bool IsGameLoaded;

    [SerializeField] Sprite[] MuteImgs;

    void LoadFiles()
    {
        Debug.Log("Start" + TimePoints.Count);

        pointsList.Clear();
        for (int i = 0; i < TimePoints.Count; i++)
        {
            pointsList.Enqueue(TimePoints.Peek());
            TimePoints.Enqueue(TimePoints.Peek());
            TimePoints.Dequeue();
        }

        songNameDisplay.text = songName;
        audioClip = audioSource.clip;
        
        bool tmp = true;

        while (tmp)
        {
            if (pointsList.Peek() < 3)
                pointsList.Dequeue();
            else
            {
                tmp = false;
            }
        }

        audioSource.Play();
        PointsListLength = pointsList.Count;
        float lastValue = 100;
        float space = 0.05f;

        //Debug.Log(pointsList.Peek());

        
        for (int i = 0; i < PointsListLength; i++)
        {
            if (pointsList.Peek() - lastValue < 0.15f)
            {
                lastValue = pointsList.Peek() + space;
                pointsList.Dequeue();
                pointsList.Enqueue(lastValue);
                space += 0.05f;
            }
            else
            {
                lastValue = pointsList.Peek();
                pointsList.Enqueue(lastValue);
                pointsList.Dequeue();
                space = 0.05f;
            }


        }
        
        if (PlayerPrefs.HasKey("ModeValue")) scrollbarMode.value = PlayerPrefs.GetInt("ModeValue");

        //songNameDisplay.text = songName;

        songNameDisplay.transform.GetChild(0).gameObject.SetActive(false);//loading off;
        MenuUICanvas.transform.GetChild(8).GetComponent<LoadingText>().enabled = false;
        MenuUICanvas.transform.GetChild(8).GetComponent<Text>().text = "Ready";

        Display();

        audioSource.loop = true;

        IsGameLoaded = true;

        
    }

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        OnLoadedGame += LoadFiles;
        LoadGame(ref audioSource);
    }

    void Update()
    {

        timer += Time.deltaTime;

        if (!PathFollower.Isstart)
            return;
            

        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            dir *= -1;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, 0.5f * dir, transform.localPosition.z), Time.deltaTime * 10);
        
        if (pointsList.Count != 0 && timer > pointsList.Peek() -4 + beforeStartTime)
        {
            if (counter < pointsCount)
            {
                counter++;
                pointsList.Dequeue();
            }
            else
            {
                pathPlacer.Generate(pointsList.Peek() * 10 -1 + beforeStartTime * 10 + 100);
                pointsList.Dequeue();

                UpdateProgressBar();

                counter = 0;
            }

            if (pointsList.Count == 0)
            {
                
                timer = 0;
                StartCoroutine(Counter());
                
            }
        }
    }

    IEnumerator Counter()
    {
        yield return new WaitForSeconds(5);
            audioSource.PlayOneShot(WinSound);

        yield return new WaitForSeconds(3);

            PlayerPrefs.SetFloat("PointsTime" + ModeName() + songNameDisplay.text, (PointsListLength - GetComponent<GameOver3D>().MinusPoints) * 100 / PointsListLength);
            DisplayPoints();

        PathFollower.Isstart = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void UpdateProgressBar()
    {
        if(IsGameOverMode)
        {
            float value = Mathf.RoundToInt((PointsListLength - pointsList.Count) * 100 / PointsListLength);

            progresBar.fillAmount = value / 100;
            progresBarText.text = value.ToString() + '%';
        }
        else
        {
            float value = Mathf.RoundToInt((PointsListLength - GetComponent<GameOver3D>().MinusPoints) * 100 / PointsListLength);
            float timeValue = Mathf.RoundToInt((PointsListLength - pointsList.Count) * 100 / PointsListLength);

            progresBar.fillAmount = timeValue / 100;
            progresBarText.text = value.ToString() + '%';
        }

    }

    public void MuteAudio()
    {
        float value = audioSource.volume * -1 + 1;
        audioSource.volume = value;
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = MuteImgs[(int)value];
    }

    public void StartGame()
    {
        if (!IsGameLoaded)
            return;

        beforeStartTime = timer;

        if (scrollbarMode.value == 0)
        {
            pointsCount = 0;
            IsGameOverMode = false;
            PlayerPrefs.SetInt("ModeValue", 0);
        }
        else
        {
            pointsCount = 1;
            IsGameOverMode = true;
            PlayerPrefs.SetInt("ModeValue", 1);
        }

        PathFollower.Isstart = true;
        audioSource.Play();

        MenuUICanvas.enabled = false;
        GameUICanvas.enabled = true;

        UpdateProgressBar();
        audioSource.loop = false;
        //pathPlacer.gameObject.GetComponent<PathCreation.PathCreator>().Generate(30);
    }

    public void SaveTime()
    {
        PlayerPrefs.SetFloat("PointsTime" + ModeName() + songNameDisplay.text, timer * 100 / audioSource.clip.length);
        DisplayPoints();
    }

    public void DisplayPoints()
    {

        float percent = PlayerPrefs.GetFloat("PointsTime" + ModeName() + songNameDisplay.text);
        int average;
        int averageCount;

        average = PlayerPrefs.GetInt("PointsAverage" + ModeName() + songNameDisplay.text);
        averageCount = PlayerPrefs.GetInt("PointsAverageCount" + ModeName() + songNameDisplay.text);


        PlayerPrefs.SetInt("PointsAverage" + ModeName() + songNameDisplay.text, Mathf.RoundToInt(average + percent));
        PlayerPrefs.SetInt("PointsAverageCount" + ModeName() + songNameDisplay.text, averageCount + 1);


        if (PlayerPrefs.GetInt("PointsPercent" + ModeName() + songNameDisplay.text) < percent)
        {
            PlayerPrefs.SetInt("PointsPercent" + ModeName() + songNameDisplay.text, Mathf.RoundToInt(percent));
        }


        Display();
        
    }

    string ModeName()
    {
        if (IsGameOverMode)
            return "Real";
        else
            return "Easy";
    }

    void Display()
    {
        if (!PlayerPrefs.HasKey("PointsPercent" + ModeName() + songNameDisplay.text))
        {
            RecordText.text = "0%";
            AverageText.text = "0%";
            return;
        }
            

        RecordText.text = PlayerPrefs.GetInt("PointsPercent" + ModeName() + songNameDisplay.text).ToString() + '%';
        AverageText.text = (PlayerPrefs.GetInt("PointsAverage" + ModeName() + songNameDisplay.text) / PlayerPrefs.GetInt("PointsAverageCount" + ModeName() + songNameDisplay.text)).ToString() + '%';
    }

    public void ChangeMode()
    {

        if(scrollbarMode.value < 0.5f)
            IsGameOverMode = false;
        else
            IsGameOverMode = true;

        Display();
    }




    
}
