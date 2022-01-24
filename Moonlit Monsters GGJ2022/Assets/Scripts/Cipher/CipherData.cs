using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Ciphers/Data")]
public class CipherData : ScriptableObject
{
	[SerializeField]
	[Tooltip("The pool of palintext passwords to pull from")]
	private List<string> _passwords;

	[SerializeField]
	[Tooltip("The pool of keys used to encrypt the passwords")]
	private List<string> _keys;

	/** Get a list of random words from the given pool
	\param words The pool of words to pull from
	\param num The maximum number of random words to get
	\param exclude The words to exclude from selection
	\return The randomly selected words from the given pool
	*/
	private List<string> GetRandomWords(List<string> words, int num, ICollection<string> exclude)
	{
		words.RemoveAll(delegate(string word){return exclude.Contains(word);});
		List<string> result = new List<string>();
		int index;
		while (num > result.Count && words.Count > 0)
		{
			index = Random.Range(0, words.Count);
			result.Add(words[index]);
			words.RemoveAt(index);
		}
		return result;
	}

	/** Get a random set of passwords
	\param num The number of passwords to get
	\param exclude The passwords to exclude from selection
	\return A random set of passwords
	*/
	public List<string> GetRandomPasswords(int num, ICollection<string> exclude)
	{
		return this.GetRandomWords(new List<string>(this._passwords), num, exclude);
	}

	/** Get a random set of keys
	\param num The number of keys to get
	\param exclude The keys to exclude from selection
	\return A random set of keys
	*/
	public List<string> GetRandomKeys(int num, ICollection<string> exclude)
	{
		return this.GetRandomWords(new List<string>(this._keys), num, exclude);
	}

	#if UNITY_EDITOR
		/** Log an error for duplicate elements in the given list
		\param list The list to check for duplicates
		\param label The name of the given list in the error message
		*/
		private void ValidateUnique(List<string> list, string label)
		{
			for (int i = 0; i + 1 < list.Count; ++i)
			{
				for (int j = i + 1; j < list.Count; ++j)
				{
					if (list[i] == list[j])
					{
						Debug.LogError(label + " cannot have duplicates: " + list[i]);
						return;
					}
				}
			}
		}

		/** Log an error for elements in the given list that contain characters not in the given list of accepted characters
		\param words The list of strings to check for illegal characters
		\param acceptedChars The list of accepted characters
		\param label The name of the given list used in error messages
		*/
		private void ValidateOnlyChars(ICollection<string> words, ICollection<char> acceptedChars, string label)
		{
			foreach (string word in words)
			{
				foreach (char c in word)
				{
					if (!acceptedChars.Contains(c))
					{
						Debug.LogError(label + " elements must not contain \'" + c + "\': " + word);
						break;
					}
				}
			}
		}

		private void OnValidate()
		{
			this.ValidateUnique(this._passwords, "passwords");
			this.ValidateUnique(this._keys, "keys");
			this.ValidateOnlyChars(this._passwords, CipherGroup.LETTERS, "passwords");
			this.ValidateOnlyChars(this._keys, CipherGroup.LETTERS, "keys");
		}
	#endif
}