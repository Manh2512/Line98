using UnityEngine;
using BoardUtilities;
public class SpriteSpawner : MonoBehaviour
{
    // 2D array representing board layout (indices refer to component array)
    public int[,] spriteNames = {
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 2, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

    // Array to hold sprite assets (assign them in the Inspector)
    public Sprite[] components;

    // Size of each cell in world space
    public float cellSize = 0.5f;

    void Start()
    {
        // Initialize the board
        //Spawn sprite based on the board
        SpawnSprites();
    }

    void SpawnSprites()
    {
        int rows = spriteNames.GetLength(0);
        int cols = spriteNames.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int spriteIndex = spriteNames[row, col];

                // Ignore empty slots (assuming 0 means no sprite)
                if (spriteIndex == 0 || spriteIndex >= components.Length)
                    continue;

                // Get the corresponding sprite from the array
                Sprite sprite = components[spriteIndex];

                if (sprite != null)
                {
                    // Create GameObject for the sprite
                    GameObject spriteObject = new GameObject($"Component {spriteIndex}");
                    SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprite;
                    spriteRenderer.sortingOrder = 1;

                    // Position the sprite based on its position in the array
                    spriteObject.transform.position = new Vector3(col * cellSize, -row * cellSize, 0);
                }
                else
                {
                    Debug.LogWarning($"Sprite at index {spriteIndex} is missing!");
                }
            }
        }
    }
}
