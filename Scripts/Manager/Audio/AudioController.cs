using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour {
    public static AudioController audioController;

    public AudioMixerSnapshot m_OutOfCombat;
    public AudioMixerSnapshot m_InCombat;
    public AudioMixerSnapshot m_BossCombat;

    public float bpm = 128f;

    private float m_Transition;
    private float m_TransitionOut; //time in ms to transition from audio snapshots
    private float m_QuarterNote;

    private GameObject enemy;
    private bool m_HasHitEnemy;

    void Awake()
    {
        audioController = this;
    }

    void Start () {
        m_QuarterNote = 60 / bpm;
        m_Transition = m_QuarterNote;
	}

    void Update()
    {
        if (enemy == null)
        {
            m_OutOfCombat.TransitionTo(m_Transition);
        }       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CombatTrigger"))
        {
            m_InCombat.TransitionTo(m_Transition);
            enemy = other.gameObject;
        }

        else if(other.CompareTag("BossCombatTrigger"))
        {
            m_BossCombat.TransitionTo(m_Transition);
            enemy = other.gameObject;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CombatTrigger"))
        {
            m_InCombat.TransitionTo(m_Transition);
            enemy = other.gameObject;
        }

        else if (other.CompareTag("BossCombatTrigger"))
        {
            m_BossCombat.TransitionTo(m_Transition);
            enemy = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CombatTrigger"))
        {
            m_OutOfCombat.TransitionTo(m_Transition);
        }
        else if (other.CompareTag("BossCombatTrigger"))
        {
            m_OutOfCombat.TransitionTo(m_Transition);
        }
    }
}
