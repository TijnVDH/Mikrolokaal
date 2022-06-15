using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
 
public class PointsPopup : MonoBehaviour
{   
    public float lifeDuration;
    public Image img;

    private void Start()
    {
        StartCoroutine(FadeImageToZeroAlpha(lifeDuration));
        StartCoroutine(Die(lifeDuration));
    }
    public IEnumerator FadeImageToZeroAlpha(float t)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
        while (img.color.a > 0.0f)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator Die(float deathDelay)
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
