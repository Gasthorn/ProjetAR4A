using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ImageRecognitionExample : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARTrackedImageManager arTrackedImageManager;

    [Header("Prefab to Spawn")]
    [SerializeField] private GameObject prefabToSpawn;

    [Header("UI")]
    public GameObject confirmButton;
    public Slider scaleSlider;
    public GameObject tutorialCanvas;
    public GameObject tutorialManager;
    public TutorialTiles tutorialScript;

    [Header("Scale Settings")]
    [SerializeField] private float scaleMultiplier = 0.05f;

    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    private bool placementLocked = false;

    private float currentSliderValue = 1f;

    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            if (!spawnedObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                SpawnObject(trackedImage);
            }
        }

        if (!placementLocked)
        {
            foreach (var trackedImage in args.updated)
            {
                if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
                {
                    obj.transform.position = trackedImage.transform.position;
                    obj.transform.rotation = trackedImage.transform.rotation;

                    UpdateScale(obj, trackedImage);
                }
            }
        }

        foreach (var trackedImage in args.removed)
        {
            if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
            {
                Destroy(obj);
                spawnedObjects.Remove(trackedImage.referenceImage.name);
            }
        }
    }

    private void SpawnObject(ARTrackedImage trackedImage)
    {
        GameObject obj = Instantiate(prefabToSpawn, trackedImage.transform.position, trackedImage.transform.rotation);
        obj.transform.SetParent(trackedImage.transform);

        UpdateScale(obj, trackedImage);

        spawnedObjects[trackedImage.referenceImage.name] = obj;
    }

    private void UpdateScale(GameObject obj, ARTrackedImage trackedImage)
    {
        float imageSize = trackedImage.size.x;
        obj.transform.localScale = Vector3.one * imageSize * scaleMultiplier * currentSliderValue;
    }

    public void ConfirmPlacement()
    {
        placementLocked = true;

        GameObject plateauInstance = null;

        foreach (var kvp in spawnedObjects)
        {
            plateauInstance = kvp.Value;
            plateauInstance.transform.SetParent(null);
        }

        // 🔥 IMPORTANT : envoie au tuto
        if (tutorialScript != null && plateauInstance != null)
        {
            tutorialScript.SetPlateau(plateauInstance);
        }

        if(confirmButton != null)
            confirmButton.SetActive(false);

        if(tutorialCanvas != null)
            tutorialCanvas.SetActive(true);

        if(tutorialManager != null)
            tutorialManager.SetActive(true);
    }

    public void OnScaleSliderChanged()
    {
        currentSliderValue = scaleSlider.value;

        foreach (var kvp in spawnedObjects)
        {
            GameObject obj = kvp.Value;

            ARTrackedImage trackedImage = obj.GetComponentInParent<ARTrackedImage>();

            if (trackedImage != null)
            {
                UpdateScale(obj, trackedImage);
            }
        }
    }
}