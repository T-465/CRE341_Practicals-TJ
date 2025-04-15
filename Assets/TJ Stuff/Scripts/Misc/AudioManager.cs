using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Singleton singleton;
    public static AudioManager instance; 
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public bool played;


    private void Awake()
    {
        singleton = GameObject.FindWithTag("singleton").GetComponent<Singleton>();
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Start();
    }

    public void Start()
    {
        singleton = GameObject.FindWithTag("singleton").GetComponent<Singleton>();
        
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            PlayMusic("Main Menu Theme");
        }
        else if (SceneManager.GetActiveScene().name == "GhostLight")
        {
            PlayMusic("Game Theme");
        }
   
        

    }
    public void PlayHatch()
    {
        PlaySFX("Hatch Open");
        played = true;

    }
    public void PlayMusic (string name)
    {
        Sound s = System.Array.Find(musicSounds, sound => sound.name == name);
        if (s == null) return;
        musicSource.clip = s.clip;
        musicSource.Play();
    }
    public void PlaySFX (string name)
    {
        Sound s = System.Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null) return;
        sfxSource.clip = s.clip;
        sfxSource.Play();
    }
       void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
