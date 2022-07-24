using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip birdThrow;
    [SerializeField] private AudioClip newRecord;
    [SerializeField] private AudioClip gameComplete;
    public static SoundController Instance { get; private set; }
    private AudioSource audioSource;

    private void Awake()
    {
        // Check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, destroy other instances
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayThrowBird()
    {
        audioSource.PlayOneShot(birdThrow);
    }

    public void PlayNewRecord()
    {
        audioSource.PlayOneShot(newRecord);
    }

    public void PlayGameComplete()
    {
        audioSource.PlayOneShot(gameComplete);
    }
}