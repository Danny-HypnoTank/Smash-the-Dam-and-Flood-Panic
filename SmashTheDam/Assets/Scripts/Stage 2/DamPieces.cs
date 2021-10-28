using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamPieces : MonoBehaviour
{
    //Script dam pieces to add force on start and collision events for stop movement and buildings
    [SerializeField]
    private Rigidbody rb;

    private void Start()
    {
        rb.AddForce(Vector3.forward * 500, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("City"))
        {
            rb.drag = 1;
            rb.angularDrag = 1;
        }

        if (other.CompareTag("Finish"))
        {
            Destroy(this.gameObject);
        }
    }
}
