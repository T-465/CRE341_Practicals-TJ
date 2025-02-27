using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Flashlight : MonoBehaviour, ISwitchable
{
    public PlayerInput playerinput;
    public GameObject volumebeam;
    public Light FlashLight;
   
    public float countdown = 2;
    public bool flashlighton;
    public bool flashlightfull;
    public bool flashlightdead;
    

   // public AudioSource click;
    private void Start()
    {
        flashlightfull = true;
       
    }

    public void Update()
    {
        #region Flashlight
        // Check for player input and toggle the flashlight
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
        
        if (Input.GetMouseButtonDown(0) && flashlightfull == true)
        {
            Toggle();
        }

        
        if (FlashLight.enabled == true)
        {
            countdown -= Time.deltaTime;
            volumebeam.SetActive(true);
        }
        else if (FlashLight.enabled == false)
        {
           volumebeam.SetActive(false);
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
           volumebeam.SetActive(false);
            FlashLight.enabled = false;
            flashlightdead = true;
            flashlightfull = false;
        }
        #endregion
    }
    public void Toggle()
    {
     // Flashlight sound
     // click.Play();
      FlashLight.enabled = !FlashLight.enabled;
    }

}
