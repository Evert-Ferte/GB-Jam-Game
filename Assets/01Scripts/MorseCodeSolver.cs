using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class MorseCodeSolver : MonoBehaviour
{
    [SerializeField] private GameObject solutionPath = null;
    [SerializeField] private string solution = null;
    [SerializeField] private string guess = "";

    public void AddLetter(string letter)
    {
        if (guess.Length < solution.Length) 
        {
            guess += letter;
            if (guess.Length == solution.Length) CheckGuess();
        }
    }

    private void CheckGuess()
    {
        if (guess == solution) {
            StartCoroutine(Scroll());
        }

        else {  // Reset the guess.
            guess = "";
            StartCoroutine(Flash());
        }
    }

    public void Switch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private IEnumerator Flash(){
        solutionPath.SetActive(false);
        yield return new WaitForSeconds(.25f);
        solutionPath.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        solutionPath.SetActive(false);
        yield return new WaitForSeconds(.25f);
        solutionPath.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        solutionPath.SetActive(false);
        yield return new WaitForSeconds(.25f);
        solutionPath.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        solutionPath.SetActive(false);
        yield return new WaitForSeconds(.25f);
        solutionPath.SetActive(true);
    }

    private IEnumerator Scroll()
    {
        foreach (Transform child in solutionPath.transform)
        {
            child.GetComponent<UnityEngine.UI.Image>().enabled = false;
            yield return new WaitForSeconds(.1f);
        }

        foreach (Transform child in solutionPath.transform)
        {
            child.GetComponent<UnityEngine.UI.Image>().enabled = true;
            yield return new WaitForSeconds(.1f);
        }

        FindObjectOfType<GameManager>().PuzzleEnd(this.transform.parent.gameObject);
    }
}
