using UnityEngine;

public class FinalScore : MonoBehaviour
{
    public static FinalScore finalScore;
    public UI ui;
    public int overallScore;
    void Awake()
    {
        if (finalScore == null)
        {
            finalScore = this;
            DontDestroyOnLoad(this);
        }
    }
    void Update()
    {
        
    }
  public void AddFinalScore(int v)

  {
    overallScore += v;
    
  }
}
