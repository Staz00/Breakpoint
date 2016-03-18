using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity {

    [Header("Health & BP UI")]
    public RectTransform healthTransform;
    public RectTransform breakpointTransform;
    public Image visualHpBar;
    public Image visualBpBar;
    
    public Canvas m_userInterfaceCanvas;

    [Header("UI Attributes")]
    public int maxBp;
    public int m_DebuggerBPCost;

    [Header("SoundFx")]
    public AudioClip m_PickUpFx;
    [Range(0,1)]
    public float m_Volume = 0.5f;

    private float m_cachedY;
    private float m_minXValue, m_maxXValue;
    private float m_cachedBpY;
    private float m_minBpXValue, m_maxBpXValue;

    private float m_CurrentBp;


    public float CurrentBp
    {
        get { return m_CurrentBp; }
        set { m_CurrentBp = value; }
    }
    

	protected override void Start() {

        base.Start();

        //references
        m_cachedY = healthTransform.position.y;
        m_maxXValue = healthTransform.position.x;
        m_minXValue = healthTransform.position.x - healthTransform.rect.width * m_userInterfaceCanvas.scaleFactor;

        m_cachedBpY = breakpointTransform.position.y;
        m_maxBpXValue = breakpointTransform.position.x;
        m_minBpXValue = breakpointTransform.position.x - breakpointTransform.rect.width * m_userInterfaceCanvas.scaleFactor;
        m_CurrentBp = maxBp;
	}
	
	void Update () {
        HandleHealth();


        //regeneration per second
        if (CurrentBp < 100)
            CurrentBp += Time.deltaTime * 2;

        if (m_Health < 100)
            m_Health += Time.deltaTime;


        //check if player is dead
        if(m_Health <= 0 && !m_Dead)
        {
            base.Die();           
        }


        //makes sure that the player's health does not exceed the starting health
        if (m_Health > m_StartingHealth)
            m_Health = m_StartingHealth;

        //makes sure that the player's breakpoint does not exceed the starting breakpoint
        if (m_CurrentBp > 100)
            m_CurrentBp = 100;

        HandleMana();
	}

    void HandleHealth()
    {
        //get the current x value based on the current health value
        float currentXValue = MapValues(m_Health, 0, m_StartingHealth, m_minXValue, m_maxXValue);

        //move the health UI to its correct position
        healthTransform.position = Vector3.Lerp(healthTransform.position, new Vector3(currentXValue, m_cachedY), Time.deltaTime);

        //change the colour of the health UI
        if (m_Health > m_StartingHealth / 2)
        {
            visualHpBar.color = new Color32((byte)MapValues(m_Health, m_StartingHealth / 2, m_StartingHealth, 255, 0), 255, 0, 255);
        }
        else
        {
            visualHpBar.color = new Color32(255, (byte)MapValues(m_Health, 0, (int)m_StartingHealth / 2, 0, 255), 0, 255);
        }

    }

    void HandleMana()
    {
        //get the current x value based on the current breakpoint value
        float currentBpValue = MapValues(m_CurrentBp, 0, maxBp, m_minBpXValue, m_maxBpXValue);

        //move the breakpoint UI to its correct position
        breakpointTransform.position = Vector3.Lerp(breakpointTransform.position, new Vector3(currentBpValue, m_cachedBpY), Time.deltaTime);
    }


    //if player enters a trigger called ShatteredPieces
    //then add health and breakpoint
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ShatteredPieces"))
        {
            float newVolume = Random.Range(m_Volume - 0.1f, m_Volume);
            AudioScript.m_Audio.PlaySoundFx(m_PickUpFx, newVolume);

            if(m_Health < m_StartingHealth)
            {
                m_Health += 8;

                if (m_Health >= 100)
                    m_Health = m_StartingHealth;
            }


            if (m_CurrentBp < 100)
            {
                m_CurrentBp += 5;

                if(m_CurrentBp >= maxBp)
                    m_CurrentBp = maxBp;
            }

                


            Destroy(other.gameObject);
        }
    }


    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
