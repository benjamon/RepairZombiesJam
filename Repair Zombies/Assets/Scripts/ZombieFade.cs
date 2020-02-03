using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFade : MonoBehaviour
{
    float speed = -.1f;
    private void Update()
    {
        speed += .001f;
        transform.position += Vector3.up * Mathf.Clamp01(speed);
    }

   
}
