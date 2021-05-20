using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassControl : MonoBehaviour, IHitObj
{
    private GameManager gameManager;
    public ParticleSystem fxHit;
    public float growTime;
    public bool isCut;
    private Vector3 totalScale;
    private Vector3 smallScale;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        smallScale = new Vector3(0.6f,0.6f,0.6f);
        totalScale = new Vector3(2.1f,2.1f,2.1f);
    }
    public void GetHit(int dano)
    {
        if(!isCut)
        {
            isCut = true;
            transform.localScale = smallScale;
            fxHit.Emit(Random.Range(15,20));

            if(gameManager.Perc(10))
            {
                Instantiate(gameManager.emerald, transform.position, transform.rotation);
            }

            StartCoroutine(Grow(growTime));
            
        }
    }

    IEnumerator Grow(float time)
    {
        float currentTime = 0.0f; //comeca em 0 pois comeca a contar quando a coroutine é iniciada
        do
        {
            transform.localScale = Vector3.Lerp(smallScale,totalScale,currentTime/time);
            currentTime += Time.deltaTime;
            yield return null;
        }while (currentTime <= time);

        isCut = false;
    }

    /*
    o método lerp funciona com porcentagems. A variavel currentTime conta o tempo que esta se passando desde o inicion da coroutine
    e a variavel time representa o tempo total que a grama leva pra crescer. Ao dividir currentTime por time, temos uma porcentagem.
    Essa sera a porcentagem do tanto que a grama cresceu, do seu valor total, naquele tempo. 
    EX. time = 10; currentTime = 2(neste momento)
    nesse momento, 2/10 = 0.2 = 20%
    ou seja, em 2 segundos, a grama terá 20% do seu tamanho total.
    */
}
