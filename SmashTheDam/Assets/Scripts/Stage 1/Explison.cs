using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explison : MonoBehaviour
{
    [SerializeField]
    private float radius = 5f;
    [SerializeField]
    private float force = 700f;

    bool hasExploaded = false;

    [SerializeField]
    private Dam dam;

    void Start()
    {
        
    }

    void Update()
    {
        if (dam.NoHealth == true && hasExploaded == false)
        {
            Explode();
        }
    }

    void Explode ()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearObject in colliders)
        {
            Rigidbody rb = nearObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }
    }
}
