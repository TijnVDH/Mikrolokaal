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

    private Vector3 screenDirection;
    private new Rigidbody rigidbody;
    private Character character;

    public event MovementAction OnStartMoving;
    public event MovementAction OnStopMoving;
    public delegate void MovementAction();

    public event DirectionalMovementAction OnMove;
    public delegate void DirectionalMovementAction(Vector3 direction);

    private bool isMoving = false;

	private void Start()
	{
		if (!isLocalPlayer)
		{
            Destroy(playerCam);
            enabled = false;
		}

        character = GetComponent<Character>();
        rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
        if (!isLocalPlayer) return;

		if (Input.GetMouseButton(0))
		{
            Vector3 transformScreenPos = playerCam.WorldToScreenPoint(transform.position);

            screenDirection = (Input.mousePosition - transformScreenPos).normalized;
            screenDirection.z = 0;
        }
        else
		{
            screenDirection = Vector3.zero;
		}
    }

	private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        // any directional input detected?
		if (screenDirection != Vector3.zero)
		{
            if (!isMoving)
            {
                isMoving = true;
                OnStartMoving?.Invoke();
            }

            MoveCharacter();
		}
        else
		{
            if (isMoving)
            {
                isMoving = false;
                OnStopMoving?.Invoke();
            }
        }

        OnMove?.Invoke(screenDirection);
    }

    private void MoveCharacter()
	{
        rigidbody.MovePosition(transform.position + (ScreenToWorldDirection(screenDirection) * character.MovementSpeed * Time.deltaTime));
	}

    /// <summary>
    /// Translates Screen vector to world vector. Screen works on xy axis, but our world movement works with xz axis
    /// </summary>
    private Vector3 ScreenToWorldDirection(Vector3 screenDirection)
	{
        return new Vector3
        {
            x = screenDirection.x,
            y = screenDirection.z,
            z = screenDirection.y,
        };
    }
}
