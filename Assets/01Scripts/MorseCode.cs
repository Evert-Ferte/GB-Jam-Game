using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MorseCode : MonoBehaviour
{
	[Header("No spaces here please...")]
	[SerializeField] private string message = null;
	[SerializeField] private Sprite lightBulbOn = null;
	[SerializeField] private Sprite lightBulbOff = null;
	[SerializeField] private Image lightBulbImage = null;
	[SerializeField] private Sprite translatorOn = null;
	[SerializeField] private Sprite translatorOff = null;
	[SerializeField] private Image translatorImage = null;

	private Dictionary<char, string> alphabet = new Dictionary<char, string>();

	IEnumerator DoMorse;

	private void OnEnable() 
	{
		if (alphabet.Count == 0) EnterAlphabet();
		message = message.ToLower();
		DoMorse = ShowMorse();
		StartCoroutine(DoMorse);
	}

	private void OnDisable() 
	{
		StopCoroutine(DoMorse);
		lightBulbImage.sprite = lightBulbOff;
		translatorImage.sprite = translatorOff;
	}

	public void SwitchGameObject()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}

	private void EnterAlphabet()
	{
		alphabet.Add('a', ".-");
		alphabet.Add('b', "-...");
		alphabet.Add('c', "-.-.");
		alphabet.Add('d', "-..");
		alphabet.Add('e', ".");
		alphabet.Add('f', "..-.");
		alphabet.Add('g', "--.");
		alphabet.Add('h', "....");
		alphabet.Add('i', "..");
		alphabet.Add('j', ".---");
		alphabet.Add('k', "-.-");
		alphabet.Add('l', ".-..");
		alphabet.Add('m', "--");
		alphabet.Add('n', "-.");
		alphabet.Add('o', "---");
		alphabet.Add('p', ".--.");
		alphabet.Add('q', "--.-");
		alphabet.Add('r', ".-.");
		alphabet.Add('s', "...");
		alphabet.Add('t', "-");
		alphabet.Add('u', "..-");
		alphabet.Add('v', "...-");
		alphabet.Add('w', ".--");
		alphabet.Add('x', "-..-");
		alphabet.Add('y', "-.--");
		alphabet.Add('z', "--..");
	}

	private IEnumerator ShowMorse() 
	{
		yield return new WaitForSeconds(1f);

		char[] characters = message.ToCharArray();

		while (true)
		{
			foreach (char c1 in characters)
			{
				char[] code = alphabet[c1].ToCharArray();

				foreach (char c2 in code)
				{
					lightBulbImage.sprite = lightBulbOn;
					translatorImage.sprite = translatorOn;
					yield return new WaitForSeconds(0.25f * BitDuration(c2));
					lightBulbImage.sprite = lightBulbOff;
					translatorImage.sprite = translatorOff;
					yield return new WaitForSeconds(0.25f);
				}

				yield return new WaitForSeconds(0.75f);
			}

			yield return new WaitForSeconds(1.75f);
		}
	}

	private int BitDuration(char letter) 
	{
		switch (letter)
		{
			case '.':
				return 1;
			case '-':
				return 3;
			default:
				return 0;
		}
	}
}
