using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Settings")]
	
	[SerializeField]
	[Tooltip("The speed at which the player moves")]
	private float _speed;

	public float Speed
	{
		get
		{
			return this._speed;
		}
	}

	[Header("Controls")]

	[SerializeField]
	[Tooltip("The name of the control used to move")]
	private string _moveName;

	public string MoveName
	{
		get
		{
			return this._moveName;
		}
	}

	/** The action used to move */
	private InputAction _moveAction;

	private Rigidbody2D _rb;

	private void Awake()
	{
		this._moveAction = this.GetComponent<PlayerInput>().actions[this.MoveName];
		this._rb = this.GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		Vector2 movement = this._moveAction.ReadValue<Vector2>().normalized * this.Speed * Time.deltaTime;
		this._rb.AddForce(movement - this._rb.velocity, ForceMode2D.Force);
	}
}