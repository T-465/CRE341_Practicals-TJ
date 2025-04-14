using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] public int playerHealth = 10;
    public UI ui;
    [SerializeField] public float speed;
    public CharacterController cc;
    public PlayerInput playerinput;
    public GameObject dungeonCreator;
    public MouseLook mouseLook;
   
    public void Awake()
    {

        cc = GetComponent<CharacterController>();
        ui = GameObject.Find("UI").GetComponent<UI>();

    }
    public void Start()
    {
        dungeonCreator = GameObject.FindWithTag("DunGen");
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
    }
    public void OnGameOver() 
    { 
        
        speed = 0;
        cc.enabled = false;
        Time.timeScale = 0f;
        dungeonCreator.SetActive(false);
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
