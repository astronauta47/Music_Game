using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver3D : MonoBehaviour
{
    public string tagTarget;
    public int MinusPoints;
    public Material Red;
    [SerializeField] AudioClip GameOverSound;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == tagTarget)
        {
            //Debug.Log(SphereControlling.timer);

            if(SphereControlling.IsGameOverMode)
            {
                GetComponent<AudioSource>().PlayOneShot(GameOverSound);
                GetComponent<SphereControlling>().SaveTime();
                GetComponent<SphereControlling>().UpdateProgressBar();
                StartCoroutine(GameOver());
            }
            else
            {
                MinusPoints++;
                collision.GetComponent<MeshRenderer>().material = Red;
                GetComponent<SphereControlling>().UpdateProgressBar();
            }
            

            
        }
    }

    IEnumerator GameOver()
    {
        GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(0.2f);

        PathCreation.Examples.PathFollower.Isstart = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
