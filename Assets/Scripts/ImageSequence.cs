using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageSequence : MonoBehaviour
{
    public RawImage canvasImage;
    public Texture firstImage;
    public Texture secondImage;
    public GameObject firstCanvas;

    void Start()
    {
        StartCoroutine(ShowImageSequence());
    }

    IEnumerator ShowImageSequence()
    {
        firstCanvas.gameObject.SetActive(true);
        canvasImage.texture = firstImage;
        canvasImage.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(5f);
        
        canvasImage.texture = secondImage;
        
        yield return new WaitForSeconds(5f);
        
        // hiding
        canvasImage.gameObject.SetActive(false);
        firstCanvas.gameObject.SetActive(false);
    }
}