using UnityEngine;

public class BallObstacle : MonoBehaviour
{
    public AudioClip pushSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && pushSound != null)
        {
            audioSource.PlayOneShot(pushSound);
        }
    }
}