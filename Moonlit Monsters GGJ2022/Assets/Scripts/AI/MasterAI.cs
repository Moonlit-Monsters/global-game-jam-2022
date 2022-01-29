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

	[Header("Settings")]

	[SerializeField]
	[Tooltip("The name of the boolean player setting to disable this attacking")]
	private string _peacefulSettingName;

	public string PeacefulSettingName
	{
		get
		{
			return this._peacefulSettingName;
		}
	}

	[SerializeField]
	[Tooltip("Whether this should not attack")]
	private bool _peaceful = false;

	public bool Peaceful
	{
		get
		{
			return this._peaceful;
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

	/** The point at which this started */
	public Vector2 StartPoint {get; private set;}

	/** Toggle the specified behaviours according to the current state */
	private void ToggleBehaviours()
	{
		this.Wander.gameObject.SetActive(this.CurrentState == MasterState.Wander);
		this.Pursue.gameObject.SetActive(this.CurrentState == MasterState.Pursue);
		this.Attack.gameObject.SetActive(this.CurrentState == MasterState.Attack);
		this.Attack.enabled = this.CurrentState == MasterState.Attack;
	}

	/** Switch to the wander state */
	public void SwitchToWander()
	{
		if (this.CurrentState != MasterState.Wander)
		{
			this.CurrentState = MasterState.Wander;
			this.ToggleBehaviours();
		}
	}

	/** Switch to the persue state */
	public void SwitchToPursue()
	{
		if (this.CurrentState != MasterState.Pursue)
		{
			this.CurrentState = MasterState.Pursue;
			this.ToggleBehaviours();
		}
	}

	/** Switch to the attack state */
	public void SwitchToAttack()
	{
		if (!this.Peaceful && this.CurrentState != MasterState.Attack)
		{
			Debug.Log("attack");
			this.CurrentState = MasterState.Attack;
			this.ToggleBehaviours();
		}
	}

	/** Switch to the stunned state */
	public void SwitchToStunned()
	{
		if (this.CurrentState != MasterState.Stunned)
		{
			this.CurrentState = MasterState.Stunned;
			this.ToggleBehaviours();
		}
	}

	private void Awake()
	{
		this.StartPoint = this.transform.position;
		this.Pursue.Master = this;
		this.Wander.Master = this;
		this.Pursue.Initialise();
		this.Wander.Initialise();
		RealitySwitcher.Instance.OnEnterReality.AddListener(this.SwitchToPursue);
		RealitySwitcher.Instance.OnEnterDelusion.AddListener(this.SwitchToWander);
		this.Stun.OnStun.AddListener(this.SwitchToStunned);
		this.Stun.OnStunOver.AddListener(this.SwitchToPursue);
		this.Attack.OnAttackDone.AddListener(this.SwitchToPursue);
	}

	private void OnEnable()
	{
		if (this.PeacefulSettingName.Length > 0)
		{
			this._peaceful = PlayerPrefs.GetInt(this.PeacefulSettingName, this.Peaceful ? 1 : 0) != 0;
		}
		this.StartPoint = this.transform.position;
		this.CurrentState = RealitySwitcher.Instance.IsReality ? MasterState.Pursue : MasterState.Wander;
		this.ToggleBehaviours();
	}

	private void OnDisable()
	{
		this.CurrentState = MasterState.Stunned;
	}

	private void Update()
	{
		if (this.CurrentState == MasterState.Pursue 
			&& this.Pursue.CurrentState == PursueAI.PersueState.Persuing
			&& Vector2.Distance(this.Pursue.Rb.position, this.Pursue.CurrentTarget.position) <= this.Pursue.Nav.stoppingDistance)
		{
			this.SwitchToAttack();
		}
	}
}