using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Flashlight : MonoBehaviour, ISwitchable
{
    public PlayerInput playerinput;
    public GameObject volumebeam;
    public Light FlashLight;
   
    public float countdown = 2;
    public bool flashlighton;
    public bool flashlightfull;
    public bool flashlightdead;
    

    private void Start()
    {
        flashlightfull = true;
       
    }

    public void Update()
    {
        #region Flashlight
        if (FlashLight.enabled == true)
        {
            flashlighton = true;
        }
        else
        {
            flashlighton = false;
        }

    
        if (countdown >= 2)
        {
            countdown = 2;

        }
        if (Input.GetMouseButton(0))
        {
            if (flashlightfull)
            {
                FlashLight.enabled = true;
                volumebeam.SetActive(true);
            }
        }
        else
        {
            FlashLight.enabled = false;
            volumebeam.SetActive(false);
        }
      

        
        if (FlashLight.enabled == true)
        {
            countdown -= Time.deltaTime;
        }
        else if (FlashLight.enabled == false)
        {
            StartCoroutine(Cooldown());
            IEnumerator Cooldown()
            {
                yield return new WaitForSeconds(4);
                countdown += Time.deltaTime;
                yield return new WaitUntil(() => countdown >= 2);
                countdown -= Time.deltaTime;
            }
        }
        if (countdown >= 2)
        {
      
            flashlightfull = true;
            flashlightdead = false;

        }

        if (countdown == 0 || countdown < 1)
        {
            FlashLight.enabled = false;
            flashlightdead = true;
            flashlightfull = false;
            volumebeam.SetActive(false);
        }
        #endregion
    }
    public void Toggle()
    {
      FlashLight.enabled = !FlashLight.enabled;
    }

}
