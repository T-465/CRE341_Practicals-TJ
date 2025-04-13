using System.Collections;
using UnityEngine;

public class UI : MonoBehaviour
{

    public TMPro.TextMeshProUGUI score;
    public TMPro.TextMeshProUGUI health;
    public int healthPoints;
    public TMPro.TextMeshProUGUI level;
    public int levelNumber;
    public int currentScore;
    public int levelScore;
    public Singleton singleton;
    public GameObject gameOverScreen;
    public DungeonCreator dungeonCreator;

public void Awake() 
{
  singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
  dungeonCreator = GameObject.FindWithTag("DunGen")?.GetComponent<DungeonCreator>();
  gameOverScreen.SetActive(true);
  StartCoroutine(LoadScreen());
}
public IEnumerator LoadScreen()
{
  
    yield return new WaitUntil(() => dungeonCreator.start == true);
    gameOverScreen.SetActive(false);
    yield return null;
}

    private void Start()
    {
        currentScore = 0;
        healthPoints = 10;
        health.text = "Health: " + healthPoints.ToString();
        score.text = "x" + currentScore.ToString();
    }
  public void AddScore(int v)

  {
    currentScore += v;
    score.text = "x" + currentScore.ToString();
    singleton.AddSingleton(v);
  }
    public void LoseHealth(int v)

  {
    healthPoints -= v;
    health.text = "Health: " + healthPoints.ToString();
  }
  public void Update ()

  {
    levelNumber = singleton.levelsComplete + 1;
    level.text = "Level -" + levelNumber.ToString();
  }
    void OnDisable()
    {
        levelScore = currentScore;
        currentScore = 0;
        healthPoints = 10;
        score.text = "x" + currentScore.ToString();
        health.text = "Health: " + healthPoints.ToString();
    }

}
