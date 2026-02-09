using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTrackingManager : MonoBehaviour
{
    public void ImageTracking()
    {
        SceneManager.LoadScene("ImageTracking");
    }
    public void PlaneTracking()
    {
        SceneManager.LoadScene("PlaneTracking");
    }
    public void Back()
    {
        SceneManager.LoadScene("MenuTracking");
    }
}
