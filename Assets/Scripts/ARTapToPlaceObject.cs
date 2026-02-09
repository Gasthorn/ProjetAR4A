using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject gameObjectToInstantiate;
    public Slider scaleSlider;
    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(index: 0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }   
    
    void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition)) return;
        if(_arRaycastManager.Raycast(touchPosition,hits,trackableTypes:TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if(spawnedObject == null)
            {
                spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                spawnedObject.transform.localScale = Vector3.one * scaleSlider.value;
            }
            else if(spawnedObject != null && scaleSlider != null)
            {
                spawnedObject.transform.localScale = Vector3.one * scaleSlider.value;
            }
            else spawnedObject.transform.position = hitPose.position;
        }
    }
}
