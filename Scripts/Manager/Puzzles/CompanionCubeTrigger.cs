using UnityEngine;
using System.Collections;

public class CompanionCubeTrigger : MonoBehaviour {
    [Header("Components")]
    public GameObject barrier;
    public Transform spawnPoint;
    public Gun objectToSpawn;
   
    MeshRenderer m_TriggerMaterial;
    bool m_isActive;
    int m_SpawnCount;


	void Start () {
        m_TriggerMaterial = GetComponent<MeshRenderer>();
        barrier.SetActive(true);
	}

    void Update()
    {
        if(barrier != null)
        {
            if (m_isActive)
                barrier.SetActive(false);
            else
                barrier.SetActive(true);
        }
    }

	void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MoveableObject") || other.CompareTag("Player"))
        {
            StartCoroutine(Fade(Color.red, Color.green, 0.5f));
            m_isActive = true;

            if(spawnPoint != null && objectToSpawn != null)
            {
                if(m_SpawnCount == 0)
                {
                    m_SpawnCount++;
                    Gun spawnGun = Instantiate(objectToSpawn, spawnPoint.position + Vector3.up, spawnPoint.rotation) as Gun;
                }
                    
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MoveableObject") || other.CompareTag("Player"))
        {
            StartCoroutine(Fade(Color.red, Color.green, .5f));
            m_isActive = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MoveableObject") || other.CompareTag("Player"))
        {
            StartCoroutine(Fade(Color.green, Color.red, .5f));
            m_isActive = false;
        }
    }
        

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;
        while(percent <= 1)
        {
            percent += Time.deltaTime * speed;
            m_TriggerMaterial.material.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }
}
