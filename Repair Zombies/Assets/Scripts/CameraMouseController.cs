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

            if (limb != null)
                limb.Highlight();

            if (heldLimb == null)
            {
                if (socket != null && socket.gameObject.layer == LayerMask.NameToLayer("Detachable"))
                    socket.Highlight();
                
            } else
            {
                if (socket != null && socket.CanAttachLimb(heldLimb))
                {
                    socket.Highlight();
                    if (Input.GetMouseButtonUp(0))
                    {
                        heldLimb.GetComponent<Rigidbody2D>().gravityScale = 1f;
                        socket.AttachLimb(heldLimb);
                        heldLimb = null;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (heldLimb == null)
                {
                    if (limb == null && socket != null)
                    {
                        if (socket.gameObject.layer == LayerMask.NameToLayer("Detachable"))
                            heldLimb = socket.DetachLimb();
                    } else if (limb != null)
                    {
                        heldLimb = limb;
                    }
                }

                if (heldLimb != null)
                {
                    heldLimb.GetComponent<Rigidbody2D>().gravityScale = 0f;
                    SoundManager.PlaySound(3, heldLimb.transform.position);
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
