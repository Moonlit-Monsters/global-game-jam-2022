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

	[SerializeField]
	[Tooltip("The animator for the player sprites")]
	private Animator _playerAnimator;

	public Animator PlayerAnimator
	{
		get
		{
			return _playerAnimator;
		}
	}

	[SerializeField]
	[Tooltip("The Player's camera follow transform")]
	private Transform _camFollowPoint;

	public Transform CamFollowPoint
	{
		get
		{
			return this._camFollowPoint;
		}
	}

	[SerializeField]
	[Tooltip("The speed at which the Camera moves to offset")]
	private float _camMoveSharpness;

	public float CamMoveSharpness
	{
		get
		{
			return _camMoveSharpness;
		}
	}

	[SerializeField]
	[Tooltip("The offset amount for camera")]
	private float _camOffset;

	public float CamOffset
	{
		get
		{
			return _camOffset;
		}
	}

	private Vector2 moveAxis;

	private Rigidbody2D _rb;

	private void Awake()
	{
		this._rb = this.GetComponent<Rigidbody2D>();
	}

	public void MyInput(InputAction.CallbackContext context) //Control scheme input values (is changed when the state of the input is change) (e.g. when w is pressed and when it is lifted)
    {
        float x = context.ReadValue<Vector2>().x;
		float y = context.ReadValue<Vector2>().y;

		moveAxis = new Vector2(AxisClamp(x), AxisClamp(y));
    }

	private float AxisClamp(float axis)
	{
		return axis > 0.333 ? 1f : (axis < -0.333 ? -1f : 0);
	}

	private void FixedUpdate()
	{
		Vector2 movement = this.moveAxis.normalized * this.Speed * Time.deltaTime;
		this._rb.AddForce(movement - this._rb.velocity, ForceMode2D.Impulse);

		CameraFollowUpdate();
	}

	private void CameraFollowUpdate()
	{
		Vector3 posOffset = new Vector3(moveAxis.x, moveAxis.y, 0f).normalized * CamOffset;
		Vector3 newPos = this.gameObject.transform.position + posOffset;

		this._camFollowPoint.position = Vector3.Lerp(this.CamFollowPoint.position, newPos, this.CamMoveSharpness * Time.deltaTime);
		this._camFollowPoint.position = (this._camFollowPoint.position - newPos).sqrMagnitude < 0.2 ? newPos : this._camFollowPoint.position;
	}
}