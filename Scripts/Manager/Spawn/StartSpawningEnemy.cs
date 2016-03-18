using UnityEngine;
using System.Collections;

public class StartSpawningEnemy : MonoBehaviour {

    public GameObject enemySpawner;

    void Start()
    {
        if(enemySpawner != null)
            enemySpawner.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemySpawner != null)
                enemySpawner.SetActive(true);

            if(HiddenEnemySpawner.spawner != null && HiddenEnemySpawner.spawner.isActiveAndEnabled)
                HiddenEnemySpawner.spawner.NextWave();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
