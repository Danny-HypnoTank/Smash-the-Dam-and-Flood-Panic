using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPieces : MonoBehaviour
{

    void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.forward * 10, ForceMode.Impulse);
    }


}
