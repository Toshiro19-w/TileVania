using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playersLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    void Awake()
    {
        int numGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playersLives.ToString();
        scoreText.text = score.ToString();   
    }

    public void ProcessPlayerDeath(){
        if(playersLives > 1){
            TakeLife();
        } else {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    public void ResetGameSession()
    {
        FindFirstObjectByType<ScenePersist>().DestroyScenePersist();
        SceneManager.LoadScene(0);
        score = 0;
        scoreText.text = score.ToString();
        Destroy(gameObject);
    }

    public void TakeLife()
    {
        playersLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playersLives.ToString();
    }
}
