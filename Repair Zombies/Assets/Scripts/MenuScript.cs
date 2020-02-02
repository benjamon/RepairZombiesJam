using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public string startGameAction;
    public string sceneName;

    void Update() {
        if (Input.GetButtonUp(startGameAction)) {
            SceneManager.LoadScene(sceneName);
        } 
    }
}
