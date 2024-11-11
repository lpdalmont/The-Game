using UnityEngine;
using UnityEngine.UI;

public class HeatlhBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalHealthBar.fillAmount = playerHealth.GetCurrentHealth() / playerHealth.GetStartingHealth();
    }

    private void Update()
    {
        currentHealthBar.fillAmount = playerHealth.GetCurrentHealth()/ playerHealth.GetStartingHealth();
    }
    
}
