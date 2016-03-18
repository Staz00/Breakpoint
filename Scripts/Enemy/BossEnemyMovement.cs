using UnityEngine;
using System.Collections;

public class BossEnemyMovement : LivingEntity {


    public int normalAttackDmg = 20;
    public float AttackCooldown = 2f;
    public float stoppingDistance = 5f;
    public float rotationSpeed = 10f;

    private GameObject barrier;

    [Range(0, 1)]
    public float m_Volume;

    private NavMeshAgent m_Nav;
    private GameObject m_Player;
    private Animator m_Anim;
    private ShatteringScript m_ShatteringScript;
    private TrailRenderer[] m_Trail;
    private float m_Timer;
    private float m_ShatteredPcsTimer = 5f;
	protected override void Start () {
        base.Start();

        //initialize references
        m_Player = GameObject.FindGameObjectWithTag("Player");

        m_Nav = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();
        m_ShatteringScript = GetComponent<ShatteringScript>();

        m_Trail = GetComponentsInChildren<TrailRenderer>();
        //end of initialization

        //check if there is a trail renderer in any child objects of this script
        if(m_Trail.Length != 0)
        {
            for (int i = 0; i < m_Trail.Length; i++)
            {
                m_Trail[i].enabled = false;
            }
        }

        //set the path finder's stopping distance from the target
        m_Nav.stoppingDistance = stoppingDistance;


        //set the initial animation
        m_Anim.SetBool("IsMoving", true);

        barrier = GameObject.FindGameObjectWithTag("BossAreaBarrier");
	
	}
	
	void Update () {
        m_ShatteredPcsTimer -= Time.deltaTime;

        //set the destination of this entity
        if(m_Player != null)
            m_Nav.SetDestination(m_Player.transform.position);

        //spawn shattered pieces
        if(m_ShatteredPcsTimer <= 0)
        {
            m_ShatteredPcsTimer = 5f;
            m_ShatteringScript.ShatterEffect();
        }

        //check if entity is still alive
        if (m_Health <= 0)
        {
            base.Die();

            if(barrier != null)
                barrier.SetActive(false);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //initialize timer
            m_Timer = AttackCooldown;

            //change animation
            m_Anim.SetBool("IsMoving", false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            m_Anim.SetBool("IsMoving", true);
        }
    }

    void OnTriggerStay(Collider other)
    { 
        if(other.tag == "Player")
        {
            //rotate game object towards its target
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_Player.transform.position - transform.position),
                    Time.deltaTime * rotationSpeed);

            //count down timer
            m_Timer -= Time.deltaTime;

            //if timer reaches 0 then attack
            if(m_Timer <= 0)
            {
                //reset timer
                m_Timer = AttackCooldown;

                //find all trail renderers in the array and if any, activate them when attacking
                if(m_Trail.Length != 0)
                {
                    for (int i = 0; i < m_Trail.Length; i++)
                    {
                        m_Trail[i].enabled = true;
                    }
                }
                
                //initiate attack animation
                m_Anim.SetTrigger("Attack");

                //tell the living entity to do the damage
                m_Player.GetComponent<LivingEntity>().TakeDamage(normalAttackDmg);

                //disable the trail effect again after some time
                StartCoroutine(DisableTrailEffect());
                
            }
        }
    }

    IEnumerator DisableTrailEffect()
    {
        yield return new WaitForSeconds(2f);
        if (m_Trail.Length != 0)
        {
            for (int i = 0; i < m_Trail.Length; i++)
            {
                m_Trail[i].enabled = false;
            }
        }
    }


}
