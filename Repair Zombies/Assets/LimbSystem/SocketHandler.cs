using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SocketHandler : MonoBehaviour
{
    private Limb attachedLimb, priorLimb = null;
    [SerializeField]
    private SocketHandler nextSocket;
    [SerializeField]
    private Torso torso;

    [SerializeField]
    public int AvailableChildren { get; private set; }

    public bool IsAttachable { get; private set; }

    public bool IsRemoveAble() { return attachedLimb != null; }


    public void AttachLimb(Limb _limb)
    {
        if (priorLimb != null)
        {
            priorLimb.AttachChild(_limb);
        }

        attachedLimb = _limb;
        _limb.AttachToBody(transform);
        IsAttachable = false;

        if (nextSocket != null) 
        {
            if (_limb.AttachedLimb == null)
            {
                nextSocket.IsAttachable = true;
            }
            else if (!_limb.IsExtremity)
            {
                nextSocket.ChainAttach(_limb.AttachedLimb);
            }
        }

        torso.GetLimbVals();
    }

    private void ChainAttach(Limb _limb) 
    {
        attachedLimb = _limb;
        _limb.AttachToBody(transform);
        IsAttachable = false;

        if (nextSocket != null)
        {
            if (_limb.AttachedLimb == null)
            {
                nextSocket.IsAttachable = true;
            }
            else if (!_limb.IsExtremity)
            {
                nextSocket.ChainAttach(_limb.AttachedLimb);
            }
        }
    }

    public void DetachLimb()
    {
        if (priorLimb != null)
        {
            priorLimb.DetachAllChildren();
        }

        IsAttachable = true;
        attachedLimb.DetachFromBody();

        if (nextSocket != null && attachedLimb.AttachedLimb != null)
        {
            nextSocket.ChainDetach();
        }

        attachedLimb = null;

        torso.GetLimbVals();
    }

    private void ChainDetach()
    {
        attachedLimb.DetachFromBody();

        if (nextSocket != null && attachedLimb.AttachedLimb != null)
        {
            nextSocket.ChainDetach();
        }

        attachedLimb = null;
    }

    public Limb.LimbVals GetVals()
    {
        if (nextSocket.attachedLimb != null)
        {
            return attachedLimb.GetVals().Add(nextSocket.GetVals());
        }
        else return attachedLimb.GetVals();
    }

    public Limb.LimbValsMultiplier GetHeadMult()
    {
        return attachedLimb.HeadMult;
    }

    public void TakeDamage(float damage)
    {
        if(nextSocket.attachedLimb == null || Random.Range(0f, 1.0f) > 0.5f)
        {
            attachedLimb.Health -= damage;
        }
        else
        {
            nextSocket.TakeDamage(damage);
        }
    }

    public void LimbRandomDecay()
    {
        if(attachedLimb != null)
        {
            float r = Random.Range(1f - attachedLimb.Health, 1f);
            if (r > 0.9)
            {
                DetachLimb();
            }
            else
            {
                attachedLimb.Health -= r * 0.125f;
            }
        }
    }

    const float SOCKET_SIZE = .55f;

    // Start is called before the first frame update
    void Awake()
    {
        FindObjectOfType<DecayController>().RegisterSocket(this);

        if (GetComponent<CircleCollider2D>() == null)
        {
            CircleCollider2D cod = gameObject.AddComponent<CircleCollider2D>();
            cod.isTrigger = true;
        }
        GetComponent<CircleCollider2D>().radius = SOCKET_SIZE;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("invader: " + collision.transform.name);
    }
}
