using UnityEngine;

public abstract class SlaveAIBase : MonoBehaviour
{
	/** The master AI controlling this */
	public MasterAI Master {get; set;}

	/** Initialise this AI once the master is set*/
	public abstract void Initialise();
}