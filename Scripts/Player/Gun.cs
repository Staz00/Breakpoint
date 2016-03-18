using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    [Header("Gun Attributes")]
    public Transform m_Muzzle;
    public Projectile m_Projectile;
    public float rateOfFire = 0.1f;
    public float muzzleVelocity = 35f;
    public float shootCost = 4f;

    [Header("SoundFx")]
    public AudioClip m_ShootClip;
    public AudioClip m_LootSound;
    [Range(0, 1)]
    public float m_Volume = .5f;

    PlayerHealth m_PlayerHealth;
    float m_NextShotTime;

    void Start()
    {
        m_PlayerHealth = GetComponentInParent<PlayerHealth>();
    }

    public void Shoot()
    {
        //only shoot if the current time is bigger than the shoot rate
        if (Time.time > m_NextShotTime)
        {
            //only allow shoot if player still has enough BP
            if (m_PlayerHealth.CurrentBp >= 0)
            {
                float newVolume = Random.Range(m_Volume - 0.1f, m_Volume);
                AudioScript.m_Audio.PlaySoundFx(m_ShootClip, newVolume);

                //deduct bp
                m_PlayerHealth.CurrentBp -= shootCost;

                //reset the next shot time
                m_NextShotTime = Time.time + rateOfFire;

                //spawn a projectile
                Projectile newProjectile = Instantiate(m_Projectile, m_Muzzle.position, m_Muzzle.rotation) as Projectile;
                
                //set the projectile speed
                newProjectile.SetSpeed(muzzleVelocity);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioScript.m_Audio.PlaySoundFx(m_LootSound, m_Volume);

            other.GetComponent<GunController>().EquipGun(this.GetComponent<Gun>());
            other.GetComponent<PlayerMovement>().hasWeapon = true;
            Destroy(gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
