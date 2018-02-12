using UnityEngine;
using System.Collections;

public class BirdControl : MonoBehaviour 
{
	public Material standardMaterial, fadeMaterial;
	[HideInInspector]
    public bool thrown, failed, success, clear;
	private Color col;
	private float distance;
	public GameObject stage;
	[HideInInspector]
	public AudioSource audioSource;
	private Rigidbody thisRigidbody;
	
	public delegate void ThrowAction();
	public delegate void EnemyKilledAction( float distance );
	public delegate void FailAction();
    public static event ThrowAction OnThrow;
    public static event EnemyKilledAction OnEnemyKilled;
    public static event FailAction OnFail;
	
	void Awake()
	{
		col = GetComponent<Renderer>().material.color;
		audioSource = GetComponent<AudioSource>();
		thisRigidbody = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
		if(failed || success) 
		{
			ResetBird();
		}
		
		if(thisRigidbody.IsSleeping() && thrown && !failed) 
		{
			print("failed, not touched enemy");
			SetFailed();
		}
	}
	
	public void ResetBird()
	{
        thrown = failed = success = false;
		col.a = 1;
		clear = true;
		GetComponent<Renderer>().material = standardMaterial;
		GetComponent<Renderer>().material.color = col;
	}
	
	public void SetThrown()
	{
		thrown = true;
		if(OnThrow != null)
			OnThrow();
        distance = (transform.position - stage.transform.position).magnitude;
        SoundController.Instance.playThrowBird();
	}
	
    public void SetSuccess()
	{
        if(!success && !failed) 
		{
            success = true;
			if(OnEnemyKilled != null)
                OnEnemyKilled(distance);
		}
	}

    public void SetFailed()
    {
        if (!failed && !success && thrown)
        {
            failed = true;
            if (OnFail != null)
                OnFail();
        }
    }
	
    // No Dead zone yet so useless for now
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "deadZone") 
		{
			SetFailed();
			print("failed, deadZone");
		}
	}
	
	void OnCollisionEnter(Collision other)
	{
        switch (other.gameObject.tag)
        {
            case "Enemy":
                DestroyObject(other.gameObject);
                SetSuccess();
                break;
        }
	}
}