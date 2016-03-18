using UnityEngine;
using System.Collections;

public class HiddenEnemySpawner : MonoBehaviour {

    public static HiddenEnemySpawner spawner;

    public Wave[] waves;
    public HiddenPathEnemy[] enemy;
    public Gun gun;

    private Wave m_CurrentWave;
    private int m_CurrentWaveNumber;

    private int m_RemainingEnemies;
    private int m_RemainingAlive;
    private float m_NextSpawnTime;

    void Awake()
    {
        spawner = this;
    }

    void Update()
    {
        //spawn new enemy once the time is greater than the next spawn time
        if(m_RemainingEnemies > 0 && Time.time > m_NextSpawnTime)
        {
            m_RemainingEnemies--;
            m_NextSpawnTime = Time.time + m_CurrentWave.timeBetweenSpawn;

            //get a random number to be used for picking random enemy types
            int rng = Random.Range(0, enemy.Length);
            HiddenPathEnemy spawnedEnemy = Instantiate(enemy[rng], transform.position + Vector3.up, Quaternion.identity) as HiddenPathEnemy;
            spawnedEnemy.SetReturnPosition(transform.position + Vector3.up);

            //add the death event
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath()
    {
        m_RemainingAlive--;

        //if there are no more enemies remaining in the current wave, then start the next wave of enemies
        if(m_RemainingAlive == 0)
        {
            NextWave();
        }
    }

    public void NextWave()
    {
        m_CurrentWaveNumber++;

        //deduct values from the current wave
        if(m_CurrentWaveNumber - 1 < waves.Length)
        {
            m_CurrentWave = waves[m_CurrentWaveNumber - 1];

            m_RemainingEnemies = m_CurrentWave.enemyCount;
            m_RemainingAlive = m_RemainingEnemies;
        }
        else
        {
            //spawn the gun once all enemies are dead and all the waves are done spawning
            if(gun != null)
                Instantiate(gun, transform.position + Vector3.up, Quaternion.identity);

            if(m_CurrentWaveNumber >= waves.Length)
            {
                Destroy(this.gameObject);
                return;
            }
        }
            
    }


    //Class that holds the enemy count and time between
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawn;
    }
}
