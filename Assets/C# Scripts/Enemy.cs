using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator anim;

    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            
            Die();
        }
    }

    void Die()
    {
        Transform raycastChild = transform.Find("raycast");


        if (raycastChild != null)
        {
            Destroy(raycastChild.gameObject);
        }
        else
        {
            Debug.LogError("Child GameObject named 'raycast' not found on the enemy GameObject.");
        }
        if (!anim.GetBool("IsDead"))
        {
            Debug.Log("enemy died");
            
            anim.SetBool("IsDead", true);

            Collider2D myCollider = GetComponent<Collider2D>();
            if (myCollider != null)
            {
                myCollider.enabled = false;
            }

            Rigidbody2D myRigidbody = GetComponent<Rigidbody2D>();
            if (myRigidbody != null)
            {
                myRigidbody.velocity = Vector2.zero;
                myRigidbody.isKinematic = true;
            }

            this.enabled = false;

            StartCoroutine(ResetIsDeadParameter());
        }
    }

    IEnumerator ResetIsDeadParameter()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(0.95f);

        anim.SetBool("IsDead", false);
        Destroy(gameObject);
    }
}