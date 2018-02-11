using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine.XR.iOS
{
	public class UnityARHitTest : MonoBehaviour
	{
		public Transform m_levelTransform;
		public float maxRayDistance = 30.0f;
		public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer
        public Button stopDetectionButton;
        public GameObject bird;

        private bool isDetecting;

        bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
                    Debug.Log ("Got hit!");
                    m_levelTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                    m_levelTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                    Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_levelTransform.position.x, m_levelTransform.position.y, m_levelTransform.position.z));
                    stopDetectionButton.gameObject.SetActive(true);
                    return true;
                }
            }
            stopDetectionButton.gameObject.SetActive(false);
            return false;
        }

        public void disablePlaneDetection()
        {
            // no need to change position of level anymore
            isDetecting = false;

            // do not display stopDetectionButton anymore
            stopDetectionButton.gameObject.SetActive(false);

            // Start game logic
            GameController.Instance.StartPlay();

            bird.SetActive(true);
        }

        private bool isPointerOverUIObject()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        void Start()
        {
            isDetecting = true;
            stopDetectionButton.onClick.AddListener(disablePlaneDetection);
        }
		
		void Update () {
			#if UNITY_EDITOR
            if( isDetecting == true && !isPointerOverUIObject())
            {
                if (Input.GetMouseButtonDown (0)) 
                {
    				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
    				RaycastHit hit;
    				
    				//we'll try to hit one of the plane collider gameobjects that were generated by the plugin
    				//effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
    				if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) 
                    {
                        stopDetectionButton.gameObject.SetActive(true);
    					//we're going to get the position from the contact point
                        m_levelTransform.position = hit.point;
            Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_levelTransform.position.x, m_levelTransform.position.y, m_levelTransform.position.z));

    					//and the rotation from the transform of the plane collider
                        m_levelTransform.rotation = hit.transform.rotation;
    				}
                    else
                        stopDetectionButton.gameObject.SetActive(false);
                }
			}
			#else
            if (Input.touchCount > 0 && m_levelTransform != null)
			{
				var touch = Input.GetTouch(0);
                if ((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) && isDetecting == true && !isPointerOverUIObject())
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = {
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
                        ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    }; 
					
                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (HitTestWithResultType (point, resultType))
                        {
                            return;
                        }
                    }
				}
			}
			#endif

		}

	
	}
}
