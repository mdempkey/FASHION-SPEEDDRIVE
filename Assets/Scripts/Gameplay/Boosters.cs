using UnityEngine;

public class Boosters : MonoBehaviour
{
    public enum BoosterType
    {
        SpeedBoost,
        Shield,
        TimeBonus,
        ScoreMultiplier,
        StylePoints
    }

    public BoosterType boosterType;
    public float duration = 5f;
    public float value = 2f;
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CarController player = other.GetComponent<CarController>();
            
            // Apply booster effect
            ActivateBooster(player);
            
            // Spawn pickup effect
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
                source.Play();
                Destroy(soundObj, pickupSound.length);
            }

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
            manager.UpdateStylePointsUI();
            Debug.Log("Booster added " + amount + " style points");
        }
        else
        {
            Debug.LogWarning("StyleManager not found in scene");
        }
    }
}