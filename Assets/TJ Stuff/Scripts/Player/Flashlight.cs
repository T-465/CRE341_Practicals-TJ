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
   
    public float countdown = 10;
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

    
        if (countdown >= 15)
        {
            countdown = 15;

        }
        
        if (Input.GetMouseButtonDown(0))
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
            }
        }
        if (countdown >= 11)
        {
      
            flashlightfull = true;

        }

        if (countdown <= 8 && countdown >= 5)
        {

            flashlightfull = false;
     
        }
        if (countdown <= 5 && countdown >= 0)
        {

            flashlightdead = false;

        }
        if (countdown <= 0)
        {
           volumebeam.SetActive(false);
            FlashLight.enabled = false;
            flashlightdead = true;
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
