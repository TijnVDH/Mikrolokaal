using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteToggle : MonoBehaviour
{
    public Image Image;
    public Sprite DefaultSprite;
    public Sprite ToggledSprite;

    private bool isToggled = false;

    private void Start() {
        Image.sprite = DefaultSprite;
    }

    public void Toggle() {
        isToggled = !isToggled;
        
        Image.sprite = isToggled ? ToggledSprite : DefaultSprite; 
    }
}
