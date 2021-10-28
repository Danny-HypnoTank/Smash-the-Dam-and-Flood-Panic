using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaterPathingManager : MonoBehaviour
{
    //Score/tally of objects that have gone the correct way.
    [SerializeField]
    private int objectRequirement;
    private int objectsScored;

    [SerializeField]
    private GameObject[] hazards;
    [SerializeField]
    private Vector3 spawnValues;
    [SerializeField]
    private int hazardCount;
    [SerializeField]
    private float startWait;
    [SerializeField]
    private float spawnWait;
    [SerializeField]
    private float waveWait;

    [SerializeField]
    private TMP_Text scoreText;

    private bool win = false;

    public int ObjectsScored { get => objectsScored; set => objectsScored = value; }

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private void Update()
    {
        scoreText.text = ObjectsScored + "/" + objectRequirement;
        if (objectsScored == objectRequirement)
        {
            win = true;
            
        }

        if (win == true)
        {
            Time.timeScale = 0;
        }
    }

    //Object Spawning - look at object spawning from the heist
    IEnumerator SpawnObjects()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];

                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;

                Instantiate(hazard, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }
}
