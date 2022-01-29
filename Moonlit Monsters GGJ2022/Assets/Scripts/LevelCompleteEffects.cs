using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelCompleteEffects : MonoBehaviour
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("The trigger for the level completion")]
	private Collider2D _trigger;

	[SerializeField]
	[Tooltip("The scene loader to trigger")]
	private SceneLoader _sceneLoader;

	[SerializeField]
	[Tooltip("The transition to invoke before loading scenes")]
	private BlackoutTransition _transition;

	/** Complete the current level */
	public void CompleteLevel()
	{
		this._transition.OnFadeOutDone.AddListener(this.LoadScene);
		this._transition.FadeOut();
	}

	/** Load the scene using the scene loader */
	private void LoadScene()
	{
		this._transition.OnFadeOutDone.RemoveListener(this.LoadScene);
		this._sceneLoader.LoadScene();
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		this.CompleteLevel();
	}

	#if UNITY_EDITOR
		private void OnValidate()
		{
			if (this._trigger != null)
			{
				if (!this._trigger.isTrigger)
				{
					Debug.LogError("Trigger must be a trigger");
				}
				if (this._trigger.gameObject != this.gameObject)
				{
					Debug.LogError("Trigger must be on smae game object as this");
				}
			}
		}
	#endif
}
