using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.U2D;
using BoardUtilities;
using Utilities;

public class generate_sprawn : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteAtlas spriteAtlas;
    Vector2 size;
    int x_begin;
    int y_begin;
    int score = 0;
    int high_score = 0;
    public GameObject spritePrefab;
    
    void Awake()
    {
        spriteAtlas = Resources.Load<SpriteAtlas>("Balls"); // Load Atlas
        size = new Vector2(4.9f, 5.8f);
        x_begin = -1;
        y_begin = -1;
    }
    void Start()
    {
        // Initialize the ball_map
        spritePrefab = Resources.Load<GameObject>("Circle");
        Board.initialize();
        VisualizeMap(Board.ball_map);
        Board.generate_next();
        VisualizePredict(Board.next_balls);
    }

    void Update(){
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 index = BoardPosition.FromCoorToIndex(mousePos, size, transform.position);
            Debug.Log($"Mouse clicked at world position: {index}");

            // Get the movement
            // If the clicked position has ball --> change begin
            // If there is no ball and begin != -1 -->move
            if(Board.ball_map[(int)index.x, (int)index.y] != 0){
                x_begin = (int)index.x;
                y_begin = (int)index.y;
            }else if(x_begin != -1 && y_begin != -1){
                int x_end = (int)index.x;
                int y_end = (int)index.y;
                int color = Board.ball_map[x_begin, y_begin];
                (bool valid, int added_score) = Board.move(x_begin, y_begin, x_end, y_end);
                if(valid){
                    MoveSprite(color, x_begin, y_begin, x_end, y_end);
                    if(added_score > 0){
                        score += added_score;
                        ScoreManager.instance.AddPoint(added_score);
                        ResetBoard();
                    }else{
                        // Make next balls appear
                        Board.show_next(); // In the ball map
                        VisualizeNext(Board.next_balls); // On UI
                        added_score = Board.proceed_next();
                        score += added_score;
                        ScoreManager.instance.AddPoint(added_score);
                        Board.generate_next();
                        ResetBoard();
                        VisualizePredict(Board.next_balls); // Visualize next balls
                    }
                }
                x_begin = -1;
                y_begin = -1;
            }
            if(Board.game_end()){
                GameOver();
            }
        }
    }
    // Function to print ball_map to console for debugging --> ball_map is synced
    void PrintArray(int[,] array)
    {
        string output = "Array:\n";
        
        for (int i = 0; i < array.GetLength(0); i++) // Rows
        {
            for (int j = 0; j < array.GetLength(1); j++) // Columns
            {
                output += array[i, j] + " ";
            }
            output += "\n"; // New line after each row
        }

        Debug.Log(output);
    }
    // Update is called once per frame
    void GenerateSprite(int color, int i, int j)
    {
        if (spriteAtlas == null)
        {
            Debug.LogError("Sprite Atlas not found!");
            return;
        }

        // 🔹 Get the sprite from the atlas
        Sprite sprite = spriteAtlas.GetSprite("Component " + color);

        if (sprite != null)
        {
            Vector2 pos = BoardPosition.FromIndexToCoor(transform.position, i, j);
            GameObject spriteObject = Instantiate(spritePrefab, pos, Quaternion.identity);
            spriteObject.name = $"GeneratedSprite ({color}, {i}, {j})";
            // GameObject spriteObject = new GameObject($"GeneratedSprite ({color}, {i}, {j})");

            // 🔹 Add SpriteRenderer component
            SpriteRenderer renderer = spriteObject.GetComponent<SpriteRenderer>();

            // 🔹 Assign the loaded sprite
            renderer.sprite = sprite;
            renderer.sortingOrder = 1;

            // 🔹 Set position
            spriteObject.transform.position = new Vector3(pos.x, pos.y, 0);

            Debug.Log("Sprite generated successfully from Sprite Atlas!");
        }
        else
        {
            Debug.LogError("Sprite not found in Sprite Atlas!");
        }
    }

    void GeneratePredict(int color, int i, int j){
        // Visualize predicted balls
        if (spriteAtlas == null)
        {
            Debug.LogError("Sprite Atlas not found!");
            return;
        }

        // 🔹 Get the sprite from the atlas
        Sprite sprite = spriteAtlas.GetSprite("Component " + color);

        if (sprite != null)
        {
            Vector2 pos = BoardPosition.FromIndexToCoor(transform.position, i, j);
            GameObject spriteObject = new GameObject($"PredictSprite ({color}, {i}, {j})");

            // 🔹 Add SpriteRenderer component
            SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();

            // 🔹 Assign the loaded sprite
            renderer.sprite = sprite;
            renderer.sortingOrder = 1;

            // 🔹 Set position
            spriteObject.transform.position = new Vector3(pos.x, pos.y, 0);
            spriteObject.transform.localScale = new Vector3(0.4f, 0.4f, 1f);

            Debug.Log("Sprite generated successfully from Sprite Atlas!");
        }
        else
        {
            Debug.LogError("Sprite not found in Sprite Atlas!");
        }
    }

    void VisualizeMap(int[,] ball_map){
        // Visualize the ball map
        for(int i=0; i<9; i++){
            for(int j=0; j<9; j++){
                if(ball_map[i,j] != 0){
                    GenerateSprite(ball_map[i,j], i, j);
                }
            }
        }
    }
    
    void VisualizePredict(int[,] next_balls){
        // Visualize predicted balls as small balls
        int rows = next_balls.GetLength(0);
        for(int i=0; i<rows; i++){
            GeneratePredict(next_balls[i,0], next_balls[i,1], next_balls[i,2]);
        }
        return;
    }
    void VisualizeNext(int[,] next_balls){
        // Delete predicted
        int n = 0;
        Regex regex = new Regex(@"^PredictSprite \((\d+), (\d+), (\d+)\)$");

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if(n >= 3){
                break;
            }
            if (regex.IsMatch(obj.name))
            {
                Match match = regex.Match(obj.name);
                if(match.Success){
                    int color = int.Parse(match.Groups[1].Value);
                    int x = int.Parse(match.Groups[2].Value);
                    int y = int.Parse(match.Groups[3].Value);
                    DeleteSprite("PredictSprite", color, x, y);
                    n++;
                }
            }
        }
        // Visualize next_balls as normal balls
        int rows = next_balls.GetLength(0);
        for(int i=0; i<rows; i++){
            GenerateSprite(next_balls[i,0], next_balls[i,1], next_balls[i,2]);
        }
    }

    void MoveSprite(int color, int x_begin, int y_begin, int x_end, int y_end){
        GameObject spriteObject = GameObject.Find($"GeneratedSprite ({color}, {x_begin}, {y_begin})");

        if (spriteObject != null)
        {
            Vector2 new_position = BoardPosition.FromIndexToCoor(transform.position, x_end, y_end);
            spriteObject.transform.position = new Vector3(new_position.x, new_position.y, 0f);
            spriteObject.name = $"GeneratedSprite ({color}, {x_end}, {y_end})";
        }
        else
        {
            Debug.LogError($"GeneratedSprite ({color}, {x_begin}, {y_begin}) not found!");
        }

    }

    void DeleteSprite(string name, int color, int x, int y){
        GameObject spriteObject = GameObject.Find($"{name} ({color}, {x}, {y})");

        if (spriteObject != null)
        {
            Destroy(spriteObject, 0.1f); // Deletes the GameObject
            Debug.Log("Deleted: " + spriteObject.name);
        }
        else
        {
            Debug.LogError($"Sprite ({color}, {x}, {y}) not found!");
        }

    }

    void ResetBoard(){
        // Compare map and current sprite
        // If a current sprite should have been deleted following
        // the map, delete it

        // Get list of current sprites
        Regex regex = new Regex(@"^GeneratedSprite \((\d+), (\d+), (\d+)\)$");
        List<string> matchingSprites = new List<string>();

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (regex.IsMatch(obj.name))
            {
                matchingSprites.Add(obj.name);
            }
        }

        foreach(string name in matchingSprites){
            // Get the parameters to compare
            Match match = regex.Match(name);
            if(match.Success){
                int color = int.Parse(match.Groups[1].Value);
                int x = int.Parse(match.Groups[2].Value);
                int y = int.Parse(match.Groups[3].Value);
                
                // Compare
                // Delete the corresponding
                if(Board.ball_map[x, y] == 0){
                    DeleteSprite("GeneratedSprite", color, x, y);
                }
            }
        }
    }

    void GameOver(){
        // Reset everything
        high_score = score;
        score = 0;
        ScoreManager.instance.UpdateHighScore();
        x_begin = -1;
        y_begin = -1;
        // Empty the ball_map
        Board.ball_map = new int[,] {
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0}
        };
        // Empty the UI
        // Empty normal balls
        ResetBoard();
        // Empty predicted balls
        int n = 0;
        Regex regex = new Regex(@"^PredictSprite \((\d+), (\d+), (\d+)\)$");

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if(n >= 3){
                break;
            }
            if (regex.IsMatch(obj.name))
            {
                Match match = regex.Match(obj.name);
                if(match.Success){
                    int color = int.Parse(match.Groups[1].Value);
                    int x = int.Parse(match.Groups[2].Value);
                    int y = int.Parse(match.Groups[3].Value);
                    DeleteSprite("PredictSprite", color, x, y);
                    n++;
                }
            }
        }
        // Call Start() function again
        Start();
    }
}
