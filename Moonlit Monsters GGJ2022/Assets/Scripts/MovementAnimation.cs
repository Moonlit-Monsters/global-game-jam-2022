using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
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

    private Rigidbody2D _rb;

    private void Awake()
    {
        this._rb = this.GetComponent<Rigidbody2D>();
        this._entityAnimator ??= this.gameObject.GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        this.EntityAnimator.SetFloat(this.X, this._rb.velocity.x);
        this.EntityAnimator.SetFloat(this.Y, this._rb.velocity.y);
    }
}


