using UnityEngine;
using System.Collections;

public class SpawnBossScript : MonoBehaviour {

    public GameObject bossObject;
    public GameObject spawnPosition;

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Instantiate(bossObject, spawnPosition.transform.position, spawnPosition.transform.rotation);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
