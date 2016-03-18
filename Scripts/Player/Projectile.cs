using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    [Header("Layer Mask")]
    public LayerMask collisionMask;

    [Header("Projectile Attributes")]
    public int damage = 5;

    float m_Speed = 10;
    float m_SkinWidth = 0.1f;

    EnemyMovement m_Enemy;
    StatePatternEnemy m_State;

    void Start()
    {
        //check for collisions when this object has just intantiated
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if(initialCollisions.Length > 0)
        {
            OnHitCheck(initialCollisions[0]);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        m_Speed = newSpeed;
    }

	void Update () {
        float moveDistance = m_Speed * Time.deltaTime;
        CheckCollision(moveDistance);

        //move this object by its forward vector
        transform.Translate(Vector3.forward * moveDistance);

        //destroy after some seconds
        Destroy(this.gameObject, 1f);
	}

    void CheckCollision(float distance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, distance + m_SkinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitCheck(hit);
        }
    }

    void OnHitCheck(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();

        //check the tag of the object that was hit
        if(hit.collider.CompareTag("Enemy"))
        { 

            m_State = hit.collider.GetComponent<StatePatternEnemy>();

            if (damageableObject != null)
            {
                damageableObject.TakeHit(damage, hit);
            }

            if (m_State.currentState != m_State.chaseState)
                m_State.currentState = m_State.alertState;

            GameObject.Destroy(gameObject);
        }
        else if(hit.collider.CompareTag("Boss"))
        {
           

            if (damageableObject != null)
            {
                damageableObject.TakeHit(damage, hit);
            }

            GameObject.Destroy(gameObject);
        }
        else if(hit.collider.CompareTag("Obstacles"))
        {


            GameObject.Destroy(gameObject);
        }
    }

    void OnHitCheck(Collider collider)
    {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();
        if (collider.CompareTag("Enemy"))
        {
            m_State = collider.GetComponent<StatePatternEnemy>();

            if (damageableObject != null)
            {
                damageableObject.TakeDamage(damage);
            }

            if (m_State.currentState != m_State.chaseState)
                m_State.currentState = m_State.alertState;

            GameObject.Destroy(gameObject);
        }
        else if (collider.CompareTag("Boss"))
        {
            if (damageableObject != null)
            {
                damageableObject.TakeDamage(damage);
            }

            GameObject.Destroy(gameObject);
        }
    }
}
