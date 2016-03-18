using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ShatteringScript))]
[RequireComponent(typeof(NavMeshAgent))]
public class StatePatternEnemy : MonoBehaviour {

    [Header("State Pattern Attributes")]
    public float searchingTurnSpeed = 120f;
    public float searchDuration = 4f;
    public float sightRange = 15f;
    public float attackDamage = 5;
    public float attackSpeed;
    public float duration = 4f;

    [Header("UI Component")]
    public Canvas agroUI;
    public MeshRenderer meshRendererFlag;
    public Transform eyes;
    public LayerMask playerMask;


    public Vector3 offset = new Vector3(0, 0.5f, 0);

    [HideInInspector]
    public Transform target;

    [HideInInspector]
    public IEnemyState currentState;

    [HideInInspector]
    public AlertState alertState;

    [HideInInspector]
    public ChaseState chaseState;

    [HideInInspector]
    public RoamState roamState;

    [HideInInspector]
    public NeutralState neutralState;

    [HideInInspector]
    public PassiveState passiveState;

    [HideInInspector]
    public NavMeshAgent m_Nav;

    [HideInInspector]
    public Animator m_Anim;

    [HideInInspector]
    public List<GameObject> spawnPoints;

    private GameObject m_Player;

    void Awake()
    {
        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        roamState = new RoamState(this);
        neutralState = new NeutralState(this);
        passiveState = new PassiveState(this);

        m_Nav = GetComponent<NavMeshAgent>();
    }

	void Start () {
        currentState = roamState;

        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Anim = GetComponent<Animator>();

        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawner");

        foreach(GameObject spawn in spawns)
        {
            spawnPoints.Add(spawn);
        }

        agroUI.enabled = false;
	}


	void Update () {
        currentState.UpdateState();
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            currentState.OnTriggerEnter(other);
        }
        
    }


    //add delay before dealing damage to compensate for the delay due to animation
    IEnumerator WaitToDealDamage(RaycastHit hit)
    {
        yield return new WaitForSeconds(0.5f);

        if(target != null)
        {
            hit.collider.GetComponent<LivingEntity>().TakeDamage(attackDamage);
        }
            
    }

    public void DoDamage(RaycastHit hit)
    {
        
        m_Anim.SetTrigger("Attack");
        StartCoroutine(WaitToDealDamage(hit));
    }
}
