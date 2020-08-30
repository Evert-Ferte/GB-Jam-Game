using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysController : MonoBehaviour
{
	public System.Action<SimonSaysTile> OnTileClicked;

	[SerializeField] private List<SimonSaysTile> tiles = new List<SimonSaysTile>();
	[SerializeField] private int maxSequence;
	private List<SimonSaysTile> gameSequence = new List<SimonSaysTile>();
	public bool SequencePlaying { get; private set; }
	private int sequenceID;

	private void OnEnable() => OnTileClicked += TileClicked;
	private void OnDisable() => OnTileClicked -= TileClicked;

	private void Start()
	{
		AddToSequence();
	}

	private void TileClicked(SimonSaysTile tile) 
	{
		if (SequencePlaying) return;
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
					Debug.Break();
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
