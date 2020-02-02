using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayController : MonoBehaviour
{
    private List<SocketHandler> sockets = new List<SocketHandler>();
    float timer = 50f;

    // Update is called once per frame
    void FixedUpdate()
    {
        timer--;
        if (timer <= 0f)
        {
            sockets[(int)Random.value * sockets.Count].LimbRandomDecay();
            timer = 50f;
        }
    }

    public void RegisterSocket(SocketHandler _socket)
    {
        sockets.Add(_socket);
    }
}
