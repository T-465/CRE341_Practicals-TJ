using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    public static Singleton singleton;
    public UI ui;
    public int overallScore;
    public int levelsComplete;
    public DungeonCreator dungeonCreator;

    public int ghostsTotal;



    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        else if (singleton != this)
        {
            Debug.Log("Singleton already exists. Destroying this");
            Destroy(gameObject);
            return;
        }

        

        if (dungeonCreator == null)
        {
            dungeonCreator = GameObject.FindWithTag("DunGen")?.GetComponent<DungeonCreator>();
        }
    }

    void Start()
    {
        if (dungeonCreator == null)
        {
            dungeonCreator = GameObject.FindWithTag("DunGen")?.GetComponent<DungeonCreator>();
        }
        if (ui == null)
        {
            ui = GameObject.Find("UI")?.GetComponent<UI>();
        }

    }
    void Update()
    {
        ghostsTotal = dungeonCreator.numberofNPCs;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Start();
    }

    public void AddSingleton(int v)
    {
        overallScore += v;
    }

}

