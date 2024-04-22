using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerControls controlScheme;
    private int maxHealth = 100;
    private int health;
    private bool invincible = false;
    private int weaponLevel = 1;

    private void Awake()
    {
        //controlScheme.Gameplay.ToggleInvincibility.performed += ctx => ToggleInvincibility();
    }

    private void ToggleInvincibility()
    {
        invincible = !invincible;
    }

    public void TakeDamage(int damageAmount)
    {
        if (health >= damageAmount)
        {
            health = Mathf.Clamp(health - damageAmount, 0, maxHealth);
            if (health == 0)
            {
                Die();
            }
        }
    }

    public void Heal(int amountToHeal)
    {
        health = Mathf.Clamp(health + amountToHeal, 0, maxHealth);
    }

    private void LevelUpWeapon()
    {
        weaponLevel += 1;
    }

    private void Die()
    {
        // call loss panel or lose life
    }
}
