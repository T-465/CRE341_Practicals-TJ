using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HatchOpen : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Opening", false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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
