using UnityEngine;
using System.Collections;

public class WeaponLoot : MonoBehaviour {

    public AudioClip m_LootSound;
    [Range(0, 1)]
    public float m_Volume = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioScript.m_Audio.PlaySoundFx(m_LootSound, m_Volume);
            other.GetComponent<GunController>().EquipGun(this.GetComponent<Gun>());
            other.GetComponent<PlayerMovement>().hasWeapon = true;
            Destroy(gameObject);
        }
    }
}
