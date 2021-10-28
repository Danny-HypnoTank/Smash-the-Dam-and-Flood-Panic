using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{

    [SerializeField]
    private Rigidbody parent;

    [SerializeField]
    private float floaterCount;

    void FixedUpdate()
    {
        UpdateBouyantObjects();
    }

    private void UpdateBouyantObjects()
    {
        parent.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        Vector3 newPos = new Vector3(transform.position.x, WaterCalculation.instance.transform.position.y, transform.position.z);

        newPos = WaterCalculation.instance.GetWavePosition(newPos);
        //floater.points[y].transform.position = new Vector3(floater.points[y].transform.position.x, newPos.y, floater.points[y].transform.position.z);
        if(transform.position.y < newPos.y)
        {
            float displacement = Mathf.Clamp01((newPos.y - transform.position.y) / BuoyancyManager.instance.forces.depth) * BuoyancyManager.instance.forces.displacementAmount;
            parent.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacement, 0), transform.position, ForceMode.Acceleration);
            parent.AddForce(displacement * -parent.velocity * BuoyancyManager.instance.forces.waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            parent.AddTorque(displacement * -parent.angularVelocity * BuoyancyManager.instance.forces.waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
               
        }
    }

    private void SetVariables()
    {

    }
}
