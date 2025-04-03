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
            dungeonCreator = FindObjectOfType<DungeonCreator>();
        }
    }
    void Update()
    {
        
    }
  public void AddSingleton(int v)

  {
    overallScore += v;
  }
}

