using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton singleton;
    public UI ui;
    public int overallScore;
    public int levelsComplete;
    public DungeonCreator dungeonCreator;

    [System.Obsolete]
    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        if (dungeonCreator == null)
        {
            dungeonCreator = GameObject.FindWithTag("DunGen").GetComponent<DungeonCreator>();
        }
    }
    void Start()
    {
   
        if (dungeonCreator == null)
        {
            dungeonCreator = GameObject.FindWithTag("DunGen").GetComponent<DungeonCreator>();
        }
    }

  public void AddSingleton(int v)

  {
    overallScore += v;
  }
}

