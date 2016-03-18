using UnityEngine;
using System.Collections;

public class EnemyMovement : LivingEntity {

    [Header("Enemy Attributes")]
    public float m_Duration = 4f;
    public bool isAggressive = true;
    public int m_Cereb;
    public int m_WCereb;


    public float Duration
    {
        get { return m_Duration; }
        set { m_Duration = value; }
    }

	protected override void Start () {
        base.Start();
        m_Cereb = Random.Range(20, 2020);
        m_WCereb = Random.Range(100, 50000);
	}
	
	void Update () 
    {
        if(m_Health <= 0 && !m_Dead)
        {
            base.Die();
        }

	}
}
