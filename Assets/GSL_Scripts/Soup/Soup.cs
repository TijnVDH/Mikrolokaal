using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Soup : NetworkBehaviour
{
	[Header("Config")]
    public int MinimumFoodCount;
    public float ConsumeRate;
	public int AllowedFoodDifference;
	public List<FoodType> RequiredFoodTypes;

	[Header("Bubbles")]
	public ParticleSystem RisingBubbleParticles;
	public ParticleSystem SurfaceBubbleParticles;
	public int MinRisingBubbleCount;
	public int MaxRisingBubbleCount;
	public int MinSurfaceBubbleCount;
	public int MaxSurfaceBubbleCount;

	[Header("Sprites")]
	public SpriteRenderer SoupSprite;
	public Sprite InactiveSoupSprite;
	public Sprite UnstableSoupSprite;
	public Sprite StableSoupSprite;

	[Header("Indicator")]
	public SoupIndicator Indicator;
	public int UrgentThreshold = 3;

	public int MaxFoodDifference { get => soupContent.Values.Max() - soupContent.Values.Min(); }

	public Dictionary<FoodType, int> soupContent = new Dictionary<FoodType, int>();

	public static SoupStateAction OnStateChange;
	public delegate void SoupStateAction(SoupState state);

	public static SoupContentAction OnSoupContentChange;
	public delegate void SoupContentAction(int square, int circle, int triangle);

	private SoupState CurrentState;

	ScoreCounter scoreScript;

	public GameObject pointsPopUpField;
	PointsPopup pointsScript;

	private void Start()
	{
		scoreScript = GameObject.Find("Score").GetComponent<ScoreCounter>();

		CurrentState = SoupState.INACTIVE;
		SoupSprite.sprite = InactiveSoupSprite;

		// populate soupContent dictionary and create indicators
		Indicator.HideAll();

		UpdateIndicators();

		if (isServer)
		{
			StartCoroutine(ConsumeFood());
		}
	}

	public int GetFoodCount(FoodType type)
	{
		if (soupContent.ContainsKey(type))
		{
			return soupContent[type];
		}
		else
		{
			return 0;
		}
	}

	private void OnTriggerEnter(Collider other) {

		// only the server triggers the trigger
		if (!isServer) return;

		Food food = other.gameObject.GetComponent<Food>();
		if (food != null) 
		{
			NetworkServer.Destroy(food.gameObject);
			RpcAddFood(food.FoodType, 1);
		}
	}

	[ClientRpc] public void RpcAddFood(FoodType type, int amount)
	{
		if (!RequiredFoodTypes.Contains(type))
		{
			Debug.Log("Soup: Food type not accepted");
			return;
		}

		if (soupContent.ContainsKey(type))
		{
			soupContent[type] += amount;
		}
		else
		{
			soupContent.Add(type, amount);
		}

		Debug.Log("==SOUP CONTENT==");
		foreach (KeyValuePair<FoodType, int> foodkvp in soupContent)
		{
			Debug.Log(foodkvp.Key + ": " + foodkvp.Value);
		}

		AnimateBubbles();

		UpdateSoupState();
		UpdateIndicators();
		if (isServer) UpdateHostIndicators();
	}

	[ClientRpc] public void RpcRemoveFood(FoodType type, int amount)
	{
		Debug.Log("soup: removing food " + type);
		if (soupContent.ContainsKey(type))
		{
			Debug.Log("soup: removed it");
			soupContent[type] -= amount;
			if (soupContent[type] < 0) soupContent[type] = 0;
		}

		UpdateSoupState();
		Debug.Log("Soup: RpcRemoveFood, updating indicators");
		UpdateIndicators();
		if (isServer) UpdateHostIndicators();
	}

	public IEnumerator ConsumeFood()
	{
		while (true)
		{
			// only the server should make the soup consume
			if (!isServer)
			{
				Debug.Log("Soup: I'm not on the server. stop consuming");
				break;
			}

			// don't consume food if the soup is inactive
			if (CurrentState == SoupState.INACTIVE)
			{
				yield return null;
			}
			else
			{
				yield return new WaitForSeconds(ConsumeRate);
				RpcRemoveFood(RequiredFoodTypes[Random.Range(0, RequiredFoodTypes.Count)], 1);
			}
		}
	}

	private void UpdateSoupState()
	{
		// is enough food present?
		if (soupContent.Sum(x => x.Value) >= MinimumFoodCount)
		{
			// is the food properly balanced?
			if (MaxFoodDifference <= AllowedFoodDifference)
			{
				// soup is just right, becomes stable
				ChangeState(SoupState.STABLE);
			}
			else
			{
				// soup is out of balance, becomes unstable
				ChangeState(SoupState.UNSTABLE);
			}
		}
		else
		{
			// not enough food, soup becomes inactive
			ChangeState(SoupState.INACTIVE);
		}
	}

	private void AnimateBubbles() {
		SurfaceBubbleParticles.Emit(Random.Range(MinSurfaceBubbleCount, MaxSurfaceBubbleCount));
		RisingBubbleParticles.Emit(Random.Range(MinRisingBubbleCount, MaxRisingBubbleCount));
	}

	private void UpdateIndicators()
	{
		for (int i = 0; i < RequiredFoodTypes.Count; i++)
		{
			int shortage = GetFoodShortage(RequiredFoodTypes[i]);
			Debug.Log("Soup: Shortage of " + shortage + " of type " + RequiredFoodTypes[i]);
			if (shortage > 0)
			{
				bool isUrgent = true;
				if (shortage < UrgentThreshold)
					isUrgent = false;
				Indicator.ShowFood(RequiredFoodTypes[i], isUrgent);
			}
			else
			{
				Indicator.HideFood(RequiredFoodTypes[i]);
			}
		}
	}

	public void UpdateHostIndicators()
	{
		Debug.Log("firing host indicator event");
		OnSoupContentChange?.Invoke(
			GetFoodCount(FoodType.SQUARE),
			GetFoodCount(FoodType.CIRCLE),
			GetFoodCount(FoodType.TRIANGLE)
		);
	}

	private int GetFoodShortage(FoodType foodType)
	{
		if (soupContent.ContainsKey(foodType))
		{
			return soupContent.Values.Max() - soupContent[foodType];
		} 
		else
		{
			if (soupContent.Count > 0)
				return soupContent.Values.Max();
			else 
				return 0;
		}
	}

	private void ChangeState(SoupState newState)
	{
		if (CurrentState == newState) return;
		CurrentState = newState;

		switch (newState)
		{
			case SoupState.INACTIVE:
				SoupSprite.sprite = InactiveSoupSprite;
				//StopSpawners();
				break;
			case SoupState.STABLE:
				SoupSprite.sprite = StableSoupSprite;
				//StartSpawners();
				break;
			case SoupState.UNSTABLE:
				SoupSprite.sprite = UnstableSoupSprite;
				//StopSpawners();
				break;
			default:
				Debug.LogErrorFormat("Unhandled state '{0}'", newState.ToString());
				break;
		}

		OnStateChange(newState);
	}

	public Dictionary<FoodType, int> getSoupContents()
    {
		return soupContent;
    }
}