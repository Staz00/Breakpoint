using UnityEngine;
using System.Collections;


public class EnemyDetector : MonoBehaviour {

    StatePatternEnemy statePatternEnemy;

	void Start () {
        statePatternEnemy = GetComponentInParent<StatePatternEnemy>();
	}
	
	void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            statePatternEnemy.currentState.OnTriggerEnter(other); 
        }
    }
}
