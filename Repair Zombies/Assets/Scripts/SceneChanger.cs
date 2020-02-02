using UnityEngine;

public class SceneChanger : MonoBehaviour {
    public GameObject currentSceneRoot;
    public GameObject nextSceneRoot;

    private void OnCollisionEnter2D(Collision2D other) {
        currentSceneRoot.SetActive(false);
        nextSceneRoot.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentSceneRoot.SetActive(false);
        nextSceneRoot.SetActive(true);
    }
}
