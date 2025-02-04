using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] int value = 1;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (GameManager.Instance != null) {
                GameManager.Instance.AddScore(value);
            } else {
                Debug.LogError("GameManager instance not found!");
            }
            Destroy(gameObject);
        }
    }
}