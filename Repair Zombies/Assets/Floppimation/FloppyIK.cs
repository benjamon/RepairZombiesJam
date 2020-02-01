using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloppyIK : MonoBehaviour
{
    public TransformTarget[] Targets;

    private void LateUpdate()
    {
        for (int i = 0; i < Targets.Length; i++)
        {
            Targets[i].MatchTarget();
        }
    }
}

[Serializable]
public class TransformTarget
{
    public Transform Visual;
    public Transform Target;
    public float momentumAmt;
    public float positionRatio;
    public float rotationRatio;

    public void MatchTarget()
    {
        Visual.position = Vector3.Lerp(Visual.position, Target.position, positionRatio);
        Visual.rotation = Quaternion.Lerp(Visual.rotation, Target.rotation, rotationRatio);
        Visual.localScale = Vector3.Lerp(Visual.localScale, Target.localScale, positionRatio);
    }
}
