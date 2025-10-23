using UnityEngine;

public class Boosters : MonoBehaviour
{
    public enum BoosterType
    {
        SpeedBoost,
        Shield,
        TimeBonus,
        ScoreMultiplier
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
            if (player != null)
            {
                ActivateBooster(player);

                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);

                if (pickupSound != null)
                {
                    // Create a temporary GameObject with an AudioSource
                    GameObject soundObj = new GameObject("PickupSound");
                    AudioSource source = soundObj.AddComponent<AudioSource>();

                    source.clip = pickupSound;
                    source.volume = 1.5f;          // go above 1.0f for extra loudness
                    source.spatialBlend = 0f;      // 0 = 2D sound, 1 = 3D
                    source.pitch = 1.0f;
                    source.Play();

                    Destroy(soundObj, pickupSound.length); // clean up after sound finishes
                }

                Destroy(gameObject);
            }
        }
    }


    
    void ActivateBooster(CarController player)
    {
        switch (boosterType)
        {
            case BoosterType.SpeedBoost:
                player.ApplySpeedBoost(value, duration);
                break;
            case BoosterType.Shield:
                player.ApplyShield(duration);
                break;
            case BoosterType.TimeBonus:
                player.AddTime(value);
                break;
            case BoosterType.ScoreMultiplier:
                player.ApplyScoreMultiplier(value, duration);
                break;
        }
    }
}