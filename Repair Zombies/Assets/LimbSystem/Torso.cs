using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torso : MonoBehaviour
{
    public AnimationState anim = new AnimationState();

    [SerializeField]
    private JointSocket head;
    [SerializeField]
    private JointSocket[] arms;
    [SerializeField]
    private JointSocket[] legs;

    [SerializeField]
    private float movementVal, damageVal;

    private int[] animChangeThreshholds = {2, 4, 6, 8};

    public float MovementVal
    {
        get
        {
            return movementVal;
        }
    }


    public float DamageVal
    {
        get
        {
            return damageVal;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string UpdateLimbs() 
    {
        movementVal = 0;
        damageVal = 0;


        Limb.limbVals _currentVal;
        int _legScore = 0;

        switch (anim.name) 
        {
            case "Walking":
                {
                    foreach(JointSocket socket in arms)
                    {
                        damageVal += socket.AttachedLimb.GetVals().damageVal;
                    }
                    foreach(JointSocket socket in legs) 
                    {
                        _currentVal = socket.AttachedLimb.GetVals();
                        movementVal += _currentVal.movementVal;
                        _legScore += _currentVal.legScore;
                    }
                    break;
                }
            case "Hopping":
                {
                    foreach (JointSocket socket in arms)
                    {
                        damageVal += socket.AttachedLimb.GetVals().damageVal*0.5f;
                    }
                    foreach (JointSocket socket in legs)
                    {
                        _currentVal = socket.AttachedLimb.GetVals();
                        movementVal += _currentVal.movementVal;
                        _legScore += _currentVal.legScore;
                    }
                    break;
                }
            case "Crawling":
                {
                    foreach (JointSocket socket in arms)
                    {
                        movementVal += socket.AttachedLimb.GetVals().movementVal * 0.5f;
                    }
                    foreach (JointSocket socket in legs)
                    {
                        _currentVal = socket.AttachedLimb.GetVals();
                        damageVal += _currentVal.damageVal * 0.5f;
                        _legScore += _currentVal.legScore;
                    }
                    break;
                }
        }

        Limb.limbValsMultiplier _headMult = head.AttachedLimb.HeadMult;
        movementVal *= _headMult.movementMult;
        damageVal *= _headMult.damageMult;

        if (_legScore <= animChangeThreshholds[0])
        {
            return "Crawling";
        }
        else if (_legScore <= animChangeThreshholds[1])
        {
            return "Hopping";
        }
        else if (_legScore <= animChangeThreshholds[2])
        {
            return "Lurching";
        }
        else if (_legScore <= animChangeThreshholds[3])
        {
            return "Hobbling";
        }
        else return "Walking";
    }

}
