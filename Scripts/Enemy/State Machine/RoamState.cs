using UnityEngine;
using System.Collections;

public class RoamState : IEnemyState {

    private readonly StatePatternEnemy enemy;
    private Vector3 m_NewPath;

    public RoamState(StatePatternEnemy state)
    { 
        enemy = state;
    }

    public void UpdateState()
    {
        Look();
        Roam();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            ToAlertState();
        }
    }

    public void ToRoamState()
    {

    }

    public void ToAlertState() 
    {
        enemy.m_Anim.SetTrigger("Alerted");
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        enemy.m_Anim.SetBool("IsMoving", true);
        enemy.currentState = enemy.chaseState;
    }

    public void ToNeutralState()
    {
        enemy.m_Anim.SetBool("IsMoving", false);
        enemy.currentState = enemy.neutralState;
        
    }
    public void ToPassiveState()
    {
        enemy.m_Anim.SetBool("IsMoving", false);
        enemy.currentState = enemy.passiveState;
        
    }

    private void Look()
    {
        RaycastHit hit;
        //fire a raycast from the eyes position to look for the player
        //if it hits a player object then change state
        if(Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.target = hit.transform;
            ToChaseState();
        }
    }

    private void Roam()
    {
        enemy.meshRendererFlag.material.color = Color.green;
        enemy.m_Nav.Resume();

        //get a new destination if current velocity is zero
        //Application: if this entity is stuck on a path, it will get a new destination
        if(enemy.m_Nav.velocity == Vector3.zero)
        {
            enemy.m_Nav.destination = GetNewDestination();
        }

        //get a new destination if the current path is finished
        if(enemy.m_Nav.remainingDistance <= enemy.m_Nav.stoppingDistance && !enemy.m_Nav.pathPending)
        {
            enemy.m_Nav.destination = GetNewDestination();
        }
    }

    //returns a new vector destination by getting a random spawn point position from the array
    public Vector3 GetNewDestination()
    {
        if (enemy.spawnPoints.Count > 0)
        {
            //get a random number
            int randomArea = Random.Range(0, enemy.spawnPoints.Count);

            //use the random number to get an item from the array to get its position
            m_NewPath = enemy.spawnPoints[randomArea].transform.position;

            //return that item's position
            return m_NewPath;
        }
        else
        {
            //if there are no spawn points, use the player's position as this entity's destination
            enemy.target = GameObject.FindGameObjectWithTag("Player").transform;
            return enemy.target.position;
        }


    }
}
