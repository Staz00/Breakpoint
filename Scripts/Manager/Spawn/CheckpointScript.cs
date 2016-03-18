using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CheckpointScript : MonoBehaviour {

    public Transform spawnPointPos;
    public Image fadePlane;
    public Text checkpointText;

    Vector3 m_SpawnPos;
    Color m_OrigColour;

    void Start()
    {
        m_OrigColour = fadePlane.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_SpawnPos = spawnPointPos.position;

            StartCoroutine(Fade(Color.clear, m_OrigColour, 1));
        }
    }

    //Returns the current spawnpoint;
    public Vector3 GetCheckpointSpawnPos()
    {
        return m_SpawnPos;
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        Color alpha;
        Color textAlpha;
        alpha = fadePlane.color;
        textAlpha = checkpointText.color;
        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;

            alpha.a = percent;
            fadePlane.color = alpha;

            textAlpha.a = percent;
            checkpointText.color = textAlpha;

            yield return null;
        }

        if (percent >= 1)
        {
            StartCoroutine(FadeOut(alpha, Color.clear, 1));
            StartCoroutine(FadeOut(textAlpha, Color.clear, 1));
        }

    }


    IEnumerator FadeOut(Color from, Color to, float time)
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
