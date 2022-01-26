using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("Where to get player input from")]
	private PlayerInput _input;

	public PlayerInput Input
	{
		get
		{
			return this._input;
		}
	}

	[SerializeField]
	[Tooltip("The attack the player uses")]
	private Attack _attack;

	public Attack Attack
	{
		get
		{
			return this._attack;
		}
	}

	[Header("Settings")]

	[SerializeField]
	[Tooltip("The name of the button action used to trigger the attack")]
	private string _actionName;

	public string ActionName
	{
		get
		{
			return this._actionName;
		}
	}

	/** Trigger the player's attack */
	private void TriggerAttack(InputAction.CallbackContext context)
	{
		this.Attack.enabled = true;
	}

	private void Awake()
	{
		this.Input.actions[this.ActionName].performed += this.TriggerAttack;
	}
}