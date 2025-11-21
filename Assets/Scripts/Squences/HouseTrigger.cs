using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseTrigger : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            SceneManager.LoadScene("FashionScene");
        }
    }
}