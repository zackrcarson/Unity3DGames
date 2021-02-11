using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDamage : MonoBehaviour
{
    // Config Parameters
    [SerializeField] Canvas bloodSplatterCanvas = null;
    [SerializeField] float bloodSplatterTime = 0.3f;
    [SerializeField] float bloodSplatterDecayRate = 3f;

    // Cached References
    Image[] bloodSplatterImages = null;

    // Start is called before the first frame update
    void Start()
    {
        bloodSplatterImages = bloodSplatterCanvas.GetComponentsInChildren<Image>();

        bloodSplatterCanvas.enabled = false;
    }

    public void ShowDamage()
    {
        StopAllCoroutines();

        bloodSplatterCanvas.enabled = false;
        DisableAllBloodSplatters();

        StartCoroutine(ShowSplatter());
    }

    private IEnumerator ShowSplatter()
    {
        bloodSplatterCanvas.enabled = true;

        Image tempImage = GetRandomSplatter();
        tempImage.enabled = true;

        yield return new WaitForSeconds(bloodSplatterTime);

        Color tempColor = tempImage.color;
        while (tempColor.a >= 0)
        {
            tempColor.a -= Time.deltaTime * bloodSplatterDecayRate;
            tempImage.color = tempColor;
            yield return null;
        }
    }

    private void DisableAllBloodSplatters()
    {
        foreach (Image image in bloodSplatterImages)
        {
            image.enabled = false;

            Color tempColor = image.color;
            tempColor.a = 1;
            image.color = tempColor;
        }
    }

    private Image GetRandomSplatter()
    {
        int splatterIndex = Random.Range(0, bloodSplatterImages.Length);

        return bloodSplatterImages[splatterIndex];
    }
}
