using UnityEngine;

public class Boosters : MonoBehaviour
{
    public enum BoosterType
    {
        SpeedBoost,
        Shield,
        TimeBonus,
        ScoreMultiplier,
        StylePoints   // ⭐ NEW booster type
    }

    public BoosterType boosterType;

    public float duration = 5f;   // for timed effects
    public float value = 2f;      // amount of boost or points

    [Header("Effects")]
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Try to get the car controller (for speed, shield, etc.)
            CarController player = other.GetComponent<CarController>();

            // Activate booster
            ActivateBooster(player);

            // Particle effect
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            // Play pickup sound
            if (pickupSound != null)
            {
                GameObject soundObj = new GameObject("PickupSound");
                AudioSource source = soundObj.AddComponent<AudioSource>();

                source.clip = pickupSound;
                source.volume = 1.5f;
                source.spatialBlend = 0f;
                source.pitch = 1.0f;
                source.Play();

                Destroy(soundObj, pickupSound.length);
            }

            // Destroy booster object
            Destroy(gameObject);
        }
    }

    void ActivateBooster(CarController player)
    {
        switch (boosterType)
        {
            case BoosterType.SpeedBoost:
                if (player != null)
                    player.ApplySpeedBoost(value, duration);
                break;

            case BoosterType.Shield:
                if (player != null)
                    player.ApplyShield(duration);
                break;

            case BoosterType.TimeBonus:
                if (player != null)
                    player.AddTime(value);
                break;

            case BoosterType.ScoreMultiplier:
                if (player != null)
                    player.ApplyScoreMultiplier(value, duration);
                break;

            case BoosterType.StylePoints:
                GiveStylePoints((int)value);
                break;
        }
    }

    void GiveStylePoints(int amount)
    {
        StyleManager manager = FindAnyObjectByType<StyleManager>();

        if (manager != null)
        {
            manager.stylePoints += amount;
            manager.UpdateStylePointsUI(); // plays sound + anim
            Debug.Log("⭐ Booster added " + amount + " style points!");
        }
        else
        {
            Debug.LogWarning("❗ StyleManager not found in scene!");
        }
    }
}
