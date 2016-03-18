using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TerminalScript : MonoBehaviour {
    [Header("Components")]
    public GameObject m_Terminal;
    public Canvas m_EventUI;
    public Text m_CountText;
    public Text m_SavedText;
    public Text m_TotalCountText;
    public Text m_ConditionText;
    public GameObject m_Barrier;

    [Header("SoundFx")]
    public AudioClip m_Select;
    public AudioClip m_ErrorSelect;
    [Range(0,1)]
    public float m_Volume = 0.5f;

    [Header("Properties")]
    public int m_Required;
    public int m_NormalisedReq;
    public bool isOpen = false;

    public EnemyMovement[] m_EnemyType;

    [HideInInspector]
    public int m_MonsterKilled;
    [HideInInspector]
    public int m_Saved;

    [HideInInspector]
    public int m_TotalCount;

    private CompanionCubeTrigger m_Trigger;
    private EnemyMovement[] m_Enemy;

	void Start () {
        m_Terminal.SetActive(false);

        m_Enemy = FindObjectsOfType(typeof(EnemyMovement)) as EnemyMovement[];

        //add the delegate event from LivingEntity class to these objects to have access to when these entities die
        if(m_Enemy != null)
        {
            foreach (EnemyMovement script in m_Enemy)
            {
                script.OnDeath += OnEnemyDeath;
            }
        }
	}

    void OnEnemyDeath()
    {
        //change values when an enemy dies
        m_MonsterKilled++;
        m_TotalCount--;
    }
	
	void Update () {

        if(isOpen)
        {
            m_CountText.text = m_MonsterKilled.ToString();
            m_SavedText.text = m_Saved.ToString();

            m_ConditionText.text = "if (mMonsterKilled >= " + m_Required.ToString() + " && mNormalized >= " + m_NormalisedReq.ToString() + "  && mRemainingEnemies == 0)\n{\n\tOpenGate();\n}";

            m_TotalCountText.text = m_TotalCount.ToString();
        }

    }

    public void SummonEnemy()
    {
        //find current and active spawnPoints
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");

        if (spawnPoints != null)
        {
            int randomEnemyType = 0;
            //get a random spawn point index
            
            int randomSpawn = Random.Range(0, spawnPoints.Length);

            //get a random enemy type index
            if (m_EnemyType.Length > 1)
            {
                randomEnemyType = Random.Range(0, m_EnemyType.Length);
            }
             

            //only call this function from the active and enabled game object in the hierarchy
            //this is essential because there are multiple game objects with the same script attached that can call this same function
            if(this.isActiveAndEnabled)
            {
                Instantiate(m_EnemyType[randomEnemyType], spawnPoints[randomSpawn].transform.position, Quaternion.identity);        //instantiate the enemy type based on the random index from the previous line

                //increase total count
                m_TotalCount++;

            }

            //find all enemy type of objects
            m_Enemy = FindObjectsOfType(typeof(EnemyMovement)) as EnemyMovement[];


            //Add the delegate death event to these objects
            if (m_Enemy != null)
            {
                foreach (EnemyMovement script in m_Enemy)
                {
                    script.OnDeath -= OnEnemyDeath;
                }

                foreach(EnemyMovement script in m_Enemy)
                {
                    script.OnDeath += OnEnemyDeath;
                }
            }

            //play soundfx
            AudioScript.m_Audio.PlaySoundFx(m_Select, m_Volume);
        }
    }

    public void OpenTerminal()
    {
        isOpen = true;

        PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if (player != null)
        {
            player.IsPaused = true;
        }

        m_Terminal.SetActive(true);
    }

    public void OnClosePress()
    {
        PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if (player != null)
        {
            player.IsPaused = false;
        }

        isOpen = false;
        m_Terminal.SetActive(false);
    }

    public void SetRequirements(int killed, int normalized, int totalCount)
    {
        m_Required = killed;
        m_NormalisedReq = normalized;
        m_TotalCount = totalCount;
    }

    public void OnCheckPress()
    {
        //if all values are equal to required and if this object is active in the hierarchy
        if (m_MonsterKilled >= m_Required && m_Saved >= m_NormalisedReq && gameObject.activeInHierarchy && m_TotalCount == 0)
        {
            m_MonsterKilled = 0;
            m_Saved = 0;
            m_TotalCount = 0;

            OpenGate();
            AudioScript.m_Audio.PlaySoundFx(m_Select, m_Volume);
        }
        else
        {
            AudioScript.m_Audio.PlaySoundFx(m_ErrorSelect, m_Volume);
            return;
        }

    }

    void OpenGate()
    {
        //disable game objects
        m_EventUI.enabled = false;
        isOpen = false;
        OnClosePress();
        m_Barrier.SetActive(false);
        this.gameObject.SetActive(false);


        //find all enemy and spawn points
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");

        //destroy all found enemy and spawn points
        foreach(GameObject obj in enemy)
        {
            Destroy(obj);
        }

        foreach(GameObject spawns in spawnPoints)
        {
            Destroy(spawns);
        }
    }
}
