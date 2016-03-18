using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ShatteringScript))]
public class LivingEntity : MonoBehaviour, IDamageable {

    [Header("Entity Attributes")]
    public float m_StartingHealth;

    [Header("SoundFx")]
    public AudioClip m_HurtClip;
    public AudioClip m_DeathClip;
    [Range(0, 1)]
    public float m_Volumes = 0.5f;

    protected bool m_Dead;
    private TerminalScript m_TerminalScript;
    private ShatteringScript m_Shattering;

    [HideInInspector]
    public float m_Health;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        m_Health = m_StartingHealth;
    }

    //takes in the damage, and the target by passing a RaycastHit
    public void TakeHit(float damage, RaycastHit hit)
    {
        TakeDamage(damage);
    }

    //handles taking damage
    public void TakeDamage(float damage)
    {
        float newVol = Random.Range(m_Volumes - .1f, m_Volumes);
        AudioScript.m_Audio.PlaySoundFx(m_HurtClip, newVol);
        m_Health -= damage;
    }

    protected void HiddenEnemyDeath()
    {
        m_Shattering = GetComponent<ShatteringScript>();
        m_Dead = true;

        float newVol = Random.Range(m_Volumes - 0.1f, m_Volumes);

        if (OnDeath != null)
        {
            OnDeath();
        }

        m_Shattering.ShatterEffect();
        AudioScript.m_Audio.PlaySoundFx(m_DeathClip, newVol);
        GameObject.Destroy(gameObject);
    }

    protected void Die()
    {
        m_Shattering = GetComponent<ShatteringScript>();

        m_Dead = true;

        if(OnDeath != null)
        {
            OnDeath();
        }

        m_Shattering.ShatterEffect();
        float newVol = Random.Range(m_Volumes - 0.1f, m_Volumes);
        AudioScript.m_Audio.PlaySoundFx(m_DeathClip, newVol);
        GameObject.Destroy(gameObject);

    }

    protected void BossDeath()
    {
        m_Shattering = GetComponent<ShatteringScript>();
        m_Dead = true;

        if (OnDeath != null)
        {
            OnDeath();
        }

        m_Shattering.ShatterEffect();
        float newVol = Random.Range(m_Volumes - 0.1f, m_Volumes);
        AudioScript.m_Audio.PlaySoundFx(m_DeathClip, newVol);
        GameObject.Destroy(gameObject);
    }

    [ContextMenu("Self Destruct")]
    protected void PlayerDeath()
    {
        m_Shattering = GetComponent<ShatteringScript>();

        if (OnDeath != null)
        {
            OnDeath();
        }
        m_Shattering.ShatterEffect();
        float newVol = Random.Range(m_Volumes - 0.1f, m_Volumes);
        AudioScript.m_Audio.PlaySoundFx(m_DeathClip, newVol);

        

        GameObject.Destroy(gameObject);
    }
}
