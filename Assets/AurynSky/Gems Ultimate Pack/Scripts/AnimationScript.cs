using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {

    public Light Light;

    [Header("General Config Bools")]
        public bool isAnimated = false;

        public bool isRotating = false;
        public bool isFloating = false;
        public bool isScaling = false;
        public bool isShining = false;

    [Header("Float/Rotation Config")]
        public Vector3 rotationAngle;
        public float rotationSpeed;

        public float floatSpeed;
        private bool goingUp = true;
        public float floatRate;
        private float floatTimer;
   
   [Header("Scale Config")]
        public Vector3 startScale;
        public Vector3 endScale;

        private bool scalingUp = true;
        public float scaleSpeed;
        public float scaleRate;
        private float scaleTimer;

    [Header("Light Config")]
        public float maxIntensity;
        public float minIntensity;
        public float pulseSpeed;
        private float targetIntensity;
        private float currentIntensity;


	// Use this for initialization
	void Start () 
    {
        if(isShining)
        {
            Light.gameObject.SetActive(true);

        }
	}
	
	// Update is called once per frame
	void Update () {

       
        
        if(isAnimated)
        {
            if(isRotating)
            {
                transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
            }

            if(isFloating)
            {
                floatTimer += Time.deltaTime;
                Vector3 moveDir = new Vector3(0.0f, 0.0f, floatSpeed);
                transform.Translate(moveDir);

                if (goingUp && floatTimer >= floatRate)
                {
                    goingUp = false;
                    floatTimer = 0;
                    floatSpeed = -floatSpeed;
                }

                else if(!goingUp && floatTimer >= floatRate)
                {
                    goingUp = true;
                    floatTimer = 0;
                    floatSpeed = +floatSpeed;
                }
            }

            if(isScaling)
            {
                scaleTimer += Time.deltaTime;

                if (scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
                }
                else if (!scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
                }

                if(scaleTimer >= scaleRate)
                {
                    if (scalingUp) { scalingUp = false; }
                    else if (!scalingUp) { scalingUp = true; }
                    scaleTimer = 0;
                }
            }

            if(isShining)
            {
                currentIntensity = Mathf.MoveTowards(Light.intensity, targetIntensity, Time.deltaTime*pulseSpeed);

                if(currentIntensity >= maxIntensity)
                {
                    currentIntensity = maxIntensity;
                    targetIntensity = minIntensity;
                }
                else if(currentIntensity<= minIntensity)
                {
                    currentIntensity = minIntensity;
                    targetIntensity = maxIntensity;
                }
                Light.intensity = currentIntensity;
            } 
        }
	}

}
