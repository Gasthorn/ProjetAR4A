using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [Header("Hex settings")]
    public float hexWidth = 1.04f;
    public float hexHeight = 0.1f;


    private Vector2Int[] layout =
    {
        new Vector2Int(-2, 2), new Vector2Int(-1, 2), new Vector2Int(0, 2),
        new Vector2Int(-2, 1), new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1),
        new Vector2Int(-2, 0), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0),
        new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1), new Vector2Int(2, -1),
        new Vector2Int(0, -2), new Vector2Int(1, -2), new Vector2Int(2, -2)
    };

    void Start(){
        ArangeBoard();
    }

    void ArangeBoard(){
        if (transform.childCount != layout.Length)
        {
            Debug.LogWarning("Le nombre de tuiles dans Board ne correspond pas au layout !");
            return;
        }

        for (int i = 0; i < layout.Length; i++)
        {
            Transform hex = transform.GetChild(i);
            float currentY = hex.localPosition.y;
            hex.localPosition = GridToWorld(layout[i],currentY);
        }
    }

    Vector3 GridToWorld(Vector2Int gridPos, float currentY)
    {
        float x = gridPos.x * hexWidth * 0.75f;
        float z = (gridPos.y + gridPos.x * 0.5f) * hexHeight;
        return new Vector3(x, currentY, z);
    }
    
}
