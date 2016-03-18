using UnityEngine;
using System.Collections;

public class HiddenPathEnemy : LivingEntity {

    NavMeshAgent nav;
    GameObject player;


    [Header("Enemy Attributes")]
    public float m_Duration = 4f;
    public bool isAggressive = true;
    public int m_Cereb;
    public int m_WCereb;

    StatePatternEnemy m_State;
    Vector3 m_ReturnPosition;

    public float Duration
    {
        get { return m_Duration; }
        set { m_Duration = value; }
    }

    protected override void Start()
    {
        base.Start();


        //initialize variables
        player = GameObject.FindGameObjectWithTag("Player");
        m_State = GetComponent<StatePatternEnemy>();
        nav = GetComponent<NavMeshAgent>();
        m_Cereb = Random.Range(20, 2020);
        m_WCereb = Random.Range(100, 50000);

        m_State.currentState = m_State.alertState;
        if (player != null)
            nav.SetDestination(player.transform.position);
        else
            nav.SetDestination(m_ReturnPosition);
     
    }

    public void SetReturnPosition(Vector3 spawnPos)
    {
        m_ReturnPosition = spawnPos;
    }

    void Update()
    {
        if (player != null)
        {
            nav.SetDestination(player.transform.position);
        }
        else
        {
            nav.SetDestination(m_ReturnPosition);
        }
        

        if (m_Health <= 0 && !m_Dead)
        {
            base.Die();
        }

    }
}
