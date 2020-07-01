using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PianoGenerator : SaveLoad
{
    public  List<Transform> ActualPianoPositions = new List<Transform>();
    public Transform actualPianoPosition;
    public Transform[] SpawnPoints;
    int pianoindex, musicIndex;
    float timer = .75f;

    public AudioSource audioSource;

    int LastRandomPos;

    //Import values
    string SongName;
    public Text songNameDisplay;
    int SongLength;
    Queue<float> PianoTimePoints = new Queue<float>();
    [SerializeField] AudioClip winClip;
    bool IsStart, IsLoadGame;
    [SerializeField] GameObject loadingBar;
    [SerializeField] GameObject startGameCanvas;
    [SerializeField] GameObject gameCanvas;
    //

    int songIndex;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        OnLoadedGame += GameLoaded;
        LoadGame(ref audioSource);
    }

    void GameLoaded()
    {
        IsLoadGame = true;
        loadingBar.SetActive(false);

        songNameDisplay.GetComponent<LoadingText>().enabled = false;
        songNameDisplay.text = songName;

        PianoTimePoints.Clear();
        for (int i = 0; i < TimePoints.Count; i++)
        {
            PianoTimePoints.Enqueue(TimePoints.Peek());
            TimePoints.Enqueue(TimePoints.Peek());
            TimePoints.Dequeue();
        }
    }

    public void StartGame()
    {
        if(IsLoadGame)
        {
            audioSource.Play();
            IsStart = true;
            startGameCanvas.SetActive(false);
            gameCanvas.SetActive(true);
        }
        
    }

    private void Update()
    {

        if (!IsStart)
            return;

        
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit.collider != null)
                {
                    //if (hit.collider.tag == "PianoElement")
                    {
                        //if(hit.collider.transform.Equals(actualPianoPosition))
                        //{
                        hit.collider.transform.position = new Vector2(20, 10);

                        //musicIndex++;

                        //if (musicIndex >= ActualPianoPositions.Count)
                        //    musicIndex = 0;

                        //actualPianoPosition = ActualPianoPositions[musicIndex];

                        //}
                        //else
                        //{
                        //    GameOver();
                        //}

                    }
                    //else
                    //{
                    //    GameOver();
                    //}
                }
                //else { GameOver(); }


            }
        }
        
        if (PianoTimePoints.Count != 0)
        {
            timer += Time.deltaTime;

            if (timer > PianoTimePoints.Peek())
            {
                PianoTimePoints.Dequeue();

                pianoindex++;

                if (pianoindex >= ActualPianoPositions.Count)
                    pianoindex = 0;

                int randomPos;

                while (true)
                {
                    randomPos = Random.Range(0, 4);

                    if (randomPos != LastRandomPos)
                        break;
                }


                ActualPianoPositions[pianoindex].position = SpawnPoints[randomPos].position;

                LastRandomPos = randomPos;
            }
        }
        else
        {
            StartCoroutine(Timer());
            IsStart = false;
        }


    }

    IEnumerator Timer()
    {

        audioSource.PlayOneShot(winClip);

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }



    void GameOver()
    {
        Debug.Log("GameOver");
    }

}
