using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialTiles : MonoBehaviour
{
    public GameObject[] previewTiles; 
    public GameObject plateau;

    public GameObject panel;

    public TMP_Text descriptionText; 
    public string[] descriptions;

    private int index = 0;
    private Vector3 plateauScale;

    void Start()
    {
        panel.gameObject.SetActive(true);
        plateauScale = Vector3.one * 0.1f;
        foreach (var tile in previewTiles)
        {
            tile.SetActive(false);
            tile.transform.localScale = plateauScale;
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
        Transform targetTile = null;
        foreach (Transform tile in plateau.transform)
        {
            if(tile.name.Contains(previewName)){
                tile.gameObject.SetActive(true);
                if(targetTile == null) targetTile = tile;
            } 
        }
        GameObject preview = previewTiles[index];
        if (targetTile != null)
        {
            StartCoroutine(AnimateToPlateau(preview, targetTile));
        }
        index++;
        if(index < previewTiles.Length) {
            ShowPreview();
        }
        else{
            descriptionText.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
        }
    }

    void ShowPreview()
    {
        previewTiles[index].SetActive(true);
        if (descriptionText != null && index < descriptions.Length)
        {
            descriptionText.text = descriptions[index];
        }
    }

    IEnumerator AnimateToPlateau(GameObject preview, Transform target)
    {
        float duration = 0.6f;
        float t = 0;

        Vector3 startPos = preview.transform.position;
        Vector3 endPos = target.position;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            preview.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        target.gameObject.SetActive(true);
        Destroy(preview);
    }
}