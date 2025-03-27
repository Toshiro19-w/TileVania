using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel(){
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) nextSceneIndex = 0;
        FindFirstObjectByType<ScenePersist>().DestroyScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
