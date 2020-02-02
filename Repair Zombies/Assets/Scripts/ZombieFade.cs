using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFade : MonoBehaviour
{
    private void Update()
    {
        transform.position += Vector3.up * .005f;
    }
}
