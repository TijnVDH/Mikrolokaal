using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;

public class SoupHostControls : MonoBehaviour
{
    public Soup Soup;

	public TextMeshProUGUI CurrentSquareCount;
	public TextMeshProUGUI CurrentCircleCount;
	public TextMeshProUGUI CurrentTriangleCount;

	void Start()
	{
		CurrentSquareCount.text = "0";
		CurrentCircleCount.text = "0";
		CurrentTriangleCount.text = "0";

		Soup.OnSoupContentChange += UpdateCurrentSoupAmount;
	}

    public void AddCircle()
	{
		Soup.RpcAddFood(FoodType.CIRCLE, 1);
	}

	public void AddSquare()
	{
		Soup.RpcAddFood(FoodType.SQUARE, 1);
	}

	public void AddTriangle()
	{
		Soup.RpcAddFood(FoodType.TRIANGLE, 1);
	}

	public void UpdateCurrentSoupAmount(int square, int circle, int triangle)
	{
		Debug.Log("updating indicators");
		CurrentSquareCount.text = square.ToString();
		CurrentCircleCount.text = circle.ToString();
		CurrentTriangleCount.text = triangle.ToString();
	}
}
