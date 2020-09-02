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

	private Image image;
	private Dictionary<char, string> alphabet = new Dictionary<char, string>();

	private void Start()
	{
		image = GetComponent<Image>();
		EnterAlphabet();
		message = message.ToLower();
		StartCoroutine(ShowMorse());
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
					image.sprite = lightBulbOn;
					yield return new WaitForSeconds(0.25f * BitDuration(c2));
					image.sprite = lightBulbOff;
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
