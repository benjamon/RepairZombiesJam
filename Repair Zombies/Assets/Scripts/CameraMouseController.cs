using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseController : MonoBehaviour
{
    public float PullForce;
    SpriteRenderer LastHighlighted;
    float lastTimeHighlighted;
    GameObject heldObject;
    Vector2 prev;
    Camera cam;
    public Transform plane;
    Plane castPlane;
    Vector2 lastdelta;


    void Start()
    {
        cam = GetComponent<Camera>();
        castPlane = new Plane(plane.transform.forward, plane.transform.position);
    }
    
    void Update()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);

        Vector3 v = Vector3.zero;

        if (castPlane.Raycast(r, out float d))
        {
        v = r.origin + r.direction.normalized * d;
        }
        v.z = 0f;
        lastdelta = v;
        prev = v;
        RaycastHit2D hit = Physics2D.CircleCast(v, .01f, Vector2.zero, 30f, LayerMask.GetMask("ZombiePart"));
        if (LastHighlighted != null)
            LastHighlighted.color = Color.white;
        if (hit.transform != null)
        {
            Debug.Log(hit.collider.name);

            if (heldObject == null)
            {
                if (hit.collider.GetComponent<SpriteRenderer>() != null)
                {
                    if (LastHighlighted != null)
                        LastHighlighted.color = Color.white;
                    LastHighlighted = hit.collider.GetComponent<SpriteRenderer>();
                    LastHighlighted.color = Color.red;
                    lastTimeHighlighted = Time.time;
                }
            } else if (LastHighlighted != null)
            {
                LastHighlighted.color = Color.white;
                LastHighlighted = null;
            }
            if (Input.GetMouseButtonDown(0))
            {
                heldObject = hit.transform.gameObject;
                heldObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
                SoundManager.PlaySound(Zound.Hit1, heldObject.transform.position);
            }
        }
        if (LastHighlighted != null && Time.time - lastTimeHighlighted > .25f)
        {
            LastHighlighted.color = Color.white;
            LastHighlighted = null;
        }
        if (!Input.GetMouseButton(0) && heldObject != null)
        {
            heldObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
            heldObject = null;
        }
        if (heldObject != null)
            lastdelta = v - heldObject.transform.position;
    }

    private void FixedUpdate()
    {
        if (heldObject != null)
        {
            Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForceAtPosition(lastdelta * PullForce, heldObject.transform.position);
                rb.velocity *= .85f;
            }
        }
    }
}
