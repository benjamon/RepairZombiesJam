using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointSocket : MonoBehaviour
{
    private Limb attachedLimb;

    public static Limb emptyLimb;

    public Limb AttachedLimb
    {
        get
        {
            if (attachedLimb == null) return emptyLimb;
            return attachedLimb;
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

    
}
