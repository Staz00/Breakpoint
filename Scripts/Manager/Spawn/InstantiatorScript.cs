using UnityEngine;
using System.Collections;

public class InstantiatorScript : MonoBehaviour {

    //public GameObject m_Spawner;
    public GameObject m_Barrier;
    public GameObject m_TerminalPC;
    public GameObject m_HiddenTerminals;
    public GameObject[] spawnPoints;
    public GameObject[] enemy;

    [Header("Min & Max")]
    public int m_MinValue = 1;
    public int m_MaxValue = 5;

    void Start()
    {
        m_MaxValue = Mathf.Clamp(m_MaxValue, 2, 10);
    }

	void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject hiddenTerminals = GameObject.FindGameObjectWithTag("HiddenTerminal");
            if (hiddenTerminals != null)
            {
                Destroy(hiddenTerminals);
            }

            //activate barriers and terminals
            if (m_Barrier != null || m_TerminalPC != null)
            {
                m_Barrier.SetActive(true);
                m_TerminalPC.SetActive(true);
            }

            if(m_HiddenTerminals != null)
            {
                m_HiddenTerminals.SetActive(true);
            }
                
            //get random values
            int monsterNumber = Random.Range(m_MinValue, m_MaxValue);
            int monsterSaved = Random.Range(m_MinValue, m_MaxValue);
            int monsterCount = monsterNumber + monsterSaved;

            //find a terminal script to initialize the required values
            if (FindObjectOfType<TerminalScript>() != null)
            {
                m_TerminalPC.GetComponent<TerminalScript>().SetRequirements(monsterNumber, monsterSaved, monsterCount);
            }

            //set all spawn points to active
            foreach(GameObject spawn in spawnPoints)
            {
                spawn.SetActive(true);
            }

            //spawn enemies
            for (int i = 0; i < monsterCount; i++)
            {
                //get random index for enemy types
                int spawnIndex = Random.Range(0, spawnPoints.Length);

                //only execute if there are more than 2 enemy types in the array
                if (enemy.Length > 1)
                {
                    int rng = Random.Range(0, enemy.Length);

                    //get the correct enemy type based on the random index
                    switch (rng)
                    {
                        case 0:
                            Instantiate(enemy[rng].gameObject, spawnPoints[spawnIndex].transform.position, Quaternion.identity);
                            break;
                        case 1:
                            Instantiate(enemy[rng].gameObject, spawnPoints[spawnIndex].transform.position, Quaternion.identity);
                            break;
                        default: break;
                    }
                }
                //otherwise, only summon the first item in the array
                else
                {
                    Instantiate(enemy[0].gameObject, spawnPoints[spawnIndex].transform.position, Quaternion.identity);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
