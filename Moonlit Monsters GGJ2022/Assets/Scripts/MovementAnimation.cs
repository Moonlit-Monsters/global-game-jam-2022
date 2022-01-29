using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementAnimation : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The rigidbody to get velocity from")]
    private Rigidbody2D _rb;

	public Rigidbody2D Rb
	{
		get
		{
			return this._rb;
		}
	}

	[SerializeField]
	[Tooltip("The nav mesh agent used to path find")]
	private NavMeshAgent _nav;

	public NavMeshAgent Nav
	{
		get
		{
			return this._nav;
		}
	}

    [SerializeField]
	[Tooltip("The animator for the character's sprites")]
	private Animator _entityAnimator;

	public Animator EntityAnimator
	{
		get
		{
			return _entityAnimator;
		}
	}

    [SerializeField]
	[Tooltip("The animator's x float parameter")]
	private string _x;

	public string X
	{
		get
		{
			return _x;
		}
	}

    [SerializeField]
	[Tooltip("The animator's y float parameter")]
	private string _y;

	public string Y
	{
		get
		{
			return _y;
		}
	}

    private void Awake()
    {
        this._rb ??= this.GetComponent<Rigidbody2D>();
        this._entityAnimator ??= this.gameObject.GetComponentInChildren<Animator>();
    }

	private Vector2 RigidbodyCheck()
	{
		if (this.Nav == null) { return this.Rb.velocity; }
		return this.Rb.velocity.sqrMagnitude < this.Nav?.velocity.sqrMagnitude ? new Vector2(this.Nav.velocity.x, this.Nav.velocity.y) : this.Rb.velocity;
	}

    private void FixedUpdate()
    {
        this.EntityAnimator.SetFloat(this.X, RigidbodyCheck().x);
        this.EntityAnimator.SetFloat(this.Y, RigidbodyCheck().y);
    }
}


