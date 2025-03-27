using UnityEngine;

public class Pickups : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    [SerializeField] int scoreValue = 100;
    bool wasColleted = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !wasColleted)
        {
            wasColleted = true;
            FindFirstObjectByType<GameSession>().AddToScore(scoreValue);
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
