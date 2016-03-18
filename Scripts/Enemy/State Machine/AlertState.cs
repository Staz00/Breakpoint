using UnityEngine;
using System.Collections;

public class AlertState : IEnemyState {

    private readonly StatePatternEnemy enemy;
    private float m_SearchTimer;

    public AlertState(StatePatternEnemy state)
    {
        enemy = state;
    }

    public void UpdateState()
    {
        Look();
        Search();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToRoamState()
    {
        enemy.m_Anim.SetBool("IsMoving", false);
        enemy.agroUI.enabled = false;
        enemy.currentState = enemy.roamState;

        m_SearchTimer = 0f;
    }

    public void ToAlertState()
    {

    }

    public void ToChaseState()
    {
        enemy.m_Anim.SetBool("IsMoving", true);
        enemy.agroUI.enabled = true;
        enemy.currentState = enemy.chaseState;

        m_SearchTimer = 0f;
    }

    public void ToNeutralState()
    {
        enemy.m_Anim.SetBool("IsMoving", false);
        enemy.currentState = enemy.neutralState;

        m_SearchTimer = 0f;
    }

    public void ToPassiveState()
    {

    }

    private void Look()
    {
        RaycastHit hit;

        //change this entity's state to chase state if it sees a player object
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.target = hit.transform;
            ToChaseState();
        }
        else
        {
            enemy.target = enemy.transform;
        }
    }
    private void Search()
    {
        enemy.meshRendererFlag.material.color = Color.Lerp(Color.green, Color.yellow, 2f);
        enemy.m_Nav.Stop();

        //rotate this entity around while looking
        enemy.transform.Rotate(0, enemy.searchingTurnSpeed * Time.deltaTime, 0);
        m_SearchTimer += Time.deltaTime;

        //if it fails to see the player while searching, then return to roam state
        if (m_SearchTimer >= enemy.searchDuration)
            ToRoamState();
    }
}
