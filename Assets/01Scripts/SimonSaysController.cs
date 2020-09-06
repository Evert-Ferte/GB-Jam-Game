using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class SimonSaysController : MonoBehaviour
{
	[SerializeField] private List<SimonSaysTile> tiles = new List<SimonSaysTile>();
	[SerializeField] private int maxSequence = 0;
	[SerializeField] AudioSource audioSource = null;
	private List<SimonSaysTile> gameSequence = new List<SimonSaysTile>();
	public bool SequencePlaying { get; private set; }
	private int sequenceID = 0;

	private void Start()
	{
		AddToSequence();
	}

	public void TileClicked(int id) 
	{
		SimonSaysTile tile = tiles[id];

		if (SequencePlaying) return;

		audioSource.clip = BeepBoop.GetTone(1000, 44000, Mathf.FloorToInt(Random.Range(440, 550)));
		audioSource.Play();

		if (tile != gameSequence[sequenceID]) // Wrong tile has been pressed.
		{
			StartCoroutine(ResetSequence());
		}
		else // Correct tile has been pressed.
		{
			if (sequenceID == gameSequence.Count -1)	// Player pressed the last tile in the sequence.
			{
				if (gameSequence.Count < maxSequence)   // The sequence has not reached its final size.
				{
					sequenceID = 0;
					AddToSequence();
				}

				else
				{
					// Game Finished!
					Debug.Log("Game Finished!");
					// Debug.Break();

					FindObjectOfType<GameManager>().PuzzleEnd(this.gameObject);
				}
			}
			else
			{
				sequenceID++;
			}
		}
	}

	private void AddToSequence()
	{
		SimonSaysTile newTile = tiles[Random.Range(0, 4)];
		gameSequence.Add(newTile);
		StartCoroutine(ShowSequence());
	}

	/// <summary>
	/// Showing the sequence.
	/// </summary>
	/// <returns></returns>
	private IEnumerator ShowSequence()
	{
		SequencePlaying = true;

		for (int i = 0; i < gameSequence.Count; i++)
		{
			yield return new WaitForSeconds(.5f);
			audioSource.clip = BeepBoop.GetTone(10000, 44000, 1000);
			audioSource.Play();
			gameSequence[i].UpdateColor(true);
			yield return new WaitForSeconds(.5f);
			gameSequence[i].UpdateColor(false);
		}

		SequencePlaying = false;
	}

	private IEnumerator ResetSequence()
	{
		SequencePlaying = true;
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < tiles.Count; y++)
			{
				tiles[y].SwitchTile(false);
			}
			yield return new WaitForSeconds(.15f);
			for (int z = 0; z < tiles.Count; z++)
			{
				tiles[z].SwitchTile(true);
			}
			yield return new WaitForSeconds(.15f);
		}
		sequenceID = 0;
		gameSequence = new List<SimonSaysTile>();   // Empty the sequence.
		AddToSequence();                            // Start a new sequence.
		SequencePlaying = false;
	}
}
