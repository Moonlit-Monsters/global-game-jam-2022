using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerQuit : MonoBehaviour
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("The player input to take input from")]
	private PlayerInput _input;

	[SerializeField]
	[Tooltip("The quitter to invoke")]
	private Quitter _quit;

	[Header("Settings")]

	[SerializeField]
	[Tooltip("The name of the triggering action")]
	private string _actionName;

	public void Quit(InputAction.CallbackContext context)
	{
		Debug.Log("player quit");
		this._quit.Quit();
	}

	private void Awake()
	{
		this._input.actions[this._actionName].performed += this.Quit;
	}
}
