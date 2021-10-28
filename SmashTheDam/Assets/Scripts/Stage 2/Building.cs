using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    private GameObject[] brokenPieces;
    [SerializeField]
    private GameObject[] buildingParticles;

        private void OnTriggerEnter(Collider other)
        {
        if (other.gameObject.CompareTag("Debris"))
        {
            for (int i = 0; i < brokenPieces.Length; i++)
            {
                brokenPieces[i].GetComponent<Rigidbody>().isKinematic = false;
                brokenPieces[i].GetComponent<Rigidbody>().velocity = new Vector3(-30, -50, Random.Range(-50, 50));          
            }

            for (int i = 0; i < buildingParticles.Length; i++)
            {
                buildingParticles[i].SetActive(true);
            }
        }

        if (other.gameObject.CompareTag("SDebris"))
        {
            for (int i = 0; i < brokenPieces.Length; i++)
            {
                brokenPieces[i].GetComponent<Rigidbody>().isKinematic = false;
                brokenPieces[i].GetComponent<Rigidbody>().velocity = new Vector3(-30, -50, Random.Range(-50, 50));
            }

            for (int i = 0; i < buildingParticles.Length; i++)
            {
                buildingParticles[i].SetActive(true);
            }
        }

        if (other.gameObject.CompareTag("DamPieces"))
        {
            for (int i = 0; i < brokenPieces.Length; i++)
            {
                brokenPieces[i].GetComponent<Rigidbody>().isKinematic = false;
                brokenPieces[i].GetComponent<Rigidbody>().velocity = new Vector3(-30, -50, Random.Range(-50, 50));
            }

            for (int i = 0; i < buildingParticles.Length; i++)
            {
                buildingParticles[i].SetActive(true);
            }
        }
    }
}
