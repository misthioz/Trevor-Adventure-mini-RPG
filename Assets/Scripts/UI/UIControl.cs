using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [Header("Barras")]
        public Slider sliderHP; 
        public Slider sliderStamina;

    [Header("Imagens")]
        public Image imageShield;
        public Image fundoSword;    
    [Header("Textos")]
        public Text textDiamonds;
        public Text textRubys;
        public Text textEmeralds;
        private Text text;
    private PlayerController player;
    private GameManager gameManager;
    void Start()
    {
        player = GameObject.FindObjectOfType(typeof(PlayerController)) as PlayerController;
        gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AtualizaSliderHP(int hp)
    {
        sliderHP.value = hp;
    }
    public void AtualizaSliderStamina(float stamina)
    {
        sliderStamina.value = stamina;
    }
    public void AtivarImagem(bool state)
    {
        imageShield.gameObject.SetActive(state);
    }

    public void AtualizaText(int amount, int gem)
    {
        int qtd = 0;
        if(gem == 1)
        {
            qtd = gameManager.diamonds;
            text = textDiamonds;
        }
        else if(gem == 2)
        {
            qtd = gameManager.emeralds;
            text = textEmeralds;
        }
        else if(gem == 3)
        {
            qtd = gameManager.rubys;
            text = textRubys;
        }
        qtd += amount;
        
        text.text = string.Format("{0}", qtd);
    }
}
