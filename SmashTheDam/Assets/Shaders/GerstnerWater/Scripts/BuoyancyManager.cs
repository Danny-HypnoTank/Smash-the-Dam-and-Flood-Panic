using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyManager : MonoBehaviour
{
    public static BuoyancyManager instance;

    public FloaterForces forces;

    [System.Serializable]
    public struct FloaterForces
    {
        public float depth, displacementAmount, waterDrag, waterAngularDrag;
    }

    void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }
}
