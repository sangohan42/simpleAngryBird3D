using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="ObjectToPlace"/> is moved to the hit position.
/// </summary>
public class PlaceOnARPlane : PressInputBase
{
    [SerializeField] private ARRaycastManager raycastManager;

    public GameObject ObjectToPlace { get; set; }

    private bool pressed;

    private static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    private void Update()
    {
        if (ObjectToPlace == null || Pointer.current == null || pressed == false)
            return;

        var touchPosition = Pointer.current.position.ReadValue();

        if (raycastManager.Raycast(touchPosition, Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one will be the closest hit.
            ObjectToPlace.transform.position = Hits[0].pose.position;
            
            if(!ObjectToPlace.activeSelf)
                ObjectToPlace.SetActive(true);
        }
    }

    protected override void OnPress(Vector2 position) => pressed = true;

    protected override void OnPressCancel() => pressed = false;
}