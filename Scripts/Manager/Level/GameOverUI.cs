using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverUI : MonoBehaviour {

    public Image fadePlane;
    public GameObject gameOverUI;


	// Use this for initialization
	void Start ()
    {
        FindObjectOfType<PlayerHealth>().OnDeath += OnGameOver;
        fadePlane.enabled = false;
	}

    void OnGameOver()
    {
        Cursor.visible = true;
        fadePlane.enabled = true;
        StartCoroutine(Fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
    }

    public void RestartPress()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void MainMenuPress()
    {
        Application.LoadLevel("MainMenu");
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

}
