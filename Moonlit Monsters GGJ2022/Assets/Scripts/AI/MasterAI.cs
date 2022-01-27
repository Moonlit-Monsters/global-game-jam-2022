using UnityEngine;

public class MasterAI : MonoBehaviour
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("The wandering AI to use")]
	private WanderAI _wander;

	public WanderAI Wander
	{
		get
		{
			return this._wander;
		}
	}

	[SerializeField]
	[Tooltip("The pursue AI to use")]
	private PursueAI _pursue;

	public PursueAI Pursue
	{
		get
		{
			return this._pursue;
		}
	}

	[SerializeField]
	[Tooltip("The attack to use")]
	private Attack _attack;

	public Attack Attack
	{
		get
		{
			return this._attack;
		}
	}

	[SerializeField]
	[Tooltip("The stun tracker to use")]
	private Stunnable _stun;

	public Stunnable Stun
	{
		get
		{
			return this._stun;
		}
	}

	public enum MasterState
	{
		Stunned,
		Wander,
		Pursue,
		Attack
	}

	/** The current state this is in */
	public MasterState CurrentState {get; private set;}

	/** Toggle the specified behaviours according to the current state */
	private void ToggleBehaviours()
	{
		this.Wander.enabled = this.CurrentState == MasterState.Wander;
		this.Pursue.enabled = this.CurrentState == MasterState.Pursue;
		this.Attack.enabled = this.CurrentState == MasterState.Attack;
	}

	/** Switch to the wander state */
	public void SwitchToWander()
	{
		this.CurrentState = MasterState.Wander;
		this.ToggleBehaviours();
	}

	/** Switch to the persue state */
	public void SwitchToPursue()
	{
		this.CurrentState = MasterState.Pursue;
		this.ToggleBehaviours();
	}

	/** Switch to the attack state */
	public void SwitchToAttack()
	{
		this.CurrentState = MasterState.Attack;
		this.ToggleBehaviours();
	}

	/** Switch to the stunned state */
	public void SwitchToStunned()
	{
		this.CurrentState = MasterState.Stunned;
		this.ToggleBehaviours();
	}

	private void Awake()
	{
		RealitySwitcher.Instance.OnEnterReality.AddListener(this.SwitchToPursue);
		RealitySwitcher.Instance.OnEnterDelusion.AddListener(this.SwitchToWander);
		this.Stun.OnStun.AddListener(this.SwitchToStunned);
		this.Stun.OnStunOver.AddListener(this.SwitchToPursue);
		this.CurrentState = RealitySwitcher.Instance.IsReality ? MasterState.Pursue : MasterState.Wander;
	}

	private void Update()
	{
		if (this.CurrentState == MasterState.Attack 
			&& !this.Attack.enabled)
		{
			this.SwitchToPursue();
		}
		else if (this.CurrentState == MasterState.Pursue 
			&& this.Pursue.CurrentState == PursueAI.PersueState.Persuing
			&& Vector2.Distance(this.Pursue.Rb.position, this.Pursue.CurrentTarget.position) <= this.Pursue.Nav.stoppingDistance)
		{
			this.SwitchToAttack();
		}
	}
}