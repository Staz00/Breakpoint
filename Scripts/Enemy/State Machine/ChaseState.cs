using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyState {

    private readonly StatePatternEnemy enemy;
    private EnemyMovement m_EnemyMovement;
    private float m_Timer;

    private LivingEntity targetEntity;

    private bool hasTarget;

    public ChaseState(StatePatternEnemy state)
    {
        enemy = state;
        m_Timer = enemy.attackSpeed;
    }

    public ChaseState(EnemyMovement move)
    {
        m_EnemyMovement = move;
    }

    public void UpdateState()
    {
        
        Look();
        Chase();
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
        enemy.agroUI.enabled = false;
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {

    }

    public void ToNeutralState()
    {
        enemy.m_Anim.SetBool("IsMoving", false);
        enemy.agroUI.enabled = false;
        enemy.currentState = enemy.neutralState;
    }
    public void ToPassiveState()
    {

    }

    void OnTargetDeath()
    {
        hasTarget = false;
        ToRoamState();

        Application.LoadLevel(Application.loadedLevel);
    }


    private void Look()
    {
        RaycastHit hit;
        if (enemy.target != null)
        {
            Vector3 enemyToTarget = (enemy.target.position + enemy.offset) - enemy.eyes.transform.position;

            //while this entity sees a player object, stay in this state otherwise return to alert state
            if (Physics.Raycast(enemy.eyes.transform.position, enemyToTarget, out hit, enemy.sightRange, enemy.playerMask) && hit.collider.CompareTag("Player"))
            {
                hasTarget = true;

                enemy.target = hit.transform;


                //if this entity is close enough to the target, then initiate attack
                if (Vector3.Distance(enemy.eyes.transform.position, hit.transform.position) <= 4f)
                {

                    m_Timer -= Time.deltaTime;

                    //Attack cooldown
                    if (m_Timer <= 0)
                    {
                        m_Timer = enemy.attackSpeed;

                        //rotate this entity towards its target
                        enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, Quaternion.LookRotation(hit.collider.transform.position - enemy.transform.position),
                                                  Time.deltaTime * 20f);

                        enemy.DoDamage(hit);

                    }
                }
            }
            else
            {
                ToAlertState();
            }
        }
        else
            ToAlertState();

    }


    private void Chase()
    {
        if (hasTarget && enemy.target != null)
        {

            //change the cube's colour to red
            enemy.meshRendererFlag.material.color = Color.red;

            //set the path finding agent's destination to the player's position
            enemy.m_Nav.destination = enemy.target.position;
            enemy.m_Nav.Resume();
        }
    }

}
