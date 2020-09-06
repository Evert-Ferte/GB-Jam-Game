using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenMaze : MonoBehaviour
{
    [SerializeField] private Vector2Int size = Vector2Int.zero;
    [SerializeField] private bool[,] maze = null;
    [SerializeField] private SpriteRenderer[,] tiles = null;
    [SerializeField] private Sprite defaultSprite = null;
    [SerializeField] private Sprite wrongSprite = null;
    [SerializeField] private Sprite finnishSprite = null;
    [SerializeField] private Sprite playerSprite = null;
    [SerializeField] AudioSource audioSource = null;

    private Vector2 playerPosition = Vector2.zero;
    private Vector2 finnish = Vector2.zero;

    private void OnEnable() 
    {
        maze = new bool[size.x, size.y];

        int row1 = Random.Range(2,3);
        int row2 = Random.Range(4, 5);

        for (int x = 0; x < size.x; x++)
        {
            maze[x, 0] = true;
            maze[x, row1] = true;
            maze[x, row2] = true;
            maze[x, size.y -1] = true;
        }

        int colomn = Random.Range(0, size.x);
        maze[colomn, 1] = true;
        maze[colomn, 2] = true;

        int colomn2 = Random.Range(0, size.x);
        for (int x = row1; x < row2; x++) maze[colomn2, x] = true;

        int colomn3 = Random.Range(0, size.x);
        for (int x = row2; x < size.y; x++) maze[colomn3, x] = true;

        tiles = new SpriteRenderer[size.x, size.y];

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                GameObject g = new GameObject();
                g.transform.name = string.Format("Tile [{0},{1}]", x, y);
                g.transform.parent = transform;
                tiles[x,y] = g.AddComponent<SpriteRenderer>();
                tiles[x,y].sprite = defaultSprite;

                Vector2 position = Vector2.zero;
                position.x = size.x % 2 == 0 ? x - size.x / 2 + .5f : x - size.x / 2;
                position.y = size.y % 2 == 0 ? y - size.y / 2 + .5f : y - size.y / 2; 
                g.transform.position = position;
            }
        }

        tiles[size.x -1, size.y-1].sprite = finnishSprite;
        finnish = new Vector2(size.x-1, size.y-1);

        tiles[(int)playerPosition.x, (int)playerPosition.y].sprite = playerSprite;
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.A)) UpdatePosition(-Vector2.right);
        if (Input.GetKeyDown(KeyCode.D)) UpdatePosition(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W)) UpdatePosition(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S)) UpdatePosition(-Vector2.up);
    }

    private void UpdatePosition(Vector2 direction)
    {
        audioSource.clip = BeepBoop.GetTone(5000, 44000, Mathf.FloorToInt(Random.Range(440, 550)));
        audioSource.Play();

        tiles[(int)playerPosition.x, (int)playerPosition.y].sprite = defaultSprite;
        playerPosition += direction;
        playerPosition.x = Mathf.Clamp(playerPosition.x, 0, size.x - 1);
        playerPosition.y = Mathf.Clamp(playerPosition.y, 0, size.y - 1);
        tiles[(int)playerPosition.x, (int)playerPosition.y].sprite = playerSprite;

        if (playerPosition == finnish)
        {
            // Finnish found...
            FindObjectOfType<Game.GameManager>().PuzzleEnd(this.transform.parent.gameObject);
        }
        else if (!FalidTile())
        {
            StartCoroutine(FakeTileAnimation(playerPosition));
            playerPosition = Vector2.zero;
            UpdatePosition(Vector2.zero);
        }
    }

    private bool FalidTile()
    {
        return maze[(int)playerPosition.x, (int)playerPosition.y];
    }

    private void OnDisable() 
    {
        foreach (SpriteRenderer item in tiles) Destroy(item.gameObject);
        tiles = null;
    }

    private IEnumerator FakeTileAnimation(Vector2 position)
    {
        tiles[(int)position.x, (int)position.y].sprite = wrongSprite;
        yield return new WaitForSeconds(.25f);
        tiles[(int)position.x, (int)position.y].sprite = null;
        yield return new WaitForSeconds(.25f);
        tiles[(int)position.x, (int)position.y].sprite = wrongSprite;
        yield return new WaitForSeconds(.25f);
        tiles[(int)position.x, (int)position.y].sprite = null;
        yield return new WaitForSeconds(3f);
        tiles[(int)position.x, (int)position.y].sprite = defaultSprite;
    }

}
