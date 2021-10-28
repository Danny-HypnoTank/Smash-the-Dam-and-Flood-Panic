using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StartingDebris : MonoBehaviour
{
    [SerializeField]
    private WaterManager gameManager;
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private int health;

    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private int movementSpeed;
    [SerializeField]
    private Vector3 moveDirection = Vector3.forward;
    private Vector3 currentVelocity;
    [SerializeField]
    private float minimumVelocity;
    [SerializeField]
    private float maximumVelocity;
    [SerializeField]
    private float absoluteMin;
    [SerializeField]
    private float absoluteMax;
    [SerializeField]
    private float initialMin;
    [SerializeField]
    private float initialMax;
    [SerializeField]
    private int bounceSpeed;
    [SerializeField]
    private float cameraDistance;
    [SerializeField]
    private float maximumDistance;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject splashParticle;

    private bool stopped = false;
    private bool done = false;
    private bool completed = false;

    public float debugdistance = 0;
    public Vector3 debugVector = new Vector3(0,0,0);

    private void Start()
    {
        rb.AddForce(Vector3.up * bounceSpeed, ForceMode.Impulse);
    }

    private void Update()
    {
        if (health <= 0)
        {
            rb.AddForce(moveDirection * movementSpeed, ForceMode.Force);
            currentVelocity = rb.velocity;
            float tempY = currentVelocity.y;
            currentVelocity.y = 0;

            float distanceToCamera = Vector3.Magnitude((new Vector3(mainCamera.transform.position.x, 0, mainCamera.transform.position.z) - transform.position));
            debugVector = new Vector3(mainCamera.transform.position.x, 0, mainCamera.transform.position.z);
            //minimumVelocity = Mathf.Min((initialMin * distanceToCamera), absoluteMin);
            //maximumVelocity = Mathf.Min((initialMax * distanceToCamera), absoluteMax);

            float t = distanceToCamera / maximumDistance;
            minimumVelocity = Mathf.Lerp(absoluteMin, initialMin, t);
            maximumVelocity = Mathf.Lerp(absoluteMax, initialMax, t);

            if (currentVelocity.magnitude < minimumVelocity)
            {
                Vector3 direction = currentVelocity.normalized * minimumVelocity;
                direction.y = tempY;
                rb.velocity = direction;
            }
            if (currentVelocity.magnitude > maximumVelocity)
            {
                Vector3 direction = currentVelocity.normalized * maximumVelocity;
                direction.y = tempY;
                rb.velocity = direction;
            }

            moveDirection = (moveDirection + (Vector3.forward - moveDirection) * 50f).normalized; //0.1f

            float cameraToObjectDotProduct = Vector3.Dot((transform.position - mainCamera.transform.position).normalized, Vector3.forward);

            if (gameManager.CurrentItems > 0 && stopped == false)
            {
                if (cameraToObjectDotProduct < cameraDistance) //-1800
                {
                    moveDirection = Vector3.forward;
                    if (Vector3.Dot(rb.velocity.normalized, Vector3.forward) < 0.7)
                    {
                        rb.velocity = Vector3.forward;
                    }
                }
            }
            else if (stopped == true && done == false)
            {
                rb.drag = 1;
                rb.angularDrag = 1;
            }
            else if (done == true)
            {
                rb.velocity = Vector3.zero;
            }
            else if (gameManager.CurrentItems <= 0 && completed == false)
            {
                rb.AddForce(Vector3.forward * 2000, ForceMode.Impulse);
                completed = true;
            }
            debugdistance = distanceToCamera;
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.Label(transform.position + new Vector3(0, 1, 0), debugdistance.ToString());
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Bounces off rocks when collides
        if (collision.gameObject.CompareTag("Rocks"))
        {
            Transform point = collision.gameObject.transform.GetChild(0).transform;
            Vector3 newDir = (transform.position - point.position).normalized;
            newDir = new Vector3(newDir.x, 0, newDir.z).normalized;
            moveDirection = newDir;
            Vector3 bounceDir = (Vector3.Reflect(moveDirection, point.position - collision.transform.position)).normalized;
            bounceDir = new Vector3(bounceDir.x, 0, bounceDir.z).normalized;
            rb.AddForce(bounceDir * movementSpeed * 2, ForceMode.Impulse);
            if (Vector3.Dot((rb.velocity).normalized, Vector3.forward) > 0)
            {
                rb.velocity = (Vector3.Reflect(rb.velocity, -Vector3.forward) + -Vector3.forward * 2) / 4;
            }

            //Add in reduction of health for rock
            collision.gameObject.GetComponent<Rocks>().ReduceHealth(1);
        }

        //Collision with other debris
        if (collision.gameObject.CompareTag("Debris"))
        {
            collision.gameObject.GetComponent<Debris>().ReduceHealth(1);
        }
        else if (collision.gameObject.CompareTag("SDebris"))
        {

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Multiplier"))
        {
            animator.SetBool("Animate", true);
            StartCoroutine(AnimationReset());
            other.gameObject.GetComponent<Animator>().SetBool("Used", true);
            //Duplicate Object
            GameObject duplicate = Instantiate(this.gameObject);
            duplicate.transform.position = this.transform.position + new Vector3(20, 0, 0);
            duplicate.GetComponent<Animator>().SetBool("Animate", true);
            duplicate.GetComponent<StartingDebris>().StartCoroutine(AnimationReset());
            other.gameObject.tag = "Untagged";
        }
        if (other.CompareTag("KillZone"))
        {
            Destroy(this.gameObject);
        }
        if (other.CompareTag("Whirlpool"))
        {
            Instantiate(splashParticle, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.Euler(90f, 0f, 0f));
            //other.gameObject.tag = "Untagged";
            health = 1;
            rb.velocity = new Vector3(0, -20, 0);
            //Make it so the object falls through the level
            this.gameObject.layer = LayerMask.NameToLayer("Null");
        }
        //ticks down number and breaks wall when collides
        if (other.gameObject.CompareTag("Wall"))
        {
            gameManager.CurrentItems--;
        }
        if (other.gameObject.CompareTag("City"))
        {
            stopped = true;
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            done = true;
        }
    }

    public IEnumerator AnimationReset()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("Animate", false);
    }
}
