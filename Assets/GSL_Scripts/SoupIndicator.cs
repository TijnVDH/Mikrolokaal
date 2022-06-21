using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoupIndicator : MonoBehaviour
{
	public GameObject IndicatorBackground;
	public List<InventoryImage> FoodImages;
    public List<InventoryUISpriteFood> FoodSprites;

	public void ShowFood(FoodType type, bool urgent)
	{
		if (IndicatorBackground.activeSelf == false)
			IndicatorBackground.SetActive(true);

		InventoryImage image = FoodImages.Find(fi => fi.CurrentType == type && !fi.IsEmpty);
		if (image == null)
		{
			image = FoodImages.Find(fi => !fi.gameObject.activeSelf);
			image.gameObject.SetActive(true);
		}
		image.SetCollectable(type, FoodSprites.Find(s => s.FoodType == type).Sprite, urgent);
	}

	public void HideFood(FoodType type)
	{
		InventoryImage image = FoodImages.Find(fi => fi.CurrentType == type);
		if (image == null)
			return;

		image.SetEmpty();
		image.gameObject.SetActive(false);

		// Check for think bubble to disappear
		if (FoodImages.Find(fi => !fi.IsEmpty) == null)
		{
			// Found none who are filled
			IndicatorBackground.SetActive(false);
		}
	}

	public void HideAll()
	{
		for (int i = 0; i < FoodImages.Count; i++)
		{
			FoodImages[i].SetEmpty();
			FoodImages[i].gameObject.SetActive(false);
		}

		IndicatorBackground.SetActive(false);
	}
}
