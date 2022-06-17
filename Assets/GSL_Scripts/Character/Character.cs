using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Only functions that are used by the player/virus controller
[RequireComponent(typeof(NetworkTransform))]
public class Character : NetworkBehaviour
{
    [Header("Host Player")]
    public Vector3 CameraPosition;

    [Header("Type")]
    public CharacterType CharacterType;
    public FoodType VirusType;

    [Header("Combat")]
    public float ImmunityTime;
    public float BlinkSpeed;

    [Header("Stats")]
    public float MovementSpeed;
    public float AttackStrength;
    public int InventorySlots;
    public UpgradeType ActiveUpgrade = UpgradeType.NONE;

    [Header("Inventory")]
    public Inventory FoodInventory;
    public InventoryDisplay FoodInventoryDisplay;

    [Header("Forms")]
    [SerializeField] private SpriteRenderer formSpriteRenderer;
    [SerializeField] private List<CharacterForm> forms; // forms should be sorted by priority

    [Header("Movement")]
    public TouchMovement Movement;
    public float WobbleHeightStrength;
    public float WobbleWidthStrength;
    public float WobbleDuration;
    public Ease WobbleEase;

    [Header("Upgrades")]
    public float UpgradedAttack;
    public float UpgradedSpeed;
    public int UpgradedSlots;

    private float defaultAttack;
    private float defaultSpeed;
    private int defaultSlots;


    // combat
    private bool isImmune = false;

    private List<Tween> wobbleTweens = new List<Tween>();

    [SyncVar] private bool isServerCharacter = false;

    [Header("Audio")]
    public AudioClip enemyDefeat1;
    public AudioClip enemyDefeat2;
    public AudioClip enemyDefeat3;
    public AudioClip speedUpgradePickup;
    public AudioClip attackUpgradePickup;
    public AudioClip carryUpgradePickup;
    public AudioClip itemPickup1;
    public AudioClip itemPickup2;
    public AudioClip itemPickup3;

    AudioSource audioSrc;

    bool justFought = false;
    public void Awake()
    {
        if(CharacterType == CharacterType.Player)
        {
            // add an audiolistener to the local scene
            GameObject.Find("Camera").AddComponent(typeof(AudioListener));

            audioSrc = GameObject.Find("Camera").GetComponent<AudioSource>();
        }
        
        defaultAttack = AttackStrength;
        defaultSlots = InventorySlots;
        defaultSpeed = MovementSpeed;

        if (CharacterType == CharacterType.Virus)
        {
            switch (VirusType)
            {
                case FoodType.CIRCLE:
                    FoodInventory.AddToInventory(FoodType.CIRCLE);
                    break;
                case FoodType.SQUARE:
                    FoodInventory.AddToInventory(FoodType.SQUARE);
                    break;
                case FoodType.TRIANGLE:
                    FoodInventory.AddToInventory(FoodType.TRIANGLE);
                    break;
                default:
                    break;
            }
        }

        ChangeInventorySlots(defaultSlots);

        if (Movement != null)
        {
            Movement.OnStartMoving += StartWobble;
            Movement.OnStopMoving += StopWobble;
            Movement.OnMove += SetSpriteFacing;
        }
        else
        {
            StartWobble();
        }

        // initialise form
        ChangeForm();
    }

    public void Start()
    {
        // Host-specific setup has to be done in Start. If done in Awake, "isServer" will always be false.
        if (isServer && CharacterType == CharacterType.Player && isLocalPlayer)
        {
            // zoom out
            Camera cam = GetComponentInChildren<Camera>();
            cam.transform.position = CameraPosition;

            // disable character movement
            Movement.enabled = false;

            isServerCharacter = true;
        }

        if (CharacterType == CharacterType.Player && isLocalPlayer)
        {
            Canvas.FindObjectOfType<InventoryUI>().Init(this);
            InventorySlots = FoodInventory.CurrentSlotsAmount;
        }

        if(isServerCharacter) {
            HideCharacter();
        }
    }

    private void HideCharacter() {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        this.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        // colliding with a character?
        Character enemy = collision.gameObject.GetComponent<Character>();
        if (enemy != null)
        {
            // only handle fights from player perspective
            if (CharacterType == CharacterType.Virus)
                return;

            if(!isServer && isLocalPlayer && AttackStrength >= enemy.AttackStrength && !isImmune && !enemy.isImmune && !justFought)
            {
                PlayEnemyDefeatAudio();
                justFought = true;
            }
            Fight(enemy);
        }
    }

    private void OnTriggerEnter(Collider other) {
        // only handle triggers from the local player
        if(!isLocalPlayer) return;

        // colliding with food?
        Food food = other.gameObject.GetComponent<Food>();
        if (food != null) 
        {
            // ask the server to handle the pickup
            CmdPickUpFood(food);

            if(FoodInventory.HasSpace())
            {
                //pickup food sound effect
                PlayItemPickupAudio();
            }
        }

        Upgrader upgrader = other.gameObject.GetComponent<Upgrader>();
        if (upgrader != null) 
        {
            // ask the server to handle the upgrade
            CmdUpgrade(upgrader.UpgradeType);

            //play upgrade sound effect
            if(upgrader.UpgradeType != ActiveUpgrade)
                PlayUpgradeAudio(upgrader.UpgradeType);
        }
    }

    [Command] public void CmdPickUpFood(Food food)
    {
        // when two players collide with food simultaneously it might ask the server to pick up
        // food that was already destroyed on the server side, but hasn't synced to client side yet 
        if (food == null)
        {
            Debug.LogWarning("Tried picking up food that no longer exists");
            return;
        }

        if (FoodInventory.HasSpace()) {
            // destroy food object on all clients
            NetworkServer.Destroy(food.gameObject);

            // tell all clients to update this character's inventory
            RpcAddToInventory(food.FoodType);
        }
    }

    [ClientRpc] public void RpcAddToInventory(FoodType food) 
    {
        if (FoodInventory.HasSpace()) {
            FoodInventory.AddToInventory(food);
            FoodInventoryDisplay.AddItem(food);
        }
    }

    public void DropInventory() 
    {
        // ask server to handle dropping inventory
        CmdDropInventory();
    }

    [Command] public void CmdDropInventory() 
    {
        // tell all clients that this character needs to drop it's inventory
        RpcDropInventory();
    }

    [ClientRpc] public void RpcDropInventory() 
    {
        FoodInventory.DropAll();
        FoodInventoryDisplay.ResetSprites();
    }

    [Command] public void CmdUpgrade(UpgradeType newUpgrade) 
    {
        RpcUpgrade(newUpgrade);
    }

    [ClientRpc] public void RpcUpgrade(UpgradeType newUpgrade) 
    {
        ActiveUpgrade = newUpgrade;
        AttackStrength = newUpgrade.Equals(UpgradeType.ATTACK) ? UpgradedAttack : defaultAttack;
        MovementSpeed = newUpgrade.Equals(UpgradeType.SPEED) ? UpgradedSpeed : defaultSpeed;
        ChangeInventorySlots(newUpgrade.Equals(UpgradeType.CARRY) ? UpgradedSlots : defaultSlots);
        ChangeForm();
    }

    public void ChangeInventorySlots(int newAmount) {
        FoodInventory.ChangeSlotsAmount(newAmount);
        FoodInventoryDisplay.ResetSprites();
        foreach (FoodType food in FoodInventory.Items)
        {
            FoodInventoryDisplay.AddItem(food);
        }
    }

    private void Fight(Character enemy)
    {
        // Server handles fighting
        if (!isServer)
            return;

        // No combat if anyone is immune
        if (isImmune || enemy.isImmune)
            return;
        // No infighting
        if (CharacterType == enemy.CharacterType)
        {
            return;
        }

        // Compare the attack values
        if (enemy.AttackStrength > AttackStrength)
        {
            // virus wins
            RpcTakeDamage();
        }
        else
        {
            // player wins
            NPC npc = enemy.GetComponent<NPC>();
            if (npc != null)
            {
                npc.RemoveFromSpawner();
            }
            NetworkServer.Destroy(enemy.gameObject);
        }

        // Set immune for x seconds
        if (CharacterType == CharacterType.Player)
        {
            Debug.Log("Setting immunity");
            RpcSetImmune();
        }
    }

    [ClientRpc]
    public void RpcSetImmune()
    {
        Debug.Log("Immunity");
        StartCoroutine(ImmunityTimer());
    }

    [ClientRpc]
    public void RpcTakeDamage()
    {
        Debug.Log("ouch");
        // i dunno, that comes later
    }

    private void SetSpriteFacing(Vector3 moveDirection) {
        formSpriteRenderer.flipX = moveDirection.x < 0;
    }

    private void ChangeForm()
    {
        // loop through all forms until you find an eligeable one
        for (int i = 0; i < forms.Count; i++)
        {
            CharacterForm form = forms[i];
            //bool meetsRequirements = true;

            if (ActiveUpgrade == form.UpgradeType)
            {
                formSpriteRenderer.sprite = form.Sprite;
            }
        }
    }

    private void StartWobble()
    {
        Transform spriteTransform = formSpriteRenderer.transform;

        // wobble height. requires both y and z because the sprite is slanted
        wobbleTweens.Add(spriteTransform.DOScaleY(spriteTransform.localScale.y * WobbleHeightStrength, WobbleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(WobbleEase));
        wobbleTweens.Add(spriteTransform.DOScaleZ(spriteTransform.localScale.z * WobbleHeightStrength, WobbleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(WobbleEase));

        // wobble width. delay start so width and height alternation their wobble
        wobbleTweens.Add(spriteTransform.DOScaleX(spriteTransform.localScale.x * WobbleWidthStrength, WobbleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(WobbleEase)
            .SetDelay(WobbleDuration));
    }

    private void StopWobble()
    {
        Transform spriteTransform = formSpriteRenderer.transform;

        foreach (Tween tween in wobbleTweens)
        {
            tween.Kill();
        }
        wobbleTweens.Clear();

        spriteTransform.localScale = Vector3.one;
    }

    private IEnumerator ImmunityTimer()
    {
        isImmune = true;
        Tween blinkTween = formSpriteRenderer.DOFade(.5f, BlinkSpeed).SetLoops(-1, LoopType.Yoyo);

        yield return new WaitForSeconds(ImmunityTime);

        justFought = false;
        isImmune = false;
        blinkTween.Kill();
        formSpriteRenderer.DOFade(1, 0);
    }

    // public methods to play audio clips

    // play a random 1 out of 3 different enemy defeat sounds
    public void PlayEnemyDefeatAudio()
    {
        Debug.Log("in audio");
        if (CharacterType != CharacterType.Player)
            return;


        if (isServer)
        {
            Debug.Log("server in audio method");
            return;
        }

        int clip = Random.Range(0, 2);
        switch (clip)
        {
            case 0:
                {
                    audioSrc.PlayOneShot(enemyDefeat1, 1);
                    break;
                }
            case 1:
                {
                    audioSrc.PlayOneShot(enemyDefeat2, 1);
                    break;
                }
            case 2:
                {
                    audioSrc.PlayOneShot(enemyDefeat3, 1);
                    break;
                }
            default: break;
        }
    }

    // play a random 1 out of 3 different item pickup sounds
    public void PlayItemPickupAudio()
    {
        if (CharacterType != CharacterType.Player)
            return;

        int clip = Random.Range(0, 2);
        switch (clip)
        {
            case 0:
                {
                    audioSrc.PlayOneShot(itemPickup1, 1);
                    break;
                }
            case 1:
                {
                    audioSrc.PlayOneShot(itemPickup2, 1);
                    break;
                }
            case 2:
                {
                    audioSrc.PlayOneShot(itemPickup3, 1);
                    break;
                }
            default: break;
        }
    }

    // plays the appropriate upgrade sound effect based on the type of the upgrade
    public void PlayUpgradeAudio(UpgradeType upgradeType)
    {
        if (CharacterType != CharacterType.Player)
            return;

        switch (upgradeType)
        {
            case UpgradeType.SPEED:
                {
                    audioSrc.PlayOneShot(speedUpgradePickup, 1);
                    break;
                }
            case UpgradeType.CARRY:
                {
                    audioSrc.PlayOneShot(carryUpgradePickup, 1);
                    break;
                }
            case UpgradeType.ATTACK:
                {
                    audioSrc.PlayOneShot(attackUpgradePickup, 1);
                    break;
                }
            default: break;
        }
    }
}
