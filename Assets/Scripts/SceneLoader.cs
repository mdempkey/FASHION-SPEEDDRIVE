using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public void LoadGameScene() {
        SceneManager.LoadScene("Scenes/GameScene");
    }
    
    public void LoadGameScene1() {
        SceneManager.LoadScene("Scenes/AboutScreen");
    }
    
    public void LoadGameScene2() {
        SceneManager.LoadScene("Scenes/MainScreen");
    }
}
