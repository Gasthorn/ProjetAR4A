using UnityEngine;

public class TutorialTiles : MonoBehaviour
{
    public GameObject[] previewTiles; 
    public GameObject plateau;

    private int index = 0;

    void Start()
    {
        foreach (var tile in previewTiles)
        {
            tile.SetActive(false);
        }
        foreach (Transform tile in plateau.transform)
        {
            tile.gameObject.SetActive(false);
        }
        ShowPreview();
    }

    public void NextStep()
    {
        if (index >= previewTiles.Length) return;
        string previewName = previewTiles[index].name;

        foreach (Transform tile in plateau.transform)
        {
            if(tile.name.Contains(previewName)) tile.gameObject.SetActive(true);
        }
        previewTiles[index].SetActive(false);
        index++;
        if(index < previewTiles.Length) previewTiles[index].SetActive(true);
    }

    void ShowPreview()
    {
        previewTiles[index].SetActive(true);
    }
}