using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private PlayerController player;
    private Status status;
    private Animate animate;
    private UIControl scriptUI;
    private Rigidbody myRb;
    public enum MovementMode{Walking, Running, Defending};
    public MovementMode movementMode;
    private float speed;
    private float smoothSpeed;

    [Header("Audio Settings")]
        public AudioClip soundRun;
    void Start()
    {
        player = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();
        status = GetComponent<Status>();
        animate = GetComponent<Animate>();
        scriptUI = GameObject.FindObjectOfType(typeof(UIControl)) as UIControl;
        speed = status.walkSpeed; 
    }

    public void Move(Vector3 direcao)
    {
        if(direcao.magnitude>0.1f)
        {
            smoothSpeed = Mathf.Lerp(smoothSpeed, speed, Time.deltaTime);
            float targetAngle = Mathf.Atan2(direcao.x, direcao.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,targetAngle,0);
        }
        else
        {
            smoothSpeed = Mathf.Lerp(smoothSpeed, 0, Time.deltaTime);
        }

        controller.Move(direcao*speed*Time.deltaTime);
        animate.MoveAnimations(direcao.magnitude*speed);
    }

    public void SetMovementeMode(MovementMode mode)
    {
        movementMode = mode;
        switch(mode)
        {
            case MovementMode.Walking:
            {
                speed = status.walkSpeed;
                break;
            }
            case MovementMode.Running:
            {
                speed = status.runSpeed;
                break;
            }
            case MovementMode.Defending:
            {
                speed = status.shieldSpeed;
                break;
            }
        }
    }

    public void MovementModeManager()
    {
        if(movementMode!= MovementMode.Running && status.stamina<status.totalStamina)
        {
            status.stamina+= Time.deltaTime*status.staminaIncreaseSpeed;
            scriptUI.AtualizaSliderStamina(status.stamina);
        }
        switch(movementMode)
        {
            case MovementMode.Running:
                if(player.direcao.magnitude>=0.1)
                {
                    status.stamina -= Time.deltaTime*status.staminaDecreaseSpeed;
                    scriptUI.AtualizaSliderStamina(status.stamina);
                }
                
                break;      
        }
    }

    public MovementMode GetMovementMode()
    {
        return movementMode;
    }

    public void Rotacionar(Vector3 direcao)
    {
        Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        myRb.MoveRotation(novaRotacao);
    }

    
}
