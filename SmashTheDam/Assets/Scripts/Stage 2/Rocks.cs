using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour
{
    [SerializeField]
    private int health;

    private void Update()
    {
        //Check if health is 0 to then play shrinking animation
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void ReduceHealth(int damamge)
    {
        health -= damamge;
    }
}
