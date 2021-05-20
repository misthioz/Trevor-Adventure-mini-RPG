using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionControl : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject camB;
    public GameObject camC;


    void Start()
    {
        gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "teste":
                other.gameObject.SetActive(false);
                break;
            
            case "CamTrigger":
                camB.SetActive(true);
                break;
            
            case "CamTrigger 2":
                camC.SetActive(true);
                break;
            
            case "diamond":
               AudioControl.instance.PlayOneShot(gameManager.soundGem);
               gameManager.SetGems(1, 1);
               Destroy(other.gameObject);
                break;

            case "ruby":
               AudioControl.instance.PlayOneShot(gameManager.soundGem);
               gameManager.SetGems(1, 3);
               Destroy(other.gameObject);
                break;
            
            case "emerald":
               AudioControl.instance.PlayOneShot(gameManager.soundGem);
               gameManager.SetGems(1, 2);
               Destroy(other.gameObject);
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "CamTrigger":
                camB.SetActive(false);
                break;

            case "CamTrigger 2":
                camC.SetActive(false);
                break;
        }
    }
}
