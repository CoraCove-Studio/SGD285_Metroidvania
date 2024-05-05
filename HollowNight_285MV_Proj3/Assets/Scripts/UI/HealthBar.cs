///////////////////////
// Script Contributors:
// 
// Zeb Baukhagen
// 
///////////////////////

using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private int lerpSpeed = 5; // Speed of the interpolation
    private float currentBarScale;
    private float desiredBarScale;

    private void Awake()
    {
        // Initialize the current scale
        currentBarScale = gameObject.transform.localScale.x;
    }

    public void UpdateDesiredBarPos(int health, int maxHealth)
    {
        // Normalize the current health with the maxHealth
        float healthRatio = (float)health / maxHealth;
        // Set the desired healthBar scale value based on the health ratio
        desiredBarScale = healthRatio;

        // Start or restart the coroutine to update the health bar scale
        StopCoroutine(UpdateHealthBar());
        StartCoroutine(UpdateHealthBar());
    }

    private IEnumerator UpdateHealthBar()
    {
        // Continue to interpolate until the current scale is approximately equal to the desired scale
        while (!Mathf.Approximately(currentBarScale, desiredBarScale))
        {
            // Interpolate the current scale towards the desired scale
            currentBarScale = Mathf.Lerp(currentBarScale, desiredBarScale, lerpSpeed * Time.deltaTime);
            // Apply the new scale to the health bar object
            gameObject.transform.localScale = new Vector3(currentBarScale, 1f, 1f);
            yield return null;
        }
    }
}
