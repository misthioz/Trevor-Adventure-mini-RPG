using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerScript;
    bool isAttacking;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerScript = GetComponent<PlayerController>();
    }

    public void MoveAnimations(float velocity)
    {
        animator.SetFloat("velocity", velocity);
    }

    public void Attacks()
    {
        float prob = Random.value;

        if(!isAttacking)
        {
              if(prob>=0 && prob<=0.5)
            {
                isAttacking = true;
                animator.SetTrigger("Attack1");
                playerScript.fxAttack1.Emit(1);
            }
            else if(prob>=0.6 && prob<=1)
            {
                isAttacking= true;
                animator.SetTrigger("Attack2");
                playerScript.fxAttack2.Emit(1);
            }
        }
    }
    
    public void CutGrass()
    {
        animator.SetTrigger("Attack2");
        playerScript.fxAttack2.Emit(1);
    }

    public void Defending(bool state)
    {
        animator.SetBool("isDefending", state);
    }

    public void AttackIsDone()
    {
        isAttacking = false;
    }

    //----------------SLIME-------------------

    public void GetHit()
    {
        animator.SetTrigger("GetHit");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }

    public void Walk(bool state)
    {
        animator.SetBool("IsMoving", state);
    }
    public void Alert(bool state)
    {
        animator.SetBool("isAlert", state);
    }

    public void Search()
    {
        animator.SetTrigger("LookForPlayer");
    }
    public void EnemyAttack()
    {
        animator.SetTrigger("Attack");
    }

    //----------------CHICKEN-------------------

    public void Eat()
    {
        animator.SetTrigger("Eat");
    }

    public void TurnHead()
    {
        animator.SetTrigger("TurnHead");
    }

    

}


