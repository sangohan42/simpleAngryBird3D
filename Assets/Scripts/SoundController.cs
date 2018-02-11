using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
	
	public AudioClip birdThrow;
	public AudioClip newRecord;
	public AudioClip gameOver;
    public static SoundController Instance { get; private set; }
	private AudioSource thisAudio;
	
    void Awake()
    {
        // Check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, destroy other instances
            Destroy(gameObject);
        }

        Instance = this;
    }

	void Start () {
		thisAudio = GetComponent<AudioSource>();
	}
	
	public void Stop(){
		if(thisAudio.isPlaying) {
			thisAudio.Stop();
			thisAudio.clip = null;
			thisAudio.loop = false;
		}
	}
	
    public void playThrowBird()
    {
        thisAudio.PlayOneShot(birdThrow);
    }

	public void playNewRecord(){
		thisAudio.PlayOneShot(newRecord);
	}
	
	public void playGameOver(){
		thisAudio.PlayOneShot(gameOver);
	}
	
}
