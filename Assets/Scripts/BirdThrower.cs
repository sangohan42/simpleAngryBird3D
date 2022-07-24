using System;
using UnityEngine;

public class BirdThrower : PressInputBase
{
    private readonly Vector2 sensitivity = new Vector2(8f, 100f);

    public float speed = 5f;
    public float resetBirdAfterSeconds = 8f;
    public float lerpTimeFactorOnTouch = 7f;
    public float cameraNearClipPlaneFactor = 2.5f;

    private Vector3 direction;

    private Vector3 inputCurrentPosition;
    private Vector2 inputStartPosition;

    private Rigidbody rigidbody;

    private bool isHoldingBird;

    public float ThrowingDistance { get; private set; }

    [SerializeField] private LevelController levelController;
    [SerializeField] private SoundController soundController;

    [SerializeField] private GameObject bird;

    public static event Action OnFail;

    private void Start()
    {
        rigidbody = bird.GetComponent<Rigidbody>();

        Reset();
    }

    private void OnEnable()
    {
        bird.SetActive(true);
    }

    private void OnDisable()
    {
        bird.SetActive(false);
    }

    private void OnInputStarted(Vector2 startPosition)
    {
        inputStartPosition = startPosition;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(inputStartPosition), out var raycastHit, 100f)) return;

        if (raycastHit.transform != bird.transform)
            return;

        isHoldingBird = true;
    }

    private void OnInputHeld(Vector2 heldPosition)
    {
        inputCurrentPosition = heldPosition;
        if (!isHoldingBird) return;

        inputCurrentPosition.z = Camera.main.nearClipPlane * cameraNearClipPlaneFactor;

        Vector3 newBirdPosition = Camera.main.ScreenToWorldPoint(inputCurrentPosition);

        transform.position = Vector3.Lerp(
            transform.position,
            newBirdPosition,
            Time.deltaTime * lerpTimeFactorOnTouch
        );
    }

    private void OnInputReleased()
    {
        if (isHoldingBird)
        {
            if (inputStartPosition.y < inputCurrentPosition.y)
            {
                ThrowingDistance = (bird.transform.position - levelController.CurrentLevelGO.transform.position)
                    .magnitude;
                Throw();
            }
        }

        isHoldingBird = false;
    }

    private void Reset()
    {
        CancelInvoke();

        // reset position
        bird.transform.position = Camera.main.ViewportToWorldPoint(
            new Vector3(0.5f, 0.1f, Camera.main.nearClipPlane * cameraNearClipPlaneFactor)
        );

        isHoldingBird = false;

        // reset rigidbody
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        bird.transform.rotation = Quaternion.Euler(0f, 200f, 0f);
        bird.transform.SetParent(Camera.main.transform);
    }

    private void Throw()
    {
        rigidbody.useGravity = true;

        Vector2 inputPositionDifference = new Vector2
        {
            x = (inputCurrentPosition.x - inputStartPosition.x) / Screen.width,
            y = (inputCurrentPosition.y - inputStartPosition.y) / Screen.height * sensitivity.y
        };

        inputPositionDifference.x = Mathf.Abs(inputCurrentPosition.x - inputStartPosition.x) / Screen.width *
                                    sensitivity.x *
                                    inputPositionDifference.x;

        direction = new Vector3(inputPositionDifference.x, 0f, 1f);
        direction = Camera.main.transform.TransformDirection(direction);

        rigidbody.AddForce((direction + Vector3.up) * (speed * inputPositionDifference.y));

        isHoldingBird = false;

        soundController.PlayThrowBird();

        Invoke(nameof(Reset), resetBirdAfterSeconds);
    }

    protected override void OnPressBegan(Vector2 position)
    {
        OnInputStarted(position);
    }

    protected override void OnPress(Vector2 position)
    {
        OnInputHeld(position);
    }

    protected override void OnPressCancel() => OnInputReleased();

    protected override void OnPressReleased() => OnInputReleased();
}