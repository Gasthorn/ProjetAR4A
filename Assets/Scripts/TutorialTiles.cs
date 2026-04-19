using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialTiles : MonoBehaviour
{
    [Header("References")]
    public GameObject plateau; 
    public Transform[] allParents;

    [Header("Preview")]
    public GameObject[] previewTiles;

    [Header("UI")]
    public GameObject panel;
    public TMP_Text descriptionText;
    public string[] descriptions;

    private int index = 0;
    private Vector3 previewScale = Vector3.one * 0.1f;
    private bool isInitialized = false;

    void Start()
    {
        if (plateau != null)
        {
            InitTutorial();
        }
    }

    public void SetPlateau(GameObject newPlateau)
    {
        plateau = newPlateau;
        InitTutorial();
    }

    void InitTutorial()
    {
        if (isInitialized) return;
        isInitialized = true;

        panel.SetActive(true);

        if (allParents == null || allParents.Length == 0)
        {
            allParents = plateau.GetComponentsInChildren<Transform>();
        }

        foreach (var preview in previewTiles)
        {
            preview.SetActive(false);
            preview.transform.localScale = previewScale;

            preview.transform.SetParent(plateau.transform);
        }

        foreach (Transform parent in allParents)
        {
            if (parent == null) continue;

            parent.gameObject.SetActive(true);

            foreach (Transform obj in parent)
            {
                obj.gameObject.SetActive(false);
            }
        }

        index = 0;
        ShowPreview();
    }

    public void NextStep()
    {
        if (!isInitialized || index >= previewTiles.Length) return;

        string previewName = previewTiles[index].name.Replace("(Clone)", "").Replace("Preview_", "");

        Transform firstTarget = null;
        var allTargets = new System.Collections.Generic.List<Transform>();

        foreach (Transform parent in allParents)
        {
            if (parent == null) continue;

            foreach (Transform obj in parent)
            {
                string objName = obj.name.Replace("(Clone)", "");

                if (objName.Contains(previewName))
                {
                    allTargets.Add(obj);

                    if (firstTarget == null)
                        firstTarget = obj;
                }
            }
        }

        GameObject preview = previewTiles[index];

        if (firstTarget != null)
        {
            StartCoroutine(AnimateToTarget(preview, firstTarget));
        }

        foreach (var target in allTargets)
        {
            target.gameObject.SetActive(true);
        }

        index++;

        if (index < previewTiles.Length)
        {
            ShowPreview();
        }
        else
        {
            EndTutorial();
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

    IEnumerator AnimateToTarget(GameObject preview, Transform target)
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

    void EndTutorial()
    {
        if (descriptionText != null)
            descriptionText.gameObject.SetActive(false);

        panel.SetActive(false);
    }
}