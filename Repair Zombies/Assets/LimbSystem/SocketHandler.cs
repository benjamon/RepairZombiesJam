using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SocketHandler : MonoBehaviour
{
    public Limb attachedLimb { get; private set;  }
    [SerializeField]
    private SpriteRenderer highlight;
    [SerializeField]
    private Color highlightColor;
    [SerializeField]
    private float alphaDecay;
    [SerializeField]
    private SocketHandler nextSocket, priorSocket;
    [SerializeField]
    private Torso torso;

    [SerializeField]
    private int currentLimbChildren = 0;

    public int CurrentLimbChildren
    {
        get
        {
            return currentLimbChildren;
        }
    }

    private Collider2D cod;

    [SerializeField]
    public int AvailableChildren;

    //USE CANATTACHLIMB TO CHECK FOR ATTACHABILITY. ANY LIMB CAN BE PASSED IN, INLCUDING MIDDLE PIECES
    //USE ATTACHLIMB TO ATTACH A LIMB. ANY LIMB CAN BE PASSED IN, INLCUDING MIDDLE PIECES
    //USE DETACHLIMB TO DETACH A LIMB AND ALL ITS CHILDREN
    //USE HIGHLIGHT TO AUTOMATICALLY HIGHLIGHT FOR BOTH ATTACHING AND DETACHING


    public bool CanAttachLimb(Limb _limb)
    {
        if (gameObject.layer != LayerMask.NameToLayer("Attachable")   || _limb.NumChildren > AvailableChildren) return false;
        Limb topLimb = _limb;
        while (topLimb.joint2D.enabled)
        {
            topLimb = topLimb.joint2D.connectedBody.GetComponent<Limb>();
            if (topLimb.NumChildren > AvailableChildren) return false;
        }

        return true;
    }

    public void AttachLimb(Limb _limb)
    {
        Limb topLimb = _limb;
        while (topLimb.joint2D.enabled)
        {
            topLimb = topLimb.joint2D.connectedBody.GetComponent<Limb>();
        }

        if (priorSocket != null && priorSocket.attachedLimb != null)
        {
            priorSocket.attachedLimb.AttachChild(topLimb);
        }

        attachedLimb = topLimb;
        topLimb.AttachToBody(transform);
        gameObject.layer = LayerMask.NameToLayer("Detachable");
        FindObjectOfType<DecayController>().RegisterSocket(this);

        if (nextSocket != null && !topLimb.IsExtremity) 
        {
            if (topLimb.AttachedLimb == null)
            {
                nextSocket.gameObject.layer = LayerMask.NameToLayer("Attachable");
            }
            else
            {
                nextSocket.ChainAttach(topLimb.AttachedLimb);
            }
        }
        torso.GetLimbVals();
    }

    private void ChainAttach(Limb _limb) 
    {
        attachedLimb = _limb;
        _limb.AttachToBody(transform);
        gameObject.layer = LayerMask.NameToLayer("Detachable");
        FindObjectOfType<DecayController>().RegisterSocket(this);

        if (nextSocket != null && !_limb.IsExtremity)
        {
            if (_limb.AttachedLimb == null)
            {
                nextSocket.gameObject.layer = LayerMask.NameToLayer("Attachable");
            }
            else
            {
                nextSocket.ChainAttach(_limb.AttachedLimb);
            }
        }
    }

    public Limb DetachLimb()
    {
        if (priorSocket != null && priorSocket.attachedLimb != null)
        {
            priorSocket.attachedLimb.DetachAllChildren();
        }

        gameObject.layer = LayerMask.NameToLayer("Attachable");
        attachedLimb.DetachFromBody();
        FindObjectOfType<DecayController>().DeregisterSocket(this);

        if (nextSocket != null && nextSocket.attachedLimb != null)
        {
            nextSocket.ChainDetach();
        }
        Limb temp = attachedLimb;
        attachedLimb = null;

        torso.GetLimbVals();

        return temp;
    }

    private void ChainDetach()
    {
        attachedLimb.DetachFromBody();
        gameObject.layer = LayerMask.NameToLayer("Inactive");
        FindObjectOfType<DecayController>().DeregisterSocket(this);

        if (nextSocket != null && nextSocket.attachedLimb != null)
        {
            nextSocket.ChainDetach();
        }

        attachedLimb = null;
    }

    public Limb.LimbVals GetVals()
    {


        if(attachedLimb == null)
        {
            currentLimbChildren = -1;
            return new Limb.LimbVals
            {
                movementVal = -0.5f,
                damageVal = -1f,
                legScore = 0
            };
        }
        currentLimbChildren = attachedLimb.NumChildren;
        if (nextSocket != null && nextSocket.attachedLimb != null)
        {
            return attachedLimb.GetVals().Add(nextSocket.GetVals());
        }
        else return attachedLimb.GetVals();
    }

    public Limb.LimbValsMultiplier GetHeadMult()
    {
        if (attachedLimb == null)
        {
            return new Limb.LimbValsMultiplier
            {
                movementMult = 0.5f,
                damageMult = 0.5f
            };
        }
        return attachedLimb.HeadMult;
    }

    public void TakeDamage(float damage)
    {
        if(nextSocket.attachedLimb == null || Random.Range(0f, 1.0f) > 0.5f)
        {
            if (attachedLimb.TakeDamage(damage))
            {
                Limb temp = attachedLimb;
                DetachLimb();
                temp.DestroyLimb();
            }
        }
        else
        {
            nextSocket.TakeDamage(damage);
        }
    }

    public void LimbRandomDecay(float damage)
    {
        if(attachedLimb != null)
        {
            float r = Random.Range(attachedLimb.Health*0.15f, attachedLimb.Health);
            if (r < 0.5 - attachedLimb.Health*0.3)
            {
                DetachLimb();
            }
        }
    }

    public void Highlight()
    {
            highlight.color = new Color(highlightColor.r, highlightColor.g, highlightColor.b, 1f);
    }

    private void Update()
    {
        highlight.color = new Color(highlight.color.r, highlight.color.g, highlight.color.b, highlight.color.a - alphaDecay);
    }

    const float SOCKET_SIZE = .55f;

    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<CircleCollider2D>() == null)
        {
            cod = gameObject.AddComponent<CircleCollider2D>();
            cod.isTrigger = true;
        }
        GetComponent<CircleCollider2D>().radius = SOCKET_SIZE;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("invader: " + collision.transform.name);
    }
}
