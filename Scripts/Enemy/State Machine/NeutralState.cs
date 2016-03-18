using UnityEngine;
using System.Collections;

public class NeutralState : IEnemyState {

    private readonly StatePatternEnemy enemy;
    private Vector3 m_NewPath;

    public NeutralState(StatePatternEnemy state)
    {
        enemy = state;
    }

    public void UpdateState()
    {
        Roam();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToRoamState()
    {

    }

    public void ToAlertState()
    {

    }

    public void ToChaseState()
    {

    }

    public void ToNeutralState()
    {

    }

    public void ToPassiveState()
    {
        enemy.m_Anim.SetBool("IsMoving", false);
        enemy.currentState = enemy.passiveState;
        
    }

    private void Roam()
    {
        enemy.meshRendererFlag.material.color = Color.grey;
        enemy.m_Nav.Resume();

        if (enemy.m_Nav.remainingDistance <= enemy.m_Nav.stoppingDistance && !enemy.m_Nav.pathPending)
        {
            if (enemy.spawnPoints != null)
                enemy.m_Nav.destination = GetNewDestination();
            else
                ToPassiveState();
        }
    }

    public Vector3 GetNewDestination()
    {
        int randomArea = Random.Range(0, enemy.spawnPoints.Count);
        m_NewPath = enemy.spawnPoints[randomArea].transform.position;
        return m_NewPath;   
    }
}
