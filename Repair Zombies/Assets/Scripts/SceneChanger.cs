using UnityEngine;

public class SceneChanger : MonoBehaviour {
    public GameObject currentSceneRoot;
    public GameObject nextSceneRoot;

    private void OnCollisionEnter2D(Collision2D other) {
        currentSceneRoot.SetActive(false);
        nextSceneRoot.SetActive(true);
    }
}
