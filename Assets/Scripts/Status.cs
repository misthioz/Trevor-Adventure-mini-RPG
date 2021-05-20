using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
   [Header("Health")]
      public int totalHealth;
      public int health;
   [Header("Speed")]
      public float walkSpeed;
      public float runSpeed;
      public float shieldSpeed;
   
   [Header("Stamina")]
      public float totalStamina;
      public float stamina;
      public float staminaDecreaseSpeed;
      public float staminaIncreaseSpeed;

   
}
