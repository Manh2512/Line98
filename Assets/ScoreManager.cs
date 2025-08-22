using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    int score;
    int high_score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI high_scoreText;
    // Start is called before the first frame update
    private void Awake(){
        instance = this;
    }
    void Start()
    {
        Debug.Log("This ScoreManager code is running");
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            Debug.Log("Canvas component enabled: " + canvas.enabled);
        }
        
        Debug.Log("Canvas GameObject active in hierarchy: " + gameObject.activeInHierarchy);
        scoreText.text = "Score: " + score.ToString();
        high_scoreText.text = "High score: " + high_score.ToString();
    }

    // Update is called once per frame
    public void AddPoint(int added_score)
    {
        score += added_score;
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateHighScore(){
        high_score = score;
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        high_scoreText.text = "High score: " + high_score.ToString();
    }
}
