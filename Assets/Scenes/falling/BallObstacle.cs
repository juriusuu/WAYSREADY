using UnityEngine;

public class BallObstacle : MonoBehaviour
{
    public AudioClip pushSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.clip = pushSound;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && pushSound != null)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}