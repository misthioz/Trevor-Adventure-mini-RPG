using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum EnemyState
{
    IDLE, ALERT, PATROL, SEARCH, FURY, FOLLOW, DEAD
}
public enum GameState
{
    GAMEPLAY, GAMEOVER
}

public enum ChickenState
{
    IDLE, WALK, EAT, TURNHEAD, RUN
}

public class GameManager : MonoBehaviour
{   
    public GameState gameState;
    private UIControl scriptUI;
    private RainController rainScript;

    [Header("Info Player")]
        public Transform player;
        public int diamonds, rubys, emeralds, wood;
    
    [Header("SlimeAI")]
        public float slimeIdleWaitTime;
        public Transform[] slimeWayPoints;
        public float slimeDistanceToAttack;
        public float slimeAlertWaitTime;
        public float slimeAttackCooldown;
        public float lookAtSpeed;

    
    [Header("RainManager")]
        public PostProcessVolume postB;
        public ParticleSystem rainParticle;
        private ParticleSystem.EmissionModule rainModule;
        public int rainRateOverTime;
        public int rainIncrement;
        public float rainIncrementDelay;
        public AudioClip soundRain;

    [Header("ChickenAI")]
        public float chickenIdleWaitTime;
        public Transform[] chickenWayPoints;

    
    [Header("Drop Item")]
        public GameObject diamond;
        public GameObject emerald;
        public GameObject ruby;
        public GameObject woodObj;
        public int percDrop;
        public Transform [] diamondSpawnPositions;

    [Header("Audio")]
        public AudioClip soundGem;
    void Start()
    {
        rainModule = rainParticle.emission;
        scriptUI = GameObject.FindObjectOfType(typeof(UIControl)) as UIControl;
        rainScript = GameObject.FindObjectOfType(typeof(RainController)) as RainController;

        diamonds = -1;
        emeralds = -1;
        rubys = -1;
    }
    public int RandomValue()
    {
        int value = Random.Range(0,100);
        return value;
    }

    public bool Perc(int p)
    {
        int temp = Random.Range(0,100);

        bool retorno = temp<=p ? true : false;

        return retorno;
    }
    
    public void ChangeGameState(GameState newState)
    {
        gameState = newState;
    }

    public void OnOffRain(bool isRainig)
    {
        StopCoroutine("RainManager");
        StopCoroutine("PostBManager");

        StartCoroutine("RainManager", isRainig);
        StartCoroutine("PostBManager", isRainig);
    }

    IEnumerator RainManager(bool isRainig)
    {
        switch(isRainig)
        {
            case true: //aumentar a chuva
                //AudioControl.instance.PlayOneShot(soundRain);
                for(float r = rainModule.rateOverTime.constant; r<rainRateOverTime; r+= rainIncrement)
                {
                    rainModule.rateOverTime = r;
                    yield return new WaitForSeconds(rainIncrementDelay);
                }

                rainModule.rateOverTime = rainRateOverTime;

                break;

            case false: //diminuir a chuva
                for(float r = rainModule.rateOverTime.constant; r>0; r-= rainIncrement)
                {
                    rainModule.rateOverTime = r;
                    yield return new WaitForSeconds(rainIncrementDelay);
                }

                rainModule.rateOverTime = 0;

                break;
        }
    }


    IEnumerator PostBManager(bool isRainig)
    {
        switch(isRainig)
        {
            case true:
                for(float w = postB.weight; w<1; w+=1*Time.deltaTime)
                {
                    postB.weight = w;
                    yield return new WaitForEndOfFrame();
                }

                postB.weight = 1;

                break;


            case false:
                for(float w = postB.weight; w>0; w-=1*Time.deltaTime)
                {
                    postB.weight = w;
                    yield return new WaitForEndOfFrame();
                }

                postB.weight = 0;
                break;
        }
    }

    public void SetGems(int amount, int gem)
    {
        if(gem == 1)
        {
            diamonds+=amount;
        }
        else if(gem == 2)
        {
            emeralds += amount;
        }
        else if(gem == 3)
        {
           rubys+=amount;
        }
        
        scriptUI.AtualizaText(1, gem);
    }

    public void SpawnDiamond()
    {
        for(int i= 0; i<2;i++)
        {
            int loc = Random.Range(0, diamondSpawnPositions.Length);
            Vector3 pos = Random.insideUnitSphere;
            pos += diamondSpawnPositions[loc].position;
            pos.y = diamondSpawnPositions[loc].position.y;

            Instantiate(diamond, pos, transform.rotation);

            Destroy(diamond, 20);
        }
    }
}
