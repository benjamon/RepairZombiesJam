using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] targets;
    public float leftBound = 0;

    void Update() {
        float sum = 0;
        foreach (var target in targets) {
            sum += target.transform.position.x;
        }
        float average = sum / targets.Length;

        transform.position = new Vector3(average, transform.position.y, transform.position.z);

        if (transform.position.x < leftBound) {
            transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.K))
            SoundManager.PlaySound(7, transform.position);
    }
}