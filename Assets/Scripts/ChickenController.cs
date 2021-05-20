using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChickenController : MonoBehaviour
{
    private Animate animate;
    private GameManager gameManager;

    private NavMeshAgent agent;
    private Vector3 destination;
    public ChickenState state;

    void Start()
    {
        ChangeState(state);
        gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    void Uptade()
    {
        StateManager();

        if(agent.desiredVelocity.magnitude>=0.1)
        {
            animate.Walk(true);
        }
        else
        {
            animate.Walk(false);
        }
    }
    void StateManager()
    {
        switch(state)
        {
            case ChickenState.IDLE:
                //comportamento em idle;
                break;
        }
    }

    void ChangeState( ChickenState newState)
    {
        StopAllCoroutines();
        switch(state)
        {
            case ChickenState.IDLE:
                StartCoroutine("IDLE");
                break;
            
            case ChickenState.EAT:
                GetComponent<Animator>().SetTrigger("Eat");
                StartCoroutine("EAT");
                break;

            case ChickenState.TURNHEAD:
                GetComponent<Animator>().SetTrigger("TurnHead");
                StartCoroutine("TURNHEAD");
                break;

            case ChickenState.WALK:
                int idWayPoint = Random.Range(0,gameManager.chickenWayPoints.Length);
                destination = gameManager.chickenWayPoints[idWayPoint].position;    
                agent.stoppingDistance = 0;
                agent.destination = destination;

                StartCoroutine("WALK");
                
                break;
        }
    }


    void StayStill(int yes) //calcula se ficara no mesmo lugar ou se vai andar 
    {
        if(Random.Range(0,100)<yes)
        {
            DoSomething(80,50);
        }
        else
        {
            print("state changed to WALK");
            ChangeState(ChickenState.WALK);
        }
    }

    void DoSomething(int yes, int eat)
    {
        if(Random.Range(0,100)<yes) // calcula se ira fazer algo, se for menor que o valor de sim, fara algo, senao, voltara para idle
        {
            if(Random.Range(0,100)<eat) // calcula se ira comer ou virar a cabeca, se for menor que o valor de eat ira comer, senao ira virar a cabeca
            {
                print("state changed to EAT");
                ChangeState(ChickenState.EAT);
            }
            else
            {
                print("state changed to TURNHEAD");
                ChangeState(ChickenState.TURNHEAD);    
            }
        }
        else
        {
            print("state changed to IDLE");
            ChangeState(ChickenState.IDLE);
        }
    }

    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(5);
        StayStill(10);
    }

    IEnumerator EAT()
    {
        yield return new WaitForSeconds(gameManager.chickenIdleWaitTime);
        StayStill(20);
    }

    IEnumerator TURNHEAD()
    {
        yield return new WaitForSeconds(gameManager.chickenIdleWaitTime);
        StayStill(20);
    }

    IEnumerator WALK()
    {
        yield return new WaitUntil(()=> agent.remainingDistance<=0);
        StayStill(30);
    }
}
