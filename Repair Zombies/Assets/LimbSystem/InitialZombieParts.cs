using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialZombieParts : MonoBehaviour
{
    public FullLimbBuilder head, armFront, armBack, legFront, legBack;

    public SocketHandler headSocket, armFrontSocket, armBackSocket, legFrontSocket, legBackSocket;

    public void AttachAll()
    {

        if (head != null)
        {
            headSocket.AttachLimb(head.BuildLimb(transform.position));
        }
        if (armFront != null)
        {
            armFrontSocket.AttachLimb(armFront.BuildLimb(transform.position));
        }
        if (armBack != null)
        {
            armBackSocket.AttachLimb(armBack.BuildLimb(transform.position));
        }
        if (legFront != null)
        {
            legFrontSocket.AttachLimb(legFront.BuildLimb(transform.position));
        }
        if (legBack != null)
        {
            legBackSocket.AttachLimb(legBack.BuildLimb(transform.position));
        }
    }
}
