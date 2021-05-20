using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeCutControl : MonoBehaviour, IHitObj
{
    public GameObject stump;
    public Transform dropPosition;
    public ParticleSystem particleLeaves;
    [Header("UI")]
        public GameObject canvasHP;
        public Slider sliderHP;
        public Image imageSlider;
        public Color minColor, maxColor;

    
    private Status status;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
        status = GetComponent<Status>();
        status.health = status.totalHealth;
        sliderHP.maxValue = status.totalHealth;
        AtualizaUI();
    }

    
    public void GetHit(int dano)
    {
        canvasHP.gameObject.SetActive(true);
        status.health -= dano;
        AtualizaUI();
        //Instantiate(gameManager.woodObj, transform.position, transform.rotation);
        particleLeaves.Emit(5);
        DropWood();

        if(status.health <= 0)
        {
            Destroy(this.gameObject);
            Instantiate(stump, transform.position, transform.rotation);
            //DropWood();
        }
    }

    void DropWood()
    {
        Vector3 pos = Random.insideUnitSphere*1.5f;
        pos += transform.position;
        pos.y = transform.position.y+0.1f;
        Instantiate(gameManager.woodObj, pos, transform.rotation);           
        
    }

    void AtualizaUI()
    {
        sliderHP.value = status.health;
        float porcentagemHP = (float)status.health/status.totalHealth;

        Color corHP = Color.Lerp(minColor, maxColor, porcentagemHP);
        imageSlider.color = corHP;
    }
}
