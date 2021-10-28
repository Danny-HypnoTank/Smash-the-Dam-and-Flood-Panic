using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterManager : MonoBehaviour
{
    [SerializeField]
    private int requiredItems;
    [SerializeField]
    private int currentItems;

    public int CurrentItems { get => currentItems; set => currentItems = value; }

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private GameObject Barrier;

    [SerializeField]
    private GameObject cityWater;

    [SerializeField]
    private CamMovement cameraMover;

    [SerializeField]
    private GameObject WaterParticle;

    [SerializeField]
    private GameObject[] barriers;
    [SerializeField]
    private GameObject[] rockParticles;

    private void Start()
    {
        WaterParticle.SetActive(true);
        Barrier.SetActive(true);
        currentItems = requiredItems;
        scoreText.text = currentItems.ToString();
    }

    private void Update()
    {
        //update text constantly
        scoreText.text = currentItems.ToString();

        //Make it so wall breaks as number drops
        if (currentItems == 7)
        {
            barriers[0].SetActive(false);
            barriers[1].SetActive(true);
            rockParticles[0].SetActive(true);
        }
        else if (currentItems == 5)
        {
            barriers[0].SetActive(false);
            barriers[1].SetActive(false);
            barriers[2].SetActive(true);
            rockParticles[1].SetActive(true);
        }
        else if (currentItems == 3)
        {
            barriers[0].SetActive(false);
            barriers[1].SetActive(false);
            barriers[2].SetActive(false);
            barriers[3].SetActive(true);
            rockParticles[2].SetActive(true);
        }
        else if (currentItems <= 0)
        {
            barriers[0].SetActive(false);
            barriers[1].SetActive(false);
            barriers[2].SetActive(false);
            barriers[3].SetActive(false);
            barriers[4].SetActive(true);
            rockParticles[3].SetActive(true);
        }

        if (currentItems <= 0)
        {
            //Barrier.SetActive(false);
            Barrier.GetComponent<BoxCollider>().enabled = false;
            //barriers[4].GetComponent<BoxCollider>().enabled = false;
            WaterParticle.SetActive(false);
            if (cityWater.transform.localScale.y < 135)
            {
                cityWater.transform.localScale = cityWater.transform.localScale + new Vector3(0,1,0);
            }
            cameraMover.playerHasLost = true;
        }
    }
}
