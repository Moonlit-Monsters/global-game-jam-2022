using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class WordBuilder : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The set of prefabs to use for each letter")]
	private List<CharGameObject> _letterPrefabs;

	[SerializeField]
	[Tooltip("The transform of the game object to build words under")]
	private Transform _parent;

	private List<GameObject> _letters;
	/** The currently instantiated letters */
	public ReadOnlyCollection<GameObject> Letters
	{
		get
		{
			return this._letters.AsReadOnly();
		}
	}

	/** The current word */
	public string Word {get; private set;}

	/** Clear the current word */
	public void Clear()
	{
		this.Word = "";
		foreach (GameObject letter in this.Letters)
		{
			Object.Destroy(letter);
		}
	}

	/** Set the word for this and build it
	\param word The word to build
	*/
	public void SetWord(string word)
	{
		this.Clear();
		List<char> characters = new List<char>(word.Length);
		CharGameObject mapping;
		foreach (char letter in word)
		{
			mapping = this._letterPrefabs.Find(delegate(CharGameObject value){
				return value.Character == letter && value.GameObject != null;});
			if (mapping == null)
			{
				Debug.LogWarning("Unrecgnised character: " + letter);
				continue;
			}
			this._letters.Add(GameObject.Instantiate(mapping.GameObject, this._parent));
			characters.Add(letter);
		}
		this.Word = new string(characters.ToArray());
	}

	private void Awake()
	{
		this._parent ??= this.transform;
	}

	private void OnDestroy()
	{
		this.Clear();
	}
}