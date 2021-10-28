using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationLock : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.identity;
    }
}
