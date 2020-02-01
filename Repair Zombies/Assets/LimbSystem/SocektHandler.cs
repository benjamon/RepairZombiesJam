using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketHandler : MonoBehaviour
{
    private Limb attachedLimb, priorLimb = null;
    private SocketHandler nextSocket;
    [SerializeField]
    private Torso torso;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
