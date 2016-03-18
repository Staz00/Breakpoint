using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EventTriggerScript : MonoBehaviour {

    public Canvas m_EventUI;


    void Start()
    {
        m_EventUI.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_EventUI.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            m_EventUI.enabled = false;
    }

    void OnKillActive()
    {
        m_EventUI.enabled = false;
    }

}
