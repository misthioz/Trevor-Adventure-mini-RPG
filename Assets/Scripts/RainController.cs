using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainController : MonoBehaviour
{
    private GameManager gameManager;
    public bool isRainig;
    public int rainChance;

    public float testTime;
    private float timer;

    

    


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isRainig)
        {
            timer+= Time.deltaTime;

            if(timer>= testTime)
            {
                print("testing if rain");
                WillRain(rainChance);
                StartCoroutine("RainTime");
                timer = 0;
            }
            
        }
    }

    IEnumerator RainTime()
    {
        yield return new WaitForSeconds(35/*Random.Range(20,40)*/);
        isRainig = false;
        gameManager.OnOffRain(false);
    }

    int RandomNum()
    {
        int n = Random.Range(0,100);
        return n;
    }

    void WillRain(int yes)
    {
        if(RandomNum()<yes)
        {
            print("will rain");

            GetComponent<AudioSource>().Play();
            isRainig = true;
            gameManager.OnOffRain(isRainig);
            gameManager.SpawnDiamond();
        }
    }
}
