using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SlimeController : MonoBehaviour, IHitObj
{
    private Animate animate;
    private Status status;
    private NPCMovement movement;
    bool isDead;
    private Rigidbody myRb;
    private GameManager gameManager;
    private bool isNearPlayer;

    [Header("Particles and Others")]
        public ParticleSystem fxBlood;
        public GameObject canvasHP;

    [Header("Vars")]
        private int rand;
        public float walkRadius;
    
    [Header("UI")]
        public Slider sliderHP;
        public Image imageSlider;
        public Color minColor, maxColor;
        
    public EnemyState state; 
    private Vector3 lookDirection;
    

    //IA
    private NavMeshAgent agent;
    private Vector3 destination;
    private Vector3 lastPlayerPosition;
    private int idWayPoint;
    private bool isAlert;
    private bool isPlayerVisible;
    public bool isAttacking;

    void Start()
    {
        ChangeState(state); //comeca no estado definido no inspector

        gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        animate = GetComponent<Animate>();
        status = GetComponent<Status>();
        movement = GetComponent<NPCMovement>();
        myRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        
        status.health = status.totalHealth;
        sliderHP.maxValue = status.totalHealth;
        AtualizaUI();
   
    }

    
    void Update()
    {
        StateManager();

        //animations
        
        if(agent.desiredVelocity.magnitude>=0.1)
        {
            animate.Walk(true);
        }
        else
        {
            animate.Walk(false);
        }

        animate.Alert(isAlert);

        //show health bar
        if(IsNearPlayer())
        {
            canvasHP.SetActive(true);
        }
        else
        {
            canvasHP.SetActive(false);
        }
    
    }

    void OnTriggerEnter(Collider obj)
    {

        if(gameManager.gameState != GameState.GAMEPLAY) {   return;}

        if(obj.gameObject.tag == "Player")
        {
            isPlayerVisible = true;

            if(state == EnemyState.IDLE || state == EnemyState.PATROL)
            {
                ChangeState(EnemyState.ALERT);
            }
            
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if(obj.gameObject.tag == "Player")
        {
            isPlayerVisible = false;
        }
    }

    //============= MY METHODS =============

    private bool IsNearPlayer()
    {
        float distancia = Vector3.Distance(transform.position, gameManager.player.position);
        if(distancia<6)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void AtualizaUI()
    {
        sliderHP.value = status.health;
        float porcentagemHP = (float)status.health/status.totalHealth;

        Color corHP = Color.Lerp(minColor, maxColor, porcentagemHP);
        imageSlider.color = corHP;
    }
    public Vector3 RandomPosition()
    {
        Vector3 position = Random.insideUnitSphere*20;
        position.y = transform.position.y;
        position += transform.position;
        return position;
    }

    public void GetHit(int dano)
    {
        if(isDead) {return; }
       
        fxBlood.Emit(5);
        status.health-=dano;
        AtualizaUI();
       
        if(status.health > 0)
        {
            ChangeState(EnemyState.FURY);
            animate.GetHit();
        }
        else
        {
            ChangeState(EnemyState.DEAD);
            StopAllCoroutines();
            //animate.Die();
            Die();      
            StartCoroutine("WaitToDestroy");      
        } 
    }

    /*IEnumerator HasDied()
    {
        isDead = true;
        agent.speed = 0;
        animate.Die();

        yield return new WaitForSeconds(5);

        GetComponent<Collider>().enabled = false;
        Destroy(gameObject);
    } */

    void Die()
    {
        
        GetComponent<Collider>().enabled = false;
        animate.Die();
        fxBlood.Emit(10);

    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(2);
        if(gameManager.Perc(gameManager.percDrop))
        {
            Instantiate(gameManager.ruby, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    void StateManager()
    {

        if(gameManager.gameState == GameState.GAMEOVER && (state == EnemyState.FOLLOW || state == EnemyState.FURY || state == EnemyState.ALERT))
        {
            ChangeState(EnemyState.IDLE);
        }


        switch(state)
        {        
            case EnemyState.PATROL:
                //comportamento em patrol
                break;

            case EnemyState.SEARCH:
                break;

            //--------------------------------------------------------
            case EnemyState.FURY:
                LookAt();
                destination = gameManager.player.position;
                agent.destination = destination;
                /*lookDirection = gameManager.player.position - transform.position;
                movement.Rotate(lookDirection); */
            
                if(agent.remainingDistance<=agent.stoppingDistance)
                {
                    Attack();
                }

                break;

            //--------------------------------------------------------
            case EnemyState.FOLLOW:
                LookAt();
                destination = gameManager.player.position;
                agent.destination = destination;
                if(agent.remainingDistance<=agent.stoppingDistance)
                {
                    Attack();
                }
                break;

            //--------------------------------------------------------
            case EnemyState.ALERT:
                LookAt();
                break;
        }   
    }

    void ChangeState(EnemyState newState)
    {
        StopAllCoroutines(); //antes de iniciar uma coroutine, é necessario parar todas as outras

        switch(newState)
        {
            case EnemyState.IDLE:   
                StartCoroutine("IDLE");
                break;

            
            case EnemyState.ALERT:
                lastPlayerPosition = gameManager.player.position;
                destination = transform.position;
                agent.SetDestination(destination); 
                isAlert = true;
                StartCoroutine("ALERT");
                break;
            

            case EnemyState.PATROL:
                idWayPoint = Random.Range(0,gameManager.slimeWayPoints.Length);
                destination = gameManager.slimeWayPoints[idWayPoint].position;
                agent.stoppingDistance = 0;
                agent.destination = destination;

                StartCoroutine("PATROL");
                break;


            case EnemyState.FURY:
                destination = transform.position;
                agent.stoppingDistance = gameManager.slimeDistanceToAttack;
                agent.destination = transform.position;
                break;


            case EnemyState.SEARCH:
                destination = lastPlayerPosition;
                agent.destination = destination;
                animate.Search();
                StartCoroutine("SEARCH");
                break;

            
            case EnemyState.FOLLOW:
                agent.stoppingDistance = gameManager.slimeDistanceToAttack;
                StartCoroutine("FOLLOW");
                break;

            case EnemyState.DEAD:
                destination = transform.position;
                agent.destination = destination;
                break;
        }

        state = newState;
    }

    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(3); //fica parado por (idleWaitTime) segundos

        StayStill(50); //50% chance de ficar parado, 50% chance de entrar em patrulha
    }

    IEnumerator PATROL()
    {
        yield return new WaitUntil(()=> agent.remainingDistance<=0);

        StayStill(30); //30% chance de ficar parado, 70% chance de continuar em patrulha
    }

    IEnumerator ALERT()
    {
        yield return new WaitForSeconds(gameManager.slimeAlertWaitTime);

        if(isPlayerVisible)
        {
            ChangeState(EnemyState.FOLLOW);
            isAlert = false;
        }
        else
        {
            ChangeState(EnemyState.SEARCH);
            isAlert = false;
        }
    }

    IEnumerator SEARCH()
    {
        //yield return new WaitUntil(()=> Vector3.Distance(transform.position, lastPlayerPosition)<=0);
        //animate.Search();
        //yield return new WaitForSeconds(3);

        if(isPlayerVisible)
        {
            ChangeState(EnemyState.FOLLOW);
        }
        else
        {
            yield return new WaitForSeconds(6);
            StayStill(10);
        }
        
    }
    
    IEnumerator FOLLOW()
    {
        yield return new WaitUntil(()=> !isPlayerVisible);
        
        yield return new WaitForSeconds(gameManager.slimeAlertWaitTime);
        StayStill(50);

        
    }

    IEnumerator ATTACK()
    {
        yield return new WaitForSeconds(gameManager.slimeAttackCooldown);
        isAttacking = false;
    }


    //decisao se continua parado ou nao
    void StayStill(int yes) 
    {
        if(gameManager.RandomValue()<yes)
        {
            ChangeState(EnemyState.IDLE);
    
        }
        else //caso NO
        {
            ChangeState(EnemyState.PATROL); //muda para o estado de patrulha
          
        }
    }

    void Attack()
    {
        if(!isAttacking && isPlayerVisible)
        {
            isAttacking = true;
            animate.EnemyAttack();
        }
    }

    void AttackIsDone()
    {
        StartCoroutine("ATTACK");
    }

    public void LookAt()
    {   
        Vector3 lookDirection = (gameManager.player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, gameManager.lookAtSpeed*Time.deltaTime);
    }




    


}
