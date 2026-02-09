using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageRecognitionExample : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARTrackedImageManager arTrackedImageManager;

    [Header("Prefab to Spawn")]
    [SerializeField] private GameObject prefabToSpawn;

    [Header("Scale Settings")]
    [SerializeField] private float scaleMultiplier = 0.05f;

    // Dictionnaire pour garder une référence aux objets instanciés par image
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

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

        foreach (var trackedImage in args.updated)
        {
            if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject obj))
            {
                obj.transform.position = trackedImage.transform.position;
                obj.transform.rotation = trackedImage.transform.rotation;

                float imageSize = trackedImage.size.x; 
                obj.transform.localScale = Vector3.one * imageSize * scaleMultiplier;
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

        float imageSize = trackedImage.size.x; 
        obj.transform.localScale = Vector3.one * imageSize * scaleMultiplier;

        spawnedObjects[trackedImage.referenceImage.name] = obj;
    }
}
