using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public List<InventoryImage> FoodImages;
    public List<InventoryUISpriteFood> FoodSprites;

    public void AddItem(FoodType foodType)
    {
        InventoryImage image = FoodImages.Find(fi => fi.gameObject.activeSelf == false);
        image.gameObject.SetActive(true);
        image.SetCollectable(foodType, FoodSprites.Find(s => s.FoodType == foodType).Sprite);
    }

    public void ResetSprites()
    {
        for (int i = 0; i < FoodImages.Count; i++)
        {
            FoodImages[i].SetEmpty();
            FoodImages[i].gameObject.SetActive(false);
        }
    }


}
