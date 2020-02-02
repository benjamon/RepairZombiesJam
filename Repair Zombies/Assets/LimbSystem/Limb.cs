using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    [Serializable]
    public struct LimbVals 
    {
        public float movementVal;
        public float damageVal;
        public int legScore;

        public LimbVals Add(LimbVals _other)
        {
            return new LimbVals {
            movementVal = movementVal + _other.movementVal,
            damageVal = damageVal + _other.damageVal,
            legScore = legScore + _other.legScore
            };
        }

        public LimbVals Multiply(LimbValsMultiplier _mult)
        {
            return new LimbVals {
                movementVal = movementVal * _mult.movementMult,
                damageVal = damageVal * _mult.damageMult,
                legScore = legScore
            };
        }
    }

    [Serializable]
    public struct LimbValsMultiplier
    {
        public float movementMult;
        public float damageMult;
    }

    [SerializeField]
    private LimbVals limbValsBase;

    [SerializeField]
    private LimbValsMultiplier extremityMult;

    [SerializeField]
    public LimbValsMultiplier HeadMult;
    [SerializeField]
    public bool IsExtremity;
    [SerializeField]
    public bool IsHead;
    [SerializeField]
    private bool IsDecomposable;
    [NonSerialized]
    public float Health;
    [SerializeField]
    private float MaxHealth;
    [SerializeField]
    public Limb AttachedLimb;

    public int NumChildren { get; private set; }
    public Joint2D joint2D { get; private set; }

    private Rigidbody2D rigidBody2D;

    //HOOK INTO HIGHLIGHT TO AUTOMATICALLY HIGHLIGHT ALL LIMBS
    //NO OTHER METHODS NECESSARY

    public LimbVals GetVals()
    {
        if(IsExtremity) 
        {
            return limbValsBase.Multiply(extremityMult);
        }
        else return limbValsBase;
    }

    public void DetachAllChildren() 
    {
        AttachedLimb = null;
        NumChildren = 0;
    }

    public void AttachChild(Limb _limb)
    {
        AttachedLimb = _limb;
        NumChildren = _limb.NumChildren + 1;
    }

    public void AttachToBody(Transform _socket)
    {
        transform.parent = _socket;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        GetComponent<SpriteRenderer>().sortingOrder = _socket.GetComponent<SpriteRenderer>().sortingOrder;
        rigidBody2D.isKinematic = true;
        rigidBody2D.simulated = false;
        joint2D.connectedBody = null;
        joint2D.enabled = false;
        if(IsDecomposable) FindObjectOfType<DecayController>().DeregisterLimb(this);
    }

    public void DetachFromBody()
    {
        transform.parent = null;
        rigidBody2D.isKinematic = false;
        rigidBody2D.simulated = true;
        if (IsDecomposable) FindObjectOfType<DecayController>().RegisterLimb(this);

        if (AttachedLimb != null)
        {
            AttachedLimb.joint2D.enabled = true;
            AttachedLimb.joint2D.connectedBody = rigidBody2D;
        }
    }

    public void Highlight()
    {
        if (joint2D.enabled)
        {
            joint2D.connectedBody.GetComponent<Limb>().Highlight();
        }
        else if(AttachedLimb != null)
        {
            AttachedLimb.Highlight();
        }
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public bool TakeDamage(float damage)
    {
        Health -= damage;
        return (Health <= 0);
    }

    public void DestroyLimb()
    {
        if (IsDecomposable) FindObjectOfType<DecayController>().DeregisterLimb(this);
        if (joint2D.enabled)
        {
            joint2D.connectedBody.GetComponent<Limb>().DetachAllChildren();
        }
        if (AttachedLimb != null && AttachedLimb.joint2D.enabled)
        {
            AttachedLimb.joint2D.enabled = false;
            AttachedLimb.joint2D.connectedBody = null;
        }

        Destroy(this);
    }

    public void DestroyAllChildren()
    {
        if (IsDecomposable) FindObjectOfType<DecayController>().DeregisterLimb(this);
        if (joint2D.enabled)
        {
            joint2D.connectedBody.GetComponent<Limb>().DetachAllChildren();
        }
        if (AttachedLimb != null && AttachedLimb.joint2D.enabled)
        {
            AttachedLimb.DetachAllChildren();
        }

        Destroy(this);
    }

    // Start is called before the first frame update
    void Awake()
    {
        joint2D = GetComponent<Joint2D>();
        joint2D.enabled = false;
        joint2D.connectedBody = null;
        if (IsDecomposable) FindObjectOfType<DecayController>().RegisterLimb(this);
        Health = MaxHealth;
        NumChildren = 0;
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
}
