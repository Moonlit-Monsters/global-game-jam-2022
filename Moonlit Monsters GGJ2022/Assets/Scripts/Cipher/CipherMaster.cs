using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[ExecuteAlways]
[DisallowMultipleComponent]
public class CipherMaster : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The data pool used to generate keys and passwords")]
	private CipherData _dataPool;

	[SerializeField]
	[Tooltip("The grouped cipher pieces and gates")]
	private List<CipherGroup> _groups = new List<CipherGroup>();

	public ReadOnlyCollection<CipherGroup> Groups
	{
		get
		{
			return this._groups.AsReadOnly();
		}
	}

	/** The singleton instance of this */
	public static CipherMaster Instance {get; private set;} = null;

	/** Set the singleton instance to this or destroy this if it is already set */
	private void SingletonCheck()
	{
		CipherMaster.Instance ??= this;
		if (CipherMaster.Instance != this)
		{
			Debug.LogWarning("Destroyed extra cipher master: " + this.name);
			Object.DestroyImmediate(this);
		}
	}

	/** Generate a new set of passwords and keys for each cipher group from the cipher data */
	private void GeneratePasswords()
	{
		List<string> passwords = this._dataPool.GetRandomPasswords(this.Groups.Count, new List<string>());
		List<string> keys = this._dataPool.GetRandomKeys(this.Groups.Count, new List<string>());
		for (int i = 0; i < this.Groups.Count; ++i)
		{
			this.Groups[i].AssignPassword(passwords[i], keys[i]);
		}
	}

	/** Set the group property of the cipher pieces in each group */
	private void LinkGroups()
	{
		foreach (CipherGroup group in this.Groups)
		{
			if (group.PasswordItem != null)
			{
				group.PasswordItem.Group = group;
			}
			if (group.KeyItem != null)
			{
				group.KeyItem.Group = group;
			}
			foreach (CipherGate gate in group.Gates)
			{
				gate.Group = group;
			}
		}
	}

	private void Awake()
	{
		this.SingletonCheck();
		if (Application.IsPlaying(this))
		{
			this.GeneratePasswords();
		}
	}

	private void OnDestroy()
	{
		if (CipherMaster.Instance == this)
		{
			CipherMaster.Instance = null;
		}
	}

	#if UNITY_EDITOR
		private void OnValidate()
		{
			this.LinkGroups();
		}
	#endif
}