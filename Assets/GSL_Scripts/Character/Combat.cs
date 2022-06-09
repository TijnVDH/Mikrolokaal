using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DEPRECATED
/// </summary>
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Combat : NetworkBehaviour
{
    [Header("Times")]
    public float ImmunityTime;
    public float LowerSpeedTime;

    public event ImmunityAction OnImmunity;
    public delegate void ImmunityAction(float immunityDuration);

    private Character character;
    private bool isImmune = false;

    ScoreCounter scoreScript;

    private void Start()
    {
        character = GetComponent<Character>();

        scoreScript = GameObject.Find("Text (TMP)").GetComponent<ScoreCounter>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        Character colliderCharacter = collision.gameObject.GetComponent<Character>();
        Combat colliderCombat = collision.gameObject.GetComponent<Combat>();

        if (colliderCharacter != null && colliderCombat != null)
        {
            Fight(colliderCombat, colliderCharacter);
        }
    }

    public void Fight(Combat otherCombat, Character otherCharacter)
    {
        if (character == null)
            return;

        if (isImmune || otherCombat.isImmune)
            return;

        // No infighting
        if (character.CharacterType == otherCharacter.CharacterType)
            return;

        // Compare the AP values
        if (otherCombat.character.AttackStrength > character.AttackStrength)
        {
            // Other is stronger, this one dies
            Die();
        }
        else if (otherCombat.character.AttackStrength == character.AttackStrength)
        {
            // Other is the same as this one
            // Check if the other is a player with advantage
            if (otherCharacter.CharacterType == CharacterType.Player)
            {
                // Other is a player, player wins

                // award points
                scoreScript.EnemyDefeatPoints();

                // This one dies
                Die();
            }
        }

        // Set immune for x seconds
        if (character.CharacterType == CharacterType.Player)
        {
            isImmune = true;
            StartCoroutine(ImmunityTimer());
        }
    }

    private void Die()
    {
        //if (character.CharacterType == CharacterType.Player)
        //{
        //    // Check if there are any upgrades to drop
        //    bool hasDroppedUpgrades = false;
        //    if (character.UpgradeInventory != null)
        //    {
        //        if (character.UpgradeInventory.Count > 0)
        //        {
        //            // Drop all upgrades
        //            character.UpgradeInventory.DropAll();
        //            hasDroppedUpgrades = true;
        //        }
        //    }

        //    // if not, lower player speed
        //    if (!hasDroppedUpgrades)
        //    {
        //        StartCoroutine(LowerPlayerSpeedTimer());
        //    }
        //}
        //else if (character.CharacterType == CharacterType.Virus)
        //{
        //    // Destroy
        //    NPC npc = GetComponent<NPC>();
        //    if (npc != null)
        //    {
        //        npc.RemoveFromSpawner();
        //    }
        //    NetworkServer.Destroy(gameObject);
        //}
    }

    private IEnumerator ImmunityTimer()
    {
        OnImmunity?.Invoke(ImmunityTime);
        yield return new WaitForSeconds(ImmunityTime);
        isImmune = false;
    }

    private IEnumerator LowerPlayerSpeedTimer()
    {
        // Get half of the current player speed
        float playerSpeed = character.MovementSpeed;
        float halfSpeed = playerSpeed / 2;

        // Subtract half of the speed
        character.MovementSpeed -= halfSpeed;

        // Wait for the timer to run out
        yield return new WaitForSeconds(LowerSpeedTime);

        // Add the subtracted speed back again
        character.MovementSpeed += halfSpeed;
    }
}
