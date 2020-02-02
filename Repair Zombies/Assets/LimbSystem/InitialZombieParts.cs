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
            headSocket.AttachLimb(head.BuildLimb());
        }
        if (armFront != null)
        {
            armFrontSocket.AttachLimb(armFront.BuildLimb());
        }
        if (armBack != null)
        {
            armBackSocket.AttachLimb(armBack.BuildLimb());
        }
        if (legFront != null)
        {
            legFrontSocket.AttachLimb(legFront.BuildLimb());
        }
        if (legBack != null)
        {
            legBackSocket.AttachLimb(legBack.BuildLimb());
        }
    }
}
