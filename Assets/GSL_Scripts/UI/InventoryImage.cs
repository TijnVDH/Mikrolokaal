using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryImage : MonoBehaviour
{
    public Image Image;
    public bool IsEmpty;
    public FoodType CurrentType;

    private bool isAnimating;

    private Sequence currentSequence;

    public void SetEmpty()
    {
        Image.sprite = null;
        IsEmpty = true;
    }

    public void SetCollectable(FoodType type, Sprite collectableSprite)
    {
        SetCollectable(type, collectableSprite, false);
    }

    public void SetCollectable(FoodType type, Sprite collectableSprite, bool animate)
    {
        Image.sprite = collectableSprite;
        CurrentType = type;
        IsEmpty = false;
        
        if (animate && !isAnimating)
        { 
            StartAnimation();
        }
        else if (!animate && isAnimating)
        {
            StopAnimation();
        }
    }

    private void StartAnimation()
    {
        currentSequence = DOTween.Sequence();
        currentSequence
            .Append(Image.gameObject.transform.DOPunchPosition(new Vector3(0, 2, 0), .5f))
            .Append(Image.gameObject.transform.DOPunchRotation(new Vector3(0, 0, 25), .5f, 15, 2))
            .SetDelay(1);
        currentSequence.SetLoops(-1, LoopType.Restart);

        isAnimating = true;

        // Image.gameObject.transform.DOPunchScale(new Vector3(.1f, .1f, .1f), .5f, 2)
        // .SetLoops(-1)
        // .SetDelay(.5f);
    }

    private void StopAnimation()
    {
        currentSequence.Kill();

        isAnimating = false;
    }
}
