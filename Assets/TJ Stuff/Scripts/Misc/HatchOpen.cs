using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HatchOpen : MonoBehaviour
{
    public Animator animator;

    public GameObject[] ghosts;
    public bool allGhostsDead;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Opening", false);
    }
    void Update()
    {
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        if (ghosts.Length == 0)
        {
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
    animator.SetBool("Opening", true);
    yield return new WaitForSeconds(1);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
}


}
