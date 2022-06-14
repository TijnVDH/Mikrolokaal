using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Character))]
public class TouchMovement : NetworkBehaviour
{
    [SerializeField] private Camera playerCam;

    private new Rigidbody rigidbody;
    private Character character;

    public event MovementAction OnStartMoving;
    public event MovementAction OnStopMoving;
    public delegate void MovementAction();

    public event DirectionalMovementAction OnMove;
    public delegate void DirectionalMovementAction(Vector3 direction);

    private Joystick joystick;
    Vector3 dir = Vector3.zero;

	private void Start()
	{
		if (!isLocalPlayer)
		{
            Destroy(playerCam);
            enabled = false;
            return;
        }

        character = GetComponent<Character>();
        rigidbody = GetComponent<Rigidbody>();
        InventoryUI inventoryUI = GameObject.Find("Inventory").transform.GetComponent<InventoryUI>();
        inventoryUI.Joystick.SetActive(true);
        joystick = inventoryUI.Joystick.transform.GetComponent<Joystick>();
    }

	private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        MoveCharacter();
    }

    private void MoveCharacter()
	{
        dir.x = joystick.Horizontal != 0 ? joystick.Horizontal : 0;
        dir.z = joystick.Vertical != 0 ? joystick.Vertical : 0;
        rigidbody.MovePosition(transform.position + (dir * character.MovementSpeed * Time.deltaTime));
	}
    
}
