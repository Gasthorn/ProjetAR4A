using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject confirmButton;
    public GameObject tutorialCanvas;
    public GameObject tutorialManager;
    public GameObject arSessionOrigin;
    private bool placementLocked = false;
    public GameObject plateau;
    public Slider scaleSlider;
    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    private bool plateauShown = false;
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
        if(placementLocked) return;
        if(!TryGetTouchPosition(out Vector2 touchPosition)) return;
        if(_arRaycastManager.Raycast(touchPosition,hits,trackableTypes:TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if (!plateauShown){
                plateau.SetActive(true);
                plateauShown = true;
            }
            plateau.transform.position = hitPose.position;
            plateau.transform.rotation = hitPose.rotation;
            if (scaleSlider != null)
            {
                plateau.transform.localScale = Vector3.one * scaleSlider.value;
            }
        }
    }

    public void ConfirmPlacement()
    {
        placementLocked = true;
        
        if(confirmButton != null)
            confirmButton.SetActive(false);
        if(tutorialCanvas != null)
            tutorialCanvas.SetActive(true);
        if(tutorialManager != null)
            tutorialManager.SetActive(true);
    }
    public void OnScaleSliderChanged()
    {
        if (plateau != null)
        {
            plateau.transform.localScale = Vector3.one * scaleSlider.value;
        }
    }
}
