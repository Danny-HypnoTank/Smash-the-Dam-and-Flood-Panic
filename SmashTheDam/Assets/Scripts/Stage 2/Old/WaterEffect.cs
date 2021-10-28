using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private Vector3 userDirection = Vector3.forward;
    [SerializeField]
    private Rigidbody rb;
    private Vector3 currentVelocity;
    [SerializeField]
    private float maximumVelocity;

    //MOVEMENT
    private void Update()
    {
        rb.AddForce(userDirection * movementSpeed, ForceMode.Force);
        currentVelocity = rb.velocity;
        float tempY = currentVelocity.y;
        currentVelocity.y = 0;
        if (currentVelocity.magnitude > maximumVelocity)
        {
            Vector3 direction = currentVelocity.normalized * maximumVelocity;
            direction.y = tempY;
            rb.velocity = direction;
        }
        userDirection = (userDirection + (Vector3.forward - userDirection) * 0.01f).normalized;
    }

    //Collision with rocks
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rocks"))
        {
            //this.transform.position = this.transform.position + new Vector3(10, 0, 0);
            Transform point = collision.gameObject.transform.GetChild(0).transform;
            Vector3 newDir = (transform.position - point.position).normalized;
            newDir = new Vector3(newDir.x, 0, newDir.z).normalized;
            userDirection = newDir;
            Vector3 bounceDir = (Vector3.Reflect(userDirection, point.position - collision.transform.position)).normalized;
            bounceDir = new Vector3(bounceDir.x, 0, bounceDir.z).normalized;
            rb.AddForce(bounceDir * movementSpeed * 2, ForceMode.Impulse);
            if (Vector3.Dot((rb.velocity).normalized, Vector3.forward) > 0)
            {
                rb.velocity = (Vector3.Reflect(rb.velocity, -Vector3.forward) + -Vector3.forward * 2) / 4;
            }
        }
    }

    //Destruction
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DestroyZone"))
        {
            Destroy(gameObject);
        }
    }
}
