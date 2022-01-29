using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawner : MonoBehaviour
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("The player's health which triggers the respawn")]
	private Health _playerHealth;

	[SerializeField]
	[Tooltip("The transition to run before reloading the current scene")]
	private BlackoutTransition _transition;

	[SerializeField]
	[Tooltip("The scene loader used to reload the scene")]
	private SceneLoader _sceneLoader;

	/** Respawn if the given health value is 0 */
	private void OnHealthChange(float health)
	{
		if (health <= 0f)
		{
			this.Respawn();
		}
	}

	/** Respawn the player by reloading the scene */
	public void Respawn()
	{
		this._transition.OnFadeOutDone.AddListener(this.FadeOutDone);
		this._transition.FadeOut();
	}

	/** Reload the scene once the transition is done */
	private void FadeOutDone()
	{
		this._transition.OnFadeOutDone.RemoveListener(this.FadeOutDone);
		this._sceneLoader.LoadScene(SceneManager.GetActiveScene().path);
	}

	private void Awake()
	{
		this._playerHealth.OnChange.AddListener(this.OnHealthChange);
	}
}