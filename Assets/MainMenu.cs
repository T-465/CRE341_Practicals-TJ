using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void GhostLight()
    {
       SceneManager.LoadScene("GhostLight");
       
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
 
}
