using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

    [Header("UI Components")]
    public GameObject m_Debugger;
    public GameObject m_Output;
    public GameObject m_Term;
    public GameObject m_Menu;

    [Header("Input Fields")]
    public InputField m_AttackInput;
    public InputField m_HealthInput;
    public InputField m_Agressiveness;
    public InputField m_DurationInput;
    public InputField m_CerebInput;

    [Header("Text Fields")]
    public Text m_AttackText;
    public Text m_HPText;
    public Text m_AgroText;
    public Text m_DurationText;
    public Text m_CerebText;
    public Text m_ErrorText;

    //only for debugging
    public Text m_GodModeText;
    public RaycastHit hit;

    [Header("Player")]
    public GameObject m_Player;

    [Header("Debugging Attributes")]
    public float m_DebugCost = 5f;
    public float m_CompileCost = 10f;


    [Header("SoundFx")]
    public AudioClip m_Select;
    [Range(0, 1)]
    public float m_Volume = 0.5f;

    private Animator m_Anim;
    private TerminalScript m_Terminal;
    private EnemyMovement m_Enemy;
    private StatePatternEnemy m_EnemyState;
    private HiddenPathEnemy m_HiddenEnemy;

    private float m_currentEnemyHp;
    private float m_currentAttackDmg;
    private float m_currentDuration;
    private bool isAgro;

	void Start () {
        m_Debugger.SetActive(false);
        m_Output.SetActive(false);
        m_Term.SetActive(false);
        m_Menu.SetActive(false);

        //only for debugging
        m_GodModeText.enabled = false;
	}

    void Update()
    {
        //Only execute if hit.collider has a reference
        if (hit.collider != null)
        {
            //checks if EnemyMovement reference is not null, if it is then use HiddenPathEnemy reference
            if(hit.collider.name.Contains("Hidden"))
            {
                m_HiddenEnemy = hit.collider.GetComponent<HiddenPathEnemy>();

                m_HPText.text = m_HiddenEnemy.m_Health.ToString();
                m_AgroText.text = m_HiddenEnemy.isAggressive.ToString();
                m_DurationText.text = m_HiddenEnemy.Duration.ToString();
                m_CerebText.text = m_HiddenEnemy.m_Cereb.ToString();
            }

            else
            {
                m_Enemy = hit.collider.GetComponent<EnemyMovement>();
                m_AgroText.text = m_Enemy.isAggressive.ToString();
                m_HPText.text = m_Enemy.m_Health.ToString();
                m_CerebText.text = m_Enemy.m_Cereb.ToString();
            }

            m_EnemyState = hit.collider.GetComponent<StatePatternEnemy>();
        }
    }
	
    public void OpenMenu()
    {
        Time.timeScale = 0;
        m_Menu.SetActive(true);

        //find ref to the player class
        PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if (player != null)
        {
            player.IsPaused = true;
        }
            
    }

    public void OnRestartLevelPress()
    {
        m_Menu.SetActive(false);
        Application.LoadLevel(Application.loadedLevel);
    }

    public void OnCloseMenuPress()
    {
        Time.timeScale = 1;
        m_Menu.SetActive(false);

        //find ref to the player class
        PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if(player != null)
            player.IsPaused = false;
    }

    public void OnQuitToMenuPress()
    {
        m_Menu.SetActive(false);

        LoadingScreen loadingScreen = FindObjectOfType(typeof(LoadingScreen)) as LoadingScreen;

        loadingScreen.levelToLoad = "MainMenu";
        loadingScreen.StartPlaying();
    }

    public void OpenDebugger()
    {
        PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if (player != null)
        {
            player.IsPaused = true;
        }

        if (hit.collider != null)
        {
            m_Enemy = hit.collider.GetComponent<EnemyMovement>();
            m_EnemyState = hit.collider.GetComponent<StatePatternEnemy>();

            //checks if EnemyMovement reference is not null, if it is then use HiddenPathEnemy reference
            if (m_Enemy == null)
            {
                m_HiddenEnemy = hit.collider.GetComponent<HiddenPathEnemy>();

                m_HPText.text = m_HiddenEnemy.m_Health.ToString();
                m_AgroText.text = m_HiddenEnemy.isAggressive.ToString();
                m_DurationText.text = m_HiddenEnemy.Duration.ToString();
                m_CerebText.text = m_HiddenEnemy.m_Cereb.ToString();
            }
            else
            {
                m_HPText.text = m_Enemy.m_Health.ToString();
                m_AgroText.text = m_Enemy.isAggressive.ToString();
                m_DurationText.text = m_Enemy.Duration.ToString();
                m_CerebText.text = m_Enemy.m_Cereb.ToString();
            }

            m_AttackText.text = m_EnemyState.attackDamage.ToString();
            m_Debugger.SetActive(true);
        }
    }

    public void CloseDebugger()
    {
        PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if (player != null)
        {
            player.IsPaused = false;
        }

        //PlaySound();
        m_Debugger.SetActive(false);
        m_ErrorText.text = "";
    }

    public void OpenOutput()
    {
        PlaySound();

        if (m_Player.GetComponent<PlayerHealth>().CurrentBp >= m_DebugCost)
        {
            m_Player.GetComponent<PlayerHealth>().CurrentBp -= m_DebugCost;

            m_Player.GetComponent<Animator>().SetBool("IsDebugging", true);
            
            //Only execute if hit.collider has a reference
            if(hit.collider != null)
            {
                m_Enemy = hit.collider.GetComponent<EnemyMovement>();
                m_EnemyState = hit.collider.GetComponent<StatePatternEnemy>();

                //checks if EnemyMovement reference is not null, if it is then use HiddenPathEnemy reference
                if (m_Enemy == null)
                {
                    m_HiddenEnemy = hit.collider.GetComponent<HiddenPathEnemy>();

                    m_CerebInput.interactable = false; //player is unable to change wcereb value if fighting a hidden enemy

                    m_currentEnemyHp = m_HiddenEnemy.m_Health;
                    m_currentDuration = m_HiddenEnemy.Duration;
                    m_CerebInput.text = m_HiddenEnemy.m_WCereb.ToString();
                    m_Agressiveness.text = m_AgroText.text;

                    if (m_HiddenEnemy.m_Health > m_HiddenEnemy.m_StartingHealth / 2)
                    {
                        m_HealthInput.interactable = false;

                        m_ErrorText.text = "Enemy's Health must be below or equal to its total HP divided by 2 to change HP and Cereb value";
                    }
                    else if (m_HiddenEnemy.m_Health <= m_HiddenEnemy.m_StartingHealth / 2)
                    {
                        m_HealthInput.interactable = true;
                    }
                }
                else
                {
                    m_currentEnemyHp = m_Enemy.m_Health;
                    m_currentDuration = m_Enemy.Duration;
                    m_CerebInput.text = m_Enemy.m_WCereb.ToString();
                    m_Agressiveness.text = m_AgroText.text;

                    if (m_Enemy.m_Health > m_Enemy.m_StartingHealth / 2)
                    {
                        m_HealthInput.interactable = false;
                        m_CerebInput.interactable = false;

                        m_ErrorText.text = "Enemy's Health must be below or equal to its total HP divided by 2 to change HP and Cereb value";
                    }
                    else if (m_Enemy.m_Health <= m_Enemy.m_StartingHealth / 2)
                    {
                        m_HealthInput.interactable = true;
                        m_CerebInput.interactable = true;
                    }
                }

                m_currentAttackDmg = m_EnemyState.attackDamage;

                m_Player.GetComponent<PlayerMovement>().DisableMove = true;
                hit.collider.GetComponentInChildren<SphereCollider>().enabled = true;
                hit.collider.GetComponent<Animator>().enabled = false;
                hit.collider.GetComponent<StatePatternEnemy>().enabled = false;

                m_AttackInput.text = m_currentAttackDmg.ToString();
                m_HealthInput.text = m_currentEnemyHp.ToString();
                m_DurationInput.text = m_currentDuration.ToString();

                m_Output.SetActive(true);

            }
        }
        
    }

    public void CloseOutput()
    {
        PlaySound();

        PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if (player != null)
        {
            player.IsPaused = false;
        }

        m_Output.SetActive(false);

        m_Player.GetComponent<Animator>().SetBool("IsDebugging", false);
        m_Player.GetComponent<PlayerMovement>().DisableMove = false;
        hit.collider.GetComponentInChildren<SphereCollider>().enabled = true;
        hit.collider.GetComponent<Animator>().enabled = true;
        hit.collider.GetComponent<StatePatternEnemy>().enabled = true;
        
    }

    public void OnCompilePress()
    {
        PlaySound();
        m_Enemy = hit.collider.GetComponent<EnemyMovement>();
        m_EnemyState = hit.collider.GetComponent<StatePatternEnemy>();

        if (m_Player.GetComponent<PlayerHealth>().CurrentBp >= m_CompileCost)
        {
            m_Player.GetComponent<PlayerHealth>().CurrentBp -= m_CompileCost;

            //Change string to bool
            if (m_Agressiveness.text == "True" || m_Agressiveness.text == "true")
            {
                isAgro = true;
            }
            else if (m_Agressiveness.text == "False" || m_Agressiveness.text == "false")
            {
                isAgro = false;
            }

            //Change Damage of the targeted enemy
            m_EnemyState.attackDamage = System.Int32.Parse(m_AttackInput.text);

            //checks if EnemyMovement reference is not null, if it is then use HiddenPathEnemy reference
            if (m_Enemy == null)
            {
                m_HiddenEnemy = hit.collider.GetComponent<HiddenPathEnemy>();
                m_HiddenEnemy.m_Health = System.Int32.Parse(m_HealthInput.text);
            }
            else
            {
                m_Enemy.m_Health = System.Int32.Parse(m_HealthInput.text);
            }
            
            //Change enemy status
            if (isAgro == false)
                m_EnemyState.currentState = m_EnemyState.passiveState;

            if (m_DurationInput.text != "")
                m_EnemyState.duration = float.Parse(m_DurationInput.text);

            //Normalization of enemy via UI changing wcereb values and matching it with the original
            if (m_CerebText.text == m_CerebInput.text)
            {
                m_Terminal = FindObjectOfType(typeof(TerminalScript)) as TerminalScript;

                if(m_Terminal != null && m_Terminal.isActiveAndEnabled)
                {
                    m_Terminal.GetComponent<TerminalScript>().m_Saved++; //Only add if terminal is found
                    m_Terminal.GetComponent<TerminalScript>().m_TotalCount--;
                }
                    

                hit.collider.GetComponent<CapsuleCollider>().enabled = false;
                m_EnemyState.currentState = m_EnemyState.neutralState;
            }

            //reset enemy and player scripts
            hit.collider.GetComponentInChildren<SphereCollider>().enabled = true;
            hit.collider.GetComponent<Animator>().enabled = true;
            hit.collider.GetComponent<StatePatternEnemy>().enabled = true;
            m_Player.GetComponent<PlayerMovement>().DisableMove = false;

            m_AttackText.text = m_EnemyState.attackDamage.ToString();

            //checks if EnemyMovement reference is not null, if it is then use HiddenPathEnemy reference
            if(m_Enemy == null)
            {
                m_HiddenEnemy = hit.collider.GetComponent<HiddenPathEnemy>();

                m_HPText.text = m_HiddenEnemy.m_Health.ToString();
                m_AgroText.text = m_HiddenEnemy.isAggressive.ToString();
                m_DurationText.text = m_HiddenEnemy.Duration.ToString();
                m_CerebText.text = m_HiddenEnemy.m_Cereb.ToString();
            }
            else
            {
                m_HPText.text = m_Enemy.m_Health.ToString();
                m_AgroText.text = m_Enemy.isAggressive.ToString();
                m_DurationText.text = m_Enemy.Duration.ToString();
                m_CerebText.text = m_Enemy.m_Cereb.ToString();
            }
            
            CloseDebugger();
            CloseOutput();
        }
        
    }
    private void PlaySound()
    {
        AudioScript.m_Audio.PlaySoundFx(m_Select, m_Volume);
    }
}
