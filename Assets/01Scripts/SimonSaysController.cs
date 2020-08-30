using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysController : MonoBehaviour
{
	public System.Action<SimonSaysTile> OnTileClicked;

	[SerializeField] private List<SimonSaysTile> tiles = new List<SimonSaysTile>();
	[SerializeField] private int maxSequence;
	private List<int> gameSequence = new List<int>();
	public bool SequencePlaying { get; private set; }
	private int sequenceID;

	/// <summary>
	/// Executes at the Start
	/// </summary>
	private void Start()
	{
		OnTileClicked += TileClicked;
	}

	private void TileClicked(SimonSaysTile tile) 
	{
		if (SequencePlaying) return;
	}

	private void AddToSequence()
	{
		gameSequence.Add(Random.Range(0, 4));
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
			tiles[gameSequence[i]].UpdateColor(true);
			yield return new WaitForSeconds(.5f);
			tiles[gameSequence[i]].UpdateColor(false);
		}

		SequencePlaying = false;
	}
}
