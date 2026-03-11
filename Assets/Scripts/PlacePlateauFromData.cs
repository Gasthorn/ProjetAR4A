using UnityEngine;

public class PlacePlateauFromData : MonoBehaviour
{
    void Start()
    {
        transform.position = PlateauData.position;
        transform.rotation = PlateauData.rotation;
    }
}