using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField]
    private float minLocation;
    [SerializeField]
    private float maxLocation;
    [SerializeField]
    private int movementSpeed;

    [SerializeField]
    private float health;

    [SerializeField]
    private Rigidbody rb;
    private Vector3 currentVelocity;
    [SerializeField]
    private float maximumVelocity;
    private Vector3 userDirection = Vector3.forward;
    [SerializeField]
    private WaterPathingManager waterManager;

    private void Start()
    {
        minLocation = transform.position.x;
        maxLocation = transform.position.x + 190;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(Mathf.PingPong(Time.time * movementSpeed, maxLocation - minLocation) + minLocation, transform.position.y, transform.position.z);
        if (health <= 0)
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
    }

    private void OnMouseDown()
    {
        ReduceHealth(1);
    }

    public void ReduceHealth(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            tag = "Untagged";
            //enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ScoreZone"))
        {
            waterManager.ObjectsScored++;
        }
        if (other.gameObject.CompareTag("LoseScoreZone"))
        {
            if (waterManager.ObjectsScored > 0)
            {
                waterManager.ObjectsScored--;
            }
        }
        if (other.gameObject.CompareTag("DestroyZone"))
        {
            Destroy(gameObject);
        }
    }
}
