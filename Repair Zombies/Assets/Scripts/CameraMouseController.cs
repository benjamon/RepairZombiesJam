using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseController : MonoBehaviour
{
    public float PullForce;
    SpriteRenderer LastHighlighted;
    float lastTimeHighlighted;
    Vector2 prev;
    Camera cam;
    public Transform plane;
    Plane castPlane;
    Vector2 lastdelta;
    Limb heldLimb;
    public LayerMask mask;


    void Start()
    {
        cam = GetComponent<Camera>();
        castPlane = new Plane(plane.transform.forward, plane.transform.position);
    }
    
    void Update()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);

        Vector3 mouseLocation = Vector3.zero;

        if (castPlane.Raycast(r, out float d))
        {
            mouseLocation = r.origin + r.direction.normalized * d;
        }
        mouseLocation.z = 0f;
        lastdelta = mouseLocation;
        prev = mouseLocation;
        RaycastHit2D hit = Physics2D.CircleCast(mouseLocation, .01f, Vector2.zero, 30f, mask);
        if (LastHighlighted != null)
            LastHighlighted.color = Color.white;

        //hit some collider
        if (hit.transform != null)
        {
            Limb limb = hit.collider.GetComponent<Limb>();
            SocketHandler socket = hit.collider.GetComponent<SocketHandler>();

            if (heldLimb == null)
            {
                if (hit.collider.GetComponent<SpriteRenderer>() != null)
                {
                    if (LastHighlighted != null)
                        LastHighlighted.color = Color.white;
                    LastHighlighted = hit.collider.GetComponent<SpriteRenderer>();
                    LastHighlighted.color = Color.red;
                    lastTimeHighlighted = Time.time;
                }
            } else 
            {
                if (LastHighlighted != null)
                {
                    LastHighlighted.color = Color.white;
                    LastHighlighted = null;
                }
                if (socket != null)
                {
                    if (socket.CanAttachLimb(heldLimb))
                        socket.AttachLimb(heldLimb);
                    heldLimb = null;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (heldLimb == null)
                {
                    Debug.Log("A");
                    if (limb == null && socket != null)
                    {
                        Debug.Log("DETACH");
                        if (socket.gameObject.layer == LayerMask.NameToLayer("Detachable"))
                            heldLimb = socket.DetachLimb();
                        Debug.Log(socket.name);
                    } else if (limb != null)
                    {
                        Debug.Log("GRAB LIMB");
                        heldLimb = limb;
                    }
                }

                if (heldLimb != null)
                {
                    heldLimb.GetComponent<Rigidbody2D>().gravityScale = 0f;
                    SoundManager.PlaySound(Zound.Hit1, heldLimb.transform.position);
                }
            }
        } else
        {
            if (!Input.GetMouseButton(0) && heldLimb != null)
            {
                heldLimb.GetComponent<Rigidbody2D>().gravityScale = 1f;
                heldLimb = null;
            }
        }

        if (!Input.GetMouseButton(0) && heldLimb != null)
        {
            heldLimb.GetComponent<Rigidbody2D>().gravityScale = 1f;
            heldLimb = null;
        }

        //PROBABLY REMOVE
        if (LastHighlighted != null && Time.time - lastTimeHighlighted > .25f)
        {
            LastHighlighted.color = Color.white;
            LastHighlighted = null;
        }

        if (heldLimb != null)
        {
            lastdelta = mouseLocation - heldLimb.transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (heldLimb != null)
        {
            Rigidbody2D rb = heldLimb.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForceAtPosition(lastdelta * PullForce, heldLimb.transform.position);
                rb.velocity *= .85f;
            }
        }
    }
}
