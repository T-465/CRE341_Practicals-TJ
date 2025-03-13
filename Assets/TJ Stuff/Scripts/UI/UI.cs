using UnityEngine;

public class UI : MonoBehaviour
{

    public TMPro.TextMeshProUGUI score;
    public int currentScore;
    public int levelScore;
    public FinalScore finalScore;

public void Awake() 
{
 finalScore = GameObject.Find("FinalScore").GetComponent<FinalScore>();
}

    private void Start()
    {
        currentScore = 0;
        score.text = "x" + currentScore.ToString();
    }
  public void AddScore(int v)

  {
    currentScore += v;
    score.text = "x" + currentScore.ToString();
    finalScore.AddFinalScore(v);
  }

    void OnDisable()
    {
        levelScore = currentScore;
        currentScore = 0;
        score.text = "x" + currentScore.ToString();
    }

}
