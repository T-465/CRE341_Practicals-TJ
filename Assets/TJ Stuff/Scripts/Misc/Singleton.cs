using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton singleton;
    public UI ui;
    public int overallScore;
    public int levelsComplete;
    public DungeonCreator dungeonCreator;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        if (dungeonCreator == null)
        {
            dungeonCreator = FindFirstObjectByType<DungeonCreator>();
        }
    }

    void Update()
    {
        if (dungeonCreator == null)
        {
            dungeonCreator = FindFirstObjectByType<DungeonCreator>();
        }
    }
  public void AddSingleton(int v)

  {
    overallScore += v;
  }
}

