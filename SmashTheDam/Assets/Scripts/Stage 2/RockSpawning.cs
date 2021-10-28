using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawning : MonoBehaviour
{
    public GameObject[] prefabs; //Prefabs to spawn

    public Camera c;
    public int selectedPrefab = 0;
    public int rayDistance = 300;
    [Tooltip("Distance between the previous rock and the next one to be placed.")]
    public double rockDistance = 1;

    [SerializeField]
    private GameObject rockEffect;

    private Vector3 previousPosition = Vector3.zero;
    private double sqrDistance;
    // Start is called before the first frame update
    void Start()
    {
        if (prefabs.Length == 0)
        {
            Debug.LogError("You haven't assigned any Prefabs to spawn");
        }

        sqrDistance = rockDistance * rockDistance;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    selectedPrefab++;
        //    if (selectedPrefab >= prefabs.Length)
        //    {
        //        selectedPrefab = 0;
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    selectedPrefab--;
        //    if (selectedPrefab < 0)
        //    {
        //        selectedPrefab = prefabs.Length - 1;
        //    }
        //}

        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
        {
            //Remove spawned prefab when holding left shift and left clicking
            Transform selectedTransform = GetObjectOnClick();
            if (selectedTransform)
            {
                Destroy(selectedTransform.gameObject);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            //On left click spawn selected prefab and align its rotation to a surface normal
            Vector3[] spawnData = GetClickPositionAndNormal();
            if (spawnData[0] != Vector3.zero)
            {
                if(previousPosition != Vector3.zero)
                {
                    if ((spawnData[0] - previousPosition).sqrMagnitude <= sqrDistance)
                    {
                        return;
                    }
                }
                selectedPrefab = Random.Range(0, prefabs.Length);
                if (selectedPrefab == 2)
                {
                    GameObject test = Instantiate(prefabs[selectedPrefab], spawnData[0], Quaternion.Euler(-90, 0, 0));
                }
                else
                {
                    GameObject go = Instantiate(prefabs[selectedPrefab], spawnData[0], Quaternion.Euler(0, 0, 0)); //Random.Range(0, 360)
                }
                //, spawnData[1]); //Quaternion.FromToRotation(prefabs[selectedPrefab].transform.up
                //go.name += " _instantiated";
                //GameObject rockEffectClone = Instantiate (rockEffect, transform.position, Quaternion.identity) as GameObject;
                previousPosition = spawnData[0];
            }
        }
    }

    Vector3[] GetClickPositionAndNormal()
    {
        Vector3[] returnData = new Vector3[] { Vector3.zero, Vector3.zero }; //0 = spawn poisiton, 1 = surface normal
        Ray ray = c.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, LayerMask.GetMask("RockDetector")))
        {
            if (hit.collider.CompareTag("Detector"))
            {
                returnData[0] = hit.point;
                returnData[1] = hit.normal;
            }
        }

        return returnData;
    }

    Transform GetObjectOnClick()
    {
        Ray ray = c.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Transform root = hit.transform.root;
            if (root.name.EndsWith("_instantiated"))
            {
                return root;
            }
        }

        return null;
    }

    void OnGUI()
    {
        if (prefabs.Length > 0 && selectedPrefab >= 0 && selectedPrefab < prefabs.Length)
        {
            string prefabName = prefabs[selectedPrefab].name;
            GUI.color = new Color(0, 0, 0, 0.84f);
            GUI.Label(new Rect(5 + 1, 5 + 1, 200, 25), prefabName);
            GUI.color = Color.green;
            GUI.Label(new Rect(5, 5, 200, 25), prefabName);
        }
    }
}
