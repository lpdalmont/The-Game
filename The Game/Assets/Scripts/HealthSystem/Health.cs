using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{   [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth;
    private Animator animator;
    private bool isDead;
    
    [Header ("Health Regen")]
    [SerializeField] private float healthRegenRate;
    [SerializeField] private float secsBeforeRegen;
    private bool readyToRegenerate;
    
    [Header ("iFrames")]
    [SerializeField]private float iFramesDuration;

    [SerializeField] private int numberOfFlashes;
    
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        readyToRegenerate = true;
    }

    public void Update()
    {
        if (readyToRegenerate)
        {
            currentHealth = Mathf.Clamp(currentHealth + healthRegenRate * Time.deltaTime, 0, startingHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        readyToRegenerate = false;
        if (currentHealth > 0)
        {   
            animator.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
            StartCoroutine(HealthRegen());


        }
        else
        {
            if (!isDead)
            {
                animator.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;
                isDead = true;
            }

            
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetStartingHealth()
    {
        return startingHealth;
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration/ (numberOfFlashes * 2 ));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration/ (numberOfFlashes * 2 ));
        }
        
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    private IEnumerator HealthRegen()
    {   
        yield return new WaitForSecondsRealtime(secsBeforeRegen);
        readyToRegenerate = true;
    }
}