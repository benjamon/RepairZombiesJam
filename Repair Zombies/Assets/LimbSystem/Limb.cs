using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
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

    public struct LimbValsMultiplier
    {
        public float movementMult;
        public float damageMult;
    }

    [SerializeField]
    private LimbVals limbValsBase;

    [SerializeField]
    private LimbValsMultiplier extremityMult;

    private Rigidbody2D rigidBody2D;
    private Joint2D joint2D;

    public Limb AttachedLimb { get; set; }
    public int NumChildren { get; private set; }
    public LimbValsMultiplier HeadMult { get; }
    public bool IsExtremity { get; }
    public bool IsHead { get; }


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
        rigidBody2D.isKinematic = true;
        rigidBody2D.simulated = false;
        joint2D.connectedBody = null;
    }

    public void DetachFromBody()
    {
        transform.parent = null;
        rigidBody2D.isKinematic = false;
        rigidBody2D.simulated = true;

        if(AttachedLimb != null)
        {
            AttachedLimb.joint2D.connectedBody = rigidBody2D;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        joint2D = GetComponent<Joint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
