using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Config Parameters
    [SerializeField] Image healthBarImage = null;
    [SerializeField] Image shieldImage = null;
    [SerializeField] float shieldFillTime = 1f;
    [SerializeField] float shieldFillResolution = 0.01f;

    public void UpdateHealthBar(float fractionRemaining)
    {
        healthBarImage.fillAmount = fractionRemaining;
    }

    public void TurnOffShields()
    {
        StartCoroutine(FillDownHealthBar());
    }

    public void TurnOnShields()
    {
        StartCoroutine(FillUpHealthBar());
    }

    private IEnumerator FillUpHealthBar()
    {
        shieldImage.fillAmount = 0;

        for (int i = 0; i < shieldFillTime / shieldFillResolution; i++)
        {
            float newFillAmount = (float)i * shieldFillResolution / shieldFillTime;

            if (newFillAmount > 1)
            {
                newFillAmount = 1;
            }

            shieldImage.fillAmount = newFillAmount;

            yield return new WaitForSeconds(shieldFillResolution / shieldFillTime);
        }

        shieldImage.fillAmount = 1;
    }

    private IEnumerator FillDownHealthBar()
    {
        shieldImage.fillAmount = 1;

        for (int i = 0; i < shieldFillTime / shieldFillResolution; i++)
        {
            float newFillAmount = 1 - (float)i * shieldFillResolution / shieldFillTime;

            if (newFillAmount < 0)
            {
                newFillAmount = 0;
            }

            shieldImage.fillAmount = newFillAmount;

            yield return new WaitForSeconds(shieldFillResolution / shieldFillTime);
        }

        shieldImage.fillAmount = 0;
    }
}
