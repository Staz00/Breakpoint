using UnityEngine;
using System.Collections;

public class PassiveState : IEnemyState {

    private readonly StatePatternEnemy enemy;

    private float m_Timer;

    public PassiveState(StatePatternEnemy state)
    {
        enemy = state;
        m_Timer = enemy.duration;
    }

    public void UpdateState()
    {
        PauseState();
    }

    public void OnTriggerEnter(Collider other)
    {

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

    }

    public void ToNeutralState()
    {

    }
    public void ToPassiveState()
    {

    }

    private void PauseState()
    {
        enemy.meshRendererFlag.material.color = Color.grey;

        enemy.m_Nav.Stop();

        m_Timer -= Time.deltaTime;

        if(m_Timer <= 0)
        {
            enemy.m_Nav.Resume();
            ToAlertState();
        }
    }
}
