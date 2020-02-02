using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseController : MonoBehaviour
{
    private float? lastMousePoint = null;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePoint = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lastMousePoint = null;
        }
        if (lastMousePoint != null)
        {
            float difference = Input.mousePosition.x - lastMousePoint.Value;
            transform.position = new Vector3(transform.position.x + (difference / 188), transform.position.y, transform.position.z);
            lastMousePoint = Input.mousePosition.x;
        }
    }


    /* public float force;
     GameObject heldObject;
     Vector2 prev;
     Camera c;
     // Start is called before the first frame update
     void Start()
     {
         c = GetComponent<Camera>();
     }

     // Update is called once per frame
     void Update()
     {
         Vector3 v = c.ScreenToWorldPoint(Input.mousePosition);
         v.z = 0f;
         Vector2 delta = v;
         prev = v;
         RaycastHit2D hit = Physics2D.CircleCast(c.ScreenToWorldPoint(Input.mousePosition), .01f, Vector2.zero);
         if (hit.transform != null)
         {
             if (Input.GetMouseButtonDown(0))
             {
                 heldObject = hit.transform.gameObject;
             }
         }
         if (!Input.GetMouseButton(0))
             heldObject = null;

         if (heldObject != null)
         {
             delta = v - heldObject.transform.position; ;
             heldObject.GetComponent<Rigidbody2D>().AddForceAtPosition(delta * force, heldObject.transform.position);
         }

     }
     */
}
