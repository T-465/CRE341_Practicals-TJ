using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] public int playerHealth = 10;
    public UI ui;
    [SerializeField] public float speed;
    public CharacterController cc;
    public PlayerInput playerinput;
    public AudioSource boo;

    public MouseLook mouseLook;
   
    public void Awake()
    {

        cc = GetComponent<CharacterController>();
        ui = GameObject.Find("UI").GetComponent<UI>();

    }
    public void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        mouseLook = GetComponentInChildren<MouseLook>();
        Time.timeScale = 1f;
        if (ui == null)
        {
            ui = GameObject.Find("UI").GetComponent<UI>();
        }
        
       

    }
    private void FixedUpdate()
    {
        #region PlayerMove

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        cc.SimpleMove(move * speed * Time.deltaTime);


        #endregion

    }


    #region Damage
    public void TakeDamage(int damage)
    {
    
        if (playerHealth <= 0)
        {
            return; 
        }
        playerHealth -= damage;
     
        ui.LoseHealth(damage);

        if (playerHealth <= 0)
        {
            OnGameOver();
        }
        else
        {
                boo.Play();
                ui.JumpscareStart();

        }
    }
    public void OnGameOver() 
    { 
        
        speed = 0;
        cc.enabled = false;
        Time.timeScale = 0f;
        ui.gameOverScreen.SetActive(true);
        ui.Tally();
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
      
   
    
    }
    void Update()
    {
        if (playerHealth <= 0)
        {
            playerHealth = 0;
        }
    }
    public void ShowHitEffect()
    {
        
    }
    #endregion
}
