using UnityEngine;

public class ThrowControl : MonoBehaviour 
{
	public Vector2 sensivity = new Vector2(8f, 100f);

	public float speed = 5f;
    public float resetBirdAfterSeconds = 8f;
	public float lerpTimeFactorOnTouch = 7f;
	public float cameraNearClipPlaneFactor = 2.5f;

	private Vector3 direction;

	private Vector3 inputPositionCurrent;
	private Vector2 inputPositionPivot;
	private Vector2 inputPositionDifference;

	private Vector3 newBirdPosition;
	private BirdControl birdControl;
	private Rigidbody _rigidbody;
	private RaycastHit raycastHit;

	private bool isThrown; 
	private bool isHolding;

	private bool isInputBegan = false;
	private bool isInputEnded = false;
	private bool isInputLast = false;


	void Start() 
	{
		_rigidbody = GetComponent<Rigidbody> ();
        birdControl = GetComponent<BirdControl>();

		Reset ();
	}

	void Update() 
	{
		#if UNITY_EDITOR

			isInputBegan = Input.GetMouseButtonDown(0);
			isInputEnded = Input.GetMouseButtonUp(0);
			isInputLast = Input.GetMouseButton(0);

			inputPositionCurrent = Input.mousePosition;

		#else

			isInputBegan = Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
			isInputEnded = Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended;
			isInputLast = Input.touchCount == 1;

			inputPositionCurrent = Input.GetTouch (0).position;

		#endif

		if (isHolding)
			OnTouch ();

		if (isThrown)
			return;
			
		if (isInputBegan)
		{
			if (Physics.Raycast (Camera.main.ScreenPointToRay (inputPositionCurrent), out raycastHit, 100f)) 
			{
				if (raycastHit.transform == transform) 
				{
					isHolding = true;
					transform.SetParent (null);

					inputPositionPivot = inputPositionCurrent;
				}
			}
		}

		if(isInputEnded)
		{

			if(inputPositionPivot.y < inputPositionCurrent.y)
			{ 
				Throw (inputPositionCurrent);
			}
		}
	}

	void Reset()
	{
		CancelInvoke ();

        // if the bird touched the enemy during the current throw then this function do nothing so no worry
        birdControl.SetFailed();

        // reset position
		transform.position = Camera.main.ViewportToWorldPoint (
			new Vector3 (0.5f, 0.1f, Camera.main.nearClipPlane * cameraNearClipPlaneFactor)
		);
		
        newBirdPosition = transform.position;

		isThrown = isHolding = false;

        // reset rigidbody
		_rigidbody.useGravity = false;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;

		transform.rotation = Quaternion.Euler (0f, 200f, 0f);
		transform.SetParent (Camera.main.transform);
	}

	void OnTouch() 
	{
		inputPositionCurrent.z = Camera.main.nearClipPlane * cameraNearClipPlaneFactor;

        newBirdPosition = Camera.main.ScreenToWorldPoint (inputPositionCurrent);

		transform.localPosition = Vector3.Lerp (
			transform.localPosition, 
            newBirdPosition, 
			Time.deltaTime * lerpTimeFactorOnTouch
		);
	}

	void Throw(Vector2 inputPosition) 
	{
		birdControl.SetThrown();

		_rigidbody.useGravity = true;

		inputPositionDifference.y = (inputPosition.y - inputPositionPivot.y) / Screen.height * sensivity.y;

		inputPositionDifference.x = (inputPosition.x - inputPositionPivot.x) / Screen.width;
		inputPositionDifference.x = Mathf.Abs (inputPosition.x - inputPositionPivot.x) / Screen.width * sensivity.x * inputPositionDifference.x;

        Debug.Log("inputPositionDifference = " + inputPositionDifference.x + ", " + inputPositionDifference.y );
		direction = new Vector3 (inputPositionDifference.x, 0f, 1f);
		direction = Camera.main.transform.TransformDirection (direction);

		_rigidbody.AddForce((direction + Vector3.up) * speed * inputPositionDifference.y);

		isHolding = false;
		isThrown = true;

		Invoke ("Reset", resetBirdAfterSeconds);
	}
}