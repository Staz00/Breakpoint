using UnityEngine;
using System.Collections;

public class Shatter : MonoBehaviour {

    private Collider m_Collider;
    private Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        StartCoroutine(BecomeTrigger());
        Destroy(this.gameObject, 5f);
    }

    IEnumerator BecomeTrigger()
    {
        yield return new WaitForSeconds(0.5f);

        m_Rigidbody.isKinematic = true;
        m_Collider.isTrigger = true;
        
    }
}
