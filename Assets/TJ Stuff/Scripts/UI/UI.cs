using UnityEngine;

public class UI : MonoBehaviour
{

    public TMPro.TextMeshProUGUI score;
    public int currentScore;
    public int levelScore;
    public Singleton singleton;

public void Awake() 
{
  singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
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
    singleton.AddSingleton(v);
  }

    void OnDisable()
    {
        levelScore = currentScore;
        currentScore = 0;
        score.text = "x" + currentScore.ToString();
    }

}
