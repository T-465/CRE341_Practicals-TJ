using UnityEngine;

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
            // Open the hatch
            animator.SetBool("Opening", true);
        }
    }


}
