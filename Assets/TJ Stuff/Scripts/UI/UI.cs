using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
  public GameObject gameUI;

    public TMPro.TextMeshProUGUI score;
    public TMPro.TextMeshProUGUI health;
    public int healthPoints;
    public TMPro.TextMeshProUGUI level;
    public int levelNumber;
    public int currentScore;
    public int levelScore;
    public TMPro.TextMeshProUGUI finalScoreText;
    public Singleton singleton;
    public GameObject loadingScreen;
    public GameObject gameOverScreen;
    public DungeonCreator dungeonCreator;

public void Awake() 
{
  singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
  dungeonCreator = GameObject.FindWithTag("DunGen")?.GetComponent<DungeonCreator>();
  gameOverScreen.SetActive(false);
  loadingScreen.SetActive(true);

  StartCoroutine(LoadScreen());
}
public IEnumerator LoadScreen()
{
  
    yield return new WaitUntil(() => dungeonCreator.start == true);
    loadingScreen.SetActive(false);
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
    if (healthPoints <= 0)
    {
      healthPoints = 0;
 
    }
  }
  public void Update ()

  {
    levelNumber = singleton.levelsComplete + 1;
    level.text = "Level -" + levelNumber.ToString();
  }
  public void Tally()
  {
    gameUI.SetActive(false);

     finalScoreText.text = "Your Final Score was: " + singleton.overallScore.ToString();
  }
    void OnDisable()
    {
        levelScore = currentScore;
        currentScore = 0;
        healthPoints = 10;
        score.text = "x" + currentScore.ToString();
        health.text = "Health: " + healthPoints.ToString();
    }
    public void Restart()
    {
      singleton.levelsComplete = 0;
      singleton.overallScore = 0;
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    
        
    }

}
