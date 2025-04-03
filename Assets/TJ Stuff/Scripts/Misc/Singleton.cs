using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton singleton;
    public UI ui;
    public int overallScore;
    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
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

