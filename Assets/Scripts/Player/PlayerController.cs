using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHitObj
{
    [HideInInspector]
    public Vector3 direcao;
    private Status status;
    private Animate animate;
    private PlayerMovement movement;
    private CharacterController controller;
    private GameManager gameManager;
    private UIControl scriptUI;
    private bool isRunning, isDefending, isAttacking;

    [Header("Particles")]
        public ParticleSystem fxAttack1;
        public ParticleSystem fxAttack2;
       

    [Header("Attack Settings")]
        public int swordDamage;
        public int axeDamage;
        private int damage;
        public Transform hitBox;
        [Range(0.2f,1f)]
        public float hitRange;
        public LayerMask hitMask;
        public Collider[] hitInfo;
    
    [Header("Damage Settings")]
        public int damageTaken;
        public int shieldDamageTaken;
    
    [Header("Weapons")]
        public GameObject sword;
        public GameObject axe;
        private int activeWeapon;

    
    void Start()
    {
        status = GetComponent<Status>();
        animate = GetComponent<Animate>();
        movement = GetComponent<PlayerMovement>();

        gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        scriptUI = GameObject.FindObjectOfType(typeof(UIControl)) as UIControl;

        controller = GetComponent<CharacterController>();

        status.health = status.totalHealth;
        scriptUI.AtualizaSliderHP(status.totalHealth);
        status.stamina = status.totalStamina;
        scriptUI.AtualizaSliderStamina(status.totalStamina);

        activeWeapon = 1;
    }

    
    void Update()
    {

        if(gameManager.gameState != GameState.GAMEPLAY)
        {
            return;
        }

        Inputs();
        movement.MovementModeManager();

    }

    void OnTriggerEnter(Collider obj)
    {
        if(obj.gameObject.tag == "TakeDamage")
        {
            if(movement.GetMovementMode() == PlayerMovement.MovementMode.Defending)
            {
                GetHit(shieldDamageTaken);
            }
            else
            {
                GetHit(damageTaken);
            }
        }
    }
    



    //=================== MY METHODS ===================

    void Inputs()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        direcao = new Vector3(horizontal, 0f, vertical).normalized;

        movement.Move(direcao);

        if(status.stamina<=0)
        {
            movement.SetMovementeMode(PlayerMovement.MovementMode.Walking);
            isRunning = false;
        }

        if(Input.GetButtonDown("RightBumper"))
        {
            ToggleRun();  
            if(isDefending)
            {
                movement.SetMovementeMode(PlayerMovement.MovementMode.Walking);
                isDefending = false;
            }          
        }

        if(Input.GetButtonDown("Attack"))
        {
            Attack();
        }

        if(Input.GetButtonDown("Defend(LB)"))
        {
            Defend();
            if(isRunning)
            {
                isRunning = false;
            }
        }

        if(Input.GetButtonDown("YButton"))
        {
            ChangeWeapon();
        }

        PlaySound();
    }
    void Attack()
    {
        //animate.Attacks();

        hitInfo = Physics.OverlapSphere(hitBox.position,hitRange, hitMask);
        if(activeWeapon == 1)
        {
            damage = swordDamage;
        } 
        else if(activeWeapon == 2)
        {
            damage = axeDamage;
        }

        if(hitInfo.Length!=0)
        {
            foreach(Collider c in hitInfo)
            {
                //c.gameObject.SendMessage("GetHit", damage, SendMessageOptions.DontRequireReceiver);
                if(c.gameObject.tag == "Grass")
                {
                    animate.CutGrass();
                }
                else if(c.gameObject.tag == "Tree")
                {
                    if(activeWeapon == 1)
                    {
                        damage = 0;
                    }
                    else if(activeWeapon == 2)
                    {
                        damage = 10;
                    }

                    animate.Attacks();
                }
                else
                {
                    animate.Attacks();
                }

                c.gameObject.SendMessage("GetHit", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            animate.Attacks();
        }
        
        
    }

    void ToggleRun()
    {
        if(movement.GetMovementMode() != PlayerMovement.MovementMode.Running && status.stamina>0)
        {
            if(isDefending)
            {
                isDefending = false;
                animate.Defending(false);
            }
            else
            {
                movement.SetMovementeMode(PlayerMovement.MovementMode.Running);
                isRunning = true;
            }
        }
        else
        {
            movement.SetMovementeMode(PlayerMovement.MovementMode.Walking);
            isRunning = false;
        }
    }

    void PlaySound()
    {
        if(movement.GetMovementMode() == PlayerMovement.MovementMode.Running)
        {
            GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    void Defend()
    {
        if(movement.GetMovementMode()!= PlayerMovement.MovementMode.Defending)
        {
            isDefending = true;
            scriptUI.AtivarImagem(true);
            movement.SetMovementeMode(PlayerMovement.MovementMode.Defending);
            animate.Defending(true);
        }
        else
        {
            isDefending = false;
            scriptUI.AtivarImagem(false);
            movement.SetMovementeMode(PlayerMovement.MovementMode.Walking);
            animate.Defending(false);
        }   
    }

    void OnDrawGizmosSelected()
    {
        if(hitBox!=null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hitBox.position,hitRange);
        }
        
    }

    public void GetHit(int dano)
    {
        status.health-=dano;
        if(status.health>0)
        {
            animate.GetHit();
            scriptUI.AtualizaSliderHP(status.health);
        }
        else
        {
            animate.Die();
            gameManager.ChangeGameState(GameState.GAMEOVER);
            scriptUI.AtualizaSliderHP(status.health);
        }
    }

    void ChangeWeapon()
    {
        if(activeWeapon == 1) //1 = sword
        {
            //change to AXE
            sword.SetActive(false);
            axe.SetActive(true);
            scriptUI.fundoSword.gameObject.SetActive(false);
            activeWeapon = 2;
        }
        else if(activeWeapon == 2) //2 = axe
        {
            //change to SWORD
            axe.SetActive(false);
            sword.SetActive(true);
            scriptUI.fundoSword.gameObject.SetActive(true);
            activeWeapon = 1;
        }
    }
    




}
 