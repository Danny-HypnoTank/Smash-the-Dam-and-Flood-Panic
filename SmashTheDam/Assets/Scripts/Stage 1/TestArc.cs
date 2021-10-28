using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArc : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Dam damManager;
    [SerializeField]
    private GameObject explosion;

    void Start()
    {
        Vector3 direction = ((damManager.WorldPos - this.gameObject.transform.position).normalized);
        rb.AddForce(( direction +  new Vector3(0,0.75f,0))* 12, ForceMode.Impulse);    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dam"))
        {
            Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        } 
    }
}
