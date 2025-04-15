using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HatchOpen : MonoBehaviour
{
    public Animator animator;
    public Singleton singleton;
    public AudioManager audioManager;
    public GameObject arrow;

    public GameObject[] ghosts;
    public bool allGhostsDead;

    [System.Obsolete]
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Opening", false);
        if (singleton == null)
        {
            singleton = FindObjectOfType<Singleton>();
        }
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        arrow.SetActive(false);

    }
    void Update()
    {
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        if (ghosts.Length == 0 && allGhostsDead == false)
        {

            audioManager.PlaySFX("Hatch Open");
            arrow.SetActive(true);
            allGhostsDead = true;
        }
        else
        {
            allGhostsDead = false;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && allGhostsDead)
        {
            StartCoroutine(Leave());
        }
    }
public IEnumerator Leave()
{
    singleton.levelsComplete++;
    animator.SetBool("Opening", true);
    yield return new WaitForSeconds(1);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
}


}
