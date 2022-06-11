using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
 
public class PointsPopup : MonoBehaviour
{   
    public TextMeshPro pointsText;
    float lifeDuration = 2;
    

    private void Awake()
    {
        pointsText = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        StartCoroutine(FadeTextToZeroAlpha(lifeDuration));
        StartCoroutine(Die(lifeDuration));
    }

    private void Update()
    {
        gameObject.transform.position = new Vector3(0, 0, 2) * Time.deltaTime;
    }
    public IEnumerator FadeTextToZeroAlpha(float t)
    {
        pointsText.color = new Color(pointsText.color.r, pointsText.color.g, pointsText.color.b, 1);
        while (pointsText.color.a > 0.0f)
        {
            pointsText.color = new Color(pointsText.color.r, pointsText.color.g, pointsText.color.b, pointsText.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator Die(float deathDelay)
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
