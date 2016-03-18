using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    public string levelToLoad;

    public GameObject background;
    public GameObject text;
    public GameObject progressBar;
    public GameObject story;

    private MenuScript m_Menu;
    private int m_LoadProgress = 0;

	void Start () {
        if(background != null || text != null || progressBar != null)
        {
            background.SetActive(false);
            text.SetActive(false);
            progressBar.SetActive(false);
        }
            
        if(story != null)
            story.SetActive(false);
	}

    public void StartPlaying()
    {
        StartCoroutine(DisplayLoadingScreen(levelToLoad));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(DisplayLoadingScreen(levelToLoad));
        }
    }

    IEnumerator DisplayLoadingScreen(string level)
    {
        background.SetActive(true);
        text.SetActive(true);
        progressBar.SetActive(true);

        if(story != null)
            story.SetActive(true);

        progressBar.transform.localScale = new Vector3(m_LoadProgress, progressBar.transform.localScale.y, progressBar.transform.localScale.z); //Set the transform of the progress bar

        text.GetComponent<GUIText>().text = "Loading Progress: " + m_LoadProgress + "%";

        AsyncOperation async = Application.LoadLevelAsync(level);

        while(!async.isDone)
        {
            m_LoadProgress = (int)(async.progress * 100);

            text.GetComponent<GUIText>().text = "Loading Progress: " + m_LoadProgress + "%";

            progressBar.transform.localScale = new Vector3(async.progress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

            //Still needs testing
            //this requires the user to press any key to continue loading the next scene
            if(async.progress >= 0.9f)
            {
                async.allowSceneActivation = false;

                text.GetComponent<GUIText>().text = "Press Any Key To Continue!";
                progressBar.SetActive(false);

                if(Input.anyKeyDown)
                {
                    async.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
