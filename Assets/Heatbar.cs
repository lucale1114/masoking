using UnityEngine;

namespace Player
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Transform healthFill; // The filling portion of the health bar (Transform or Image)
        [SerializeField] private SpriteRenderer healthBarRenderer; // Optional, for changing color based on health

        // Update the health bar based on the normalized heat value (0 to 1)
        public void UpdateHealthBar(float normalizedHeat)
        {
            // Update the health bar's fill amount (based on the normalized heat value)
            if (healthFill != null)
            {
                healthFill.localScale = new Vector3(normalizedHeat, 1f, 1f); // Scale on the X axis
            }

        }
    }


}

