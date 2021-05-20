using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    private Rigidbody myRb;
    private GameManager gameManager;

    void Start()
    {
        myRb = GetComponent<Rigidbody>();
    }

    public void Rotate(Vector3 direcao)
    {
        Quaternion newRotation = Quaternion.LookRotation(direcao);
        myRb.MoveRotation(newRotation);
    }

    public void LookAt()
    {   
        Vector3 lookDirection = (gameManager.player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, gameManager.lookAtSpeed*Time.deltaTime);
    }
}
