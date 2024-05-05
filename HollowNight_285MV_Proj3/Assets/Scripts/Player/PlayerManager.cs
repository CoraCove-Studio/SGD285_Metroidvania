///////////////////////
// Script Contributors:
// 
// Zeb Baukhagen
// 
///////////////////////

using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerControls controlScheme;
    [SerializeField] private HealthBar healthBar;
    private readonly int maxHealth = 100;
    private int health;
    private bool invincible = false;

    private void Awake()
    {
        health = maxHealth;
        //controlScheme.Gameplay.ToggleInvincibility.performed += ctx => ToggleInvincibility();
    }

    private void ToggleInvincibility()
    {
        invincible = !invincible;
    }

    public void TakeDamage(int damageAmount)
    {
        if (invincible == false)
        {
            health = Mathf.Clamp(health - damageAmount, 0, maxHealth);
            healthBar.UpdateDesiredBarPos(health, maxHealth);
            if (health == 0)
            {
                Die();
            }
        }
    }

    public void Heal(int amountToHeal)
    {
        health = Mathf.Clamp(health + amountToHeal, 0, maxHealth);
        healthBar.UpdateDesiredBarPos(health, maxHealth);
    }

    private void Die()
    {
        // call loss panel or lose life
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        TakeDamage(15);
    //    }
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        Heal(15);
    //    }
    //}
}
