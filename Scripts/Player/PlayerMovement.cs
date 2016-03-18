using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GunController))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerMovement : MonoBehaviour
{

    #region Variables
    [Header("UI Properties")]
    public LayerMask mask;
    public Canvas m_EventUI;

    [Header("Player Attributes")]
	public float m_Speed = 6;
	public float m_RotationSpeed = 10;
    public float m_AttackRate = 1;
    public int m_Damage = 5;
    public bool hasWeapon = false;

    [Header("Soundfx")]
    public AudioClip footSteps;
    [Range(0, 1)]
    public float fxVolume = 1f;

    public Crosshairs crosshairs;
    
	private Rigidbody playerRigidbody;
	private Vector3 m_Movement;

    public UIControl m_UI;
	private Vector3 m_PreviousLoc;
	private Vector3 m_CurrentLoc;
    private Vector3 m_TargetPos;
    private Vector3 m_TargetEnemy;
	private Animator m_Anim;
    private RaycastHit m_hit;
    
    private Ray m_Ray;
    private RaycastHit m_MouseHit;
	private bool moveDisabled;
    private GunController m_gunController;
    private float m_timer;
    private bool m_isFiring;
    private float m_walkTimer = 0.3f;
    private bool m_GodMode = false;
    private bool m_NeedsCursor = false;
    //only for testing (cheat mode)
    private int m_originalDamage;

    public bool DisableMove
    {
        set { moveDisabled = value; }
    }

    public bool IsPaused
    {
        set { m_NeedsCursor = value; }
    }


    Vector3 velocity;
    #endregion


    void Awake()
	{
		playerRigidbody = GetComponent<Rigidbody>();
        m_gunController = GetComponent<GunController>();
		m_Anim = GetComponent<Animator> ();

        moveDisabled = false;

        hasWeapon = false;
        m_EventUI.enabled = false;
        
	}

    void Start()
    {
        m_NeedsCursor = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        //convert mouse position from screen space to world space;
        Vector3 mouseToWorldPos = Input.mousePosition;

        //add an offset to the Z axis of the camera position
        mouseToWorldPos.z = 20f;

        //convert mouse position from screen to world coordinates
        mouseToWorldPos = Camera.main.ScreenToWorldPoint(mouseToWorldPos);

        //assign converted position to the crosshairs current position
        crosshairs.transform.position = mouseToWorldPos;


        //Cheat code for debugging
        if (Input.GetKeyDown(KeyCode.F12) && !m_GodMode)
        {
            m_GodMode = true;
            GetComponent<CapsuleCollider>().enabled = false;
            m_originalDamage = m_Damage;
            m_Damage = int.MaxValue;

            m_UI.m_GodModeText.enabled = true;
            m_UI.m_GodModeText.text = "Cheat mode = " + m_GodMode.ToString();
        }
        else if (Input.GetKeyDown(KeyCode.F12) && m_GodMode)
        {
            m_GodMode = false;
            GetComponent<CapsuleCollider>().enabled = true;
            m_Damage = m_originalDamage;

            m_UI.m_GodModeText.enabled = false;
            m_UI.m_GodModeText.text = "Cheat mode = " + m_GodMode.ToString();
        }
        //end of cheat code


        //Opening menu
        if (Input.GetKeyDown(KeyCode.Escape) && !m_NeedsCursor)
        {
            m_NeedsCursor = true;
            m_UI.OpenMenu();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && m_NeedsCursor)
        {
            m_NeedsCursor = false;
            m_UI.OnCloseMenuPress();
        }


        //Get reference to the FpsCounter script
        FpsCounter fpsCounter = FindObjectOfType(typeof(FpsCounter)) as FpsCounter;

        //To enable/disable the fps counter on key press
        //mainly for debugging
        if (Input.GetKeyDown(KeyCode.F1) && !fpsCounter.Enabled)
        {
            fpsCounter.Enabled = true;
        }
        else if(Input.GetKeyDown(KeyCode.F1) && fpsCounter.Enabled)
        {
            fpsCounter.Enabled = false;
        }

        if (m_NeedsCursor)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    void FixedUpdate()
	{
        m_timer += Time.deltaTime;
        m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        

        m_isFiring = false;
        RaycastHit terminalHit;
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        if (GetComponent<PlayerHealth>().m_Health > 0)
        {
            GetUserInput(horizontalMovement, verticalMovement);

            //Check which game object the mouse is hovering over
            //and based on that game object, the player's attack will differ
            if (Physics.Raycast(m_Ray, out m_MouseHit, Mathf.Infinity, mask))
            {
                if ((m_MouseHit.collider.CompareTag("Enemy") || m_MouseHit.collider.CompareTag("Boss")) && Input.GetMouseButton(1) &&
                    m_timer >= m_AttackRate && m_timer != 0)
                {
                    //use the normal attack if the player is close enough
                    if (Vector3.Distance(m_MouseHit.collider.transform.position, transform.position) <= 3f)
                    {
                        RotatePlayerModel(m_MouseHit);

                        m_hit = m_MouseHit;
                        if (!m_hit.collider.CompareTag("Boss"))
                        {
                            m_UI.hit = m_MouseHit;
                        }

                        Attack();
                    }
                    //use the weapon if the player is more than 3 vector3 units from its target and the player has a weapon
                    else if (Vector3.Distance(m_MouseHit.collider.transform.position, transform.position) > 3f && hasWeapon)
                    {
                        m_isFiring = true;
                        RotatePlayerModel(m_MouseHit);

                        m_gunController.Shoot();
                    }

                }

                //open the debugger if the player press left mouse button and if the player is close enough to the target
                if (m_MouseHit.collider.CompareTag("Enemy") && Vector3.Distance(m_MouseHit.collider.transform.position, transform.position) <= 7f && Input.GetMouseButton(0))
                { 
                    m_UI.hit = m_MouseHit;
                    m_hit = m_MouseHit;

                    m_TargetEnemy = m_MouseHit.collider.transform.position;
                    bool isOpen = false;

                    //leave the window open if the player is close enough to the target
                    while (Vector3.Distance(m_TargetEnemy, transform.position) <= 4f && GetComponent<PlayerHealth>().CurrentBp > 0)
                    {
                        m_UI.OpenDebugger();
                        isOpen = true;

                        if (isOpen)
                        {
                            break;
                        }
                    }
                }
            }

            //if player is facing a terminal and player presses E key
            if (Physics.Raycast(transform.position + Vector3.up * .5f, transform.forward, out terminalHit, 2f))
            {
                
                if (terminalHit.collider.CompareTag("Terminal"))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        terminalHit.collider.GetComponent<TerminalScript>().OpenTerminal();
                    }
                }
                else if(terminalHit.collider.CompareTag("HiddenTerminal"))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                        terminalHit.collider.GetComponent<HiddenPlatformScript>().OpenTerminal();
                }
            }

            //automatically close the Debugger UI if the player is more than 3 vector3 units from the target
            if(m_TargetEnemy != null)
            {
                if (Vector3.Distance(m_TargetEnemy, transform.position) > 5f)
                {
                    if(this.enabled != false)
                        m_UI.CloseDebugger();
                }
            }
        }
        
        if(!moveDisabled)
            Animating(horizontalMovement, verticalMovement);
	}

 
    private IEnumerator WaitToFinish()
    {
        yield return new WaitForSeconds(2.5f); // waits 2.5 seconds
        moveDisabled = false;
    }

    

    #region Input&Movement
    void GetUserInput(float h, float v)
    {
        //check if animation is idle or run.
        //if this condition returns true then only then is the player allowed to move
        //useful for when opening treasure chest is implemented as it uses a different animation
        if (((m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) || (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))) && moveDisabled == false)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Move(h, v);
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                Move(h, v);
            }
        }

        ////for pick up
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        //    {
        //        m_Anim.SetTrigger("PickUp");
        //        moveDisabled = true;
        //        StartCoroutine(WaitToFinish());
        //    }
        //}
    }


    //rotate the player model towards the target
    void RotatePlayerModel(RaycastHit hit)
    {
        Vector3 targetPos = hit.collider.transform.position;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(targetPos.x, 0f, targetPos.z) - transform.position),
                                              Time.deltaTime * m_RotationSpeed + 20f);
    }

    void Attack()
    {
        //reset the attack rate timer
        m_timer = 0;
        
        //disable player movement
        moveDisabled = true;

        //set the animator
        m_Anim.SetTrigger("Attack");

        //wait some time before dealing the damage
        StartCoroutine(WaitOneSecond());
    }

    IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(0.7f);
        moveDisabled = false;

        //get reference to the interface that handles the damage system
        IDamageable damageableObject = m_hit.collider.GetComponent<IDamageable>();

        if (damageableObject != null)
        {
            if(m_hit.collider.CompareTag("Enemy"))
            {
                damageableObject.TakeHit(m_Damage, m_hit);
            }
                

            else if(m_hit.collider.CompareTag("Boss"))
            {
                damageableObject.TakeDamage(m_Damage);
            }
                
        }
    }

    //for playing soundfx
    //obsolete but could be useful for later
    private void PlayEnemyHurtSound(AudioClip clip)
    {
        float newVol = Random.Range(fxVolume - 0.1f, fxVolume);
        AudioScript.m_Audio.PlaySoundFx(clip, newVol);
    }

    //Moving the player's position vector
    void Move(float h, float v)
    {
        m_walkTimer -= Time.deltaTime;
        m_Movement.Set(h, 0, v);

        //normalize the vector, essential to avoid distance advantage when moving diagonally
        m_Movement = m_Movement.normalized * m_Speed * Time.deltaTime;

        //move the players rigidbody to the new position
        playerRigidbody.MovePosition(transform.position + m_Movement);

        //play footsteps soundfx
        if(m_walkTimer <= 0)
        {
            float newVolume = Random.Range(fxVolume - 0.25f, fxVolume);
            m_walkTimer = 0.3f;
            AudioScript.m_Audio.PlaySoundFx(footSteps, newVolume);
        }

        //only rotate when the player is not firing
        if (!m_isFiring)
        {
            m_PreviousLoc = m_CurrentLoc;
            m_CurrentLoc = transform.position;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - m_PreviousLoc),
                                                  Time.deltaTime * m_RotationSpeed);

            transform.position = m_CurrentLoc;
        }
    }

    //running and idle animation
	void Animating(float h, float v)
	{
		bool running = h != 0f || v != 0f;
        m_Anim.SetBool("IsRunning", running);
    }

    #endregion


}
