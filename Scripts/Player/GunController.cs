using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

    public Transform m_WeaponHolder;
    public Gun m_StartingGun;

    Gun m_EquippedGun;

    void Start()
    {
        if(m_StartingGun != null)
        {
            EquipGun(m_StartingGun);
        }
    }

    public void EquipGun(Gun gunToEquip)
    {
        if(m_EquippedGun != null)
        {
            Destroy(m_EquippedGun.gameObject);
        }

        m_EquippedGun = Instantiate(gunToEquip, m_WeaponHolder.position, m_WeaponHolder.rotation) as Gun;
        m_EquippedGun.transform.parent = m_WeaponHolder;

    }

    public void Shoot()
    {

        if(m_EquippedGun != null)
        {
            m_EquippedGun.Shoot();
        }
    }
}
