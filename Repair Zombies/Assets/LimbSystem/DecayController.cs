using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayController : MonoBehaviour
{
    private List<SocketHandler> sockets = new List<SocketHandler>();
    private List<Limb> limbs = new List<Limb>();

    public float popoffTime = 250f, decayTime = 50f, decayDamage = 0.01f;
    private float popoffTimer = 0f, decayTimer = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        popoffTimer--;
        decayTimer--;
        if (popoffTimer <= 0f)
        {
            sockets[(int)(Random.value * sockets.Count)].LimbRandomDecay(decayDamage);
            popoffTimer = popoffTime/(float)sockets.Count;
        }
        if (decayTimer <= 0f && limbs.Count > 0)
        {
            int index = (int)(Random.Range(0f, limbs.Count));
            if (limbs[index].TakeDamage(decayDamage))
            {
                limbs[index].DestroyLimb();
            }
            decayTimer = decayTime/((float)limbs.Count);
        }
    }

    public void RegisterSocket(SocketHandler _socket)
    {
        sockets.Add(_socket);
    }

    public void DeregisterSocket(SocketHandler _socket)
    {
        sockets.Remove(_socket);
    }

    public void RegisterLimb(Limb _limb)
    {
        limbs.Add(_limb);
    }

    public void DeregisterLimb(Limb _limb)
    {
        limbs.Remove(_limb);

    }
}
