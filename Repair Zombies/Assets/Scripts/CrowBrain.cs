using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrowBrain : MonoBehaviour
{
    public float crowSpeed;
    Transform target;
    List<GameObject> allWithTag = new List<GameObject>();
    public float xDir = 1f;
    bool gotPart;
    float yStart;
    float holdDistance = 0f;
    SpringJoint2D grabJoint;

    private void Start()
    {
        yStart = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindTarget();
        }

        if (target == null || gotPart)
        {
            transform.position += ((Vector3.right * crowSpeed / 2f) * xDir + Vector3.up * (yStart - transform.position.y)).normalized * crowSpeed * Time.deltaTime;
        } else
        {
            transform.position += (target.position - transform.position).normalized * crowSpeed * Time.deltaTime;
            if ((transform.position - target.position).magnitude < crowSpeed * Time.deltaTime)
            {
                target.parent = null;
                target.tag = "Untagged";
                Rigidbody2D body=  target.gameObject.AddComponent<Rigidbody2D>();
                body.drag = 1f;
                body.angularDrag = 1f;
                grabJoint = gameObject.AddComponent<SpringJoint2D>();
                grabJoint.connectedBody = target.GetComponent<Rigidbody2D>();
                grabJoint.autoConfigureDistance = false;
                grabJoint.distance = holdDistance;
                grabJoint.frequency = 5f;
                gotPart = true;
                SoundManager.PlaySound(Zound.CrowCaw, transform.position);
            } else if (xDir > 0f != transform.position.x < target.position.x || (!gotPart && target.tag != "attached"))
            {
                target = null;
            }
        }

        if (grabJoint != null && target == null)
        {
            Destroy(grabJoint);
        }

        transform.rotation = Quaternion.identity;
        transform.right = (target == null || gotPart) ? Vector3.right : (target.position - transform.position);
    }

    

    void FindTarget()
    {
        allWithTag.Clear();
        allWithTag.AddRange(GameObject.FindGameObjectsWithTag("attachable"));
        allWithTag = allWithTag.FindAll((x) => (x.transform.position.x > transform.position.x == xDir > 0));
        if (allWithTag.Count > 0)
            target = allWithTag[Random.Range(0, allWithTag.Count)].transform;
    }
}
