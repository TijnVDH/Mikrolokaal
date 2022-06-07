using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class Food : NetworkBehaviour
{
    [Header("Food Type")]
    [SerializeField] [SyncVar] private FoodType type;
	[SerializeField] private bool RandomiseTypeOnStart;
    [SerializeField] private Sprite squareSprite;
    [SerializeField] private Sprite triangleSprite;
    [SerializeField] private Sprite circleSprite;

	[Header("Wobble")]
	public Ease WobbleEase;
	public float WobbleDuration;
	public float WobbleStrength;

	public FoodType FoodType { get => type; }

    public float ImmuneAfterDropTime { get => 2f; }

    public bool CanBeCollected { get => canBeCollected; }

    // components
    private SpriteRenderer spriteRenderer;

    private bool canBeCollected = true;

    private List<Tween> wobbleTweens = new List<Tween>();

	private AudioSource pickupSound;
	void Start()
    {
		if(RandomiseTypeOnStart && isServer)
		{
			Array foodTypes = Enum.GetValues(typeof(FoodType));
			type = (FoodType)foodTypes.GetValue(Random.Range(0, foodTypes.Length));
		}

		SetImmune();
		InitSprite();
		StartWobble();

		pickupSound = GetComponent<AudioSource>();
	}

	public void Collect(GameObject collector)
	{
		Debug.Log("Food collected");
		StopWobble();

		//play collect sound
		pickupSound.Play(0);
	}

	private void InitSprite()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		switch (type)
		{
			case FoodType.CIRCLE:
				spriteRenderer.sprite = circleSprite;
				break;
			case FoodType.SQUARE:
				spriteRenderer.sprite = squareSprite;
				break;
			case FoodType.TRIANGLE:
				spriteRenderer.sprite = triangleSprite;
				break;
			default:
				break;
		}
	}

	private void StartWobble()
	{
		// wobble height. requires both y and z because the sprite is slanted
		wobbleTweens.Add(transform.DOScaleY(transform.localScale.y * WobbleStrength, WobbleDuration).SetLoops(-1, LoopType.Yoyo).SetEase(WobbleEase));
		wobbleTweens.Add(transform.DOScaleZ(transform.localScale.z * WobbleStrength, WobbleDuration).SetLoops(-1, LoopType.Yoyo).SetEase(WobbleEase));

		// wobble width. delay start so width and height alternation their wobble
		wobbleTweens.Add(transform.DOScaleX(transform.localScale.x * WobbleStrength, WobbleDuration).SetLoops(-1, LoopType.Yoyo).SetEase(WobbleEase).SetDelay(WobbleDuration));
	}

	private void StopWobble()
	{
		foreach (Tween tween in wobbleTweens)
		{
			tween.Kill();
		}
		wobbleTweens.Clear();
	}

    public void SetImmune()
    {
        canBeCollected = false;
        StartCoroutine(AfterDropImmunityTimer());
    }

    private IEnumerator AfterDropImmunityTimer()
    {
        yield return new WaitForSeconds(ImmuneAfterDropTime);
		canBeCollected = true;
    }
}
