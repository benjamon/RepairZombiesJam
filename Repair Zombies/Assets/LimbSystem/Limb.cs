using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    public struct limbVals 
    {
        public float movementVal;
        public float damageVal;
        public int legScore;
    }

    public struct limbValsMultiplier
    {
        public float movementMult;
        public float damageMult;
    }

    [SerializeField]
    private limbVals limbValsBase;

    [SerializeField]
    private limbValsMultiplier extremityMult, headMult;

    public limbValsMultiplier HeadMult
    {
        get
        {
            return headMult;
        }
    }

    private bool isExtremity, isHead;
    private JointBall ball;
    private JointSocket socket;


    public limbVals GetVals()
    {
        if(isExtremity) 
        {
            return new limbVals
            {
                movementVal = limbValsBase.movementVal * extremityMult.movementMult,
                damageVal = limbValsBase.damageVal * extremityMult.damageMult
            };
        }

        limbVals prior = socket.AttachedLimb.GetVals();

        return new limbVals
        {
            movementVal = limbValsBase.movementVal + prior.movementVal,
            damageVal = limbValsBase.damageVal + prior.damageVal
        };
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
