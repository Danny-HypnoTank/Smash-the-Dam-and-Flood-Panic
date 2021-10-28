using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* Dam Breaking Steps
     * 
     * 1. Pick Ability
     * 2. Tap dam to destroy
     * 3. when dam breaks water rushes out
     * 
    */

    //Step 1 Ability Picking
    [SerializeField]
    private GameObject abilityUI;
    [SerializeField]
    private GameObject damBreakingUI;

    //Reference to Regular Dam
    //Reference to Broken Version

    private int abilityType;
    public int AbilityType { get => abilityType; set => abilityType = value; }

    private void Start()
    {
        abilityUI.SetActive(true);
        abilityType = 0;
    }

    public void Ability1Button()
    {
        abilityType = 1;
        abilityUI.SetActive(false);
        damBreakingUI.SetActive(true);
    }
    public void Ability2Button()
    {
        abilityType = 2;
        abilityUI.SetActive(false);
        damBreakingUI.SetActive(true);
    }
    public void Ability3Button()
    {
        abilityType = 3;
        abilityUI.SetActive(false);
        damBreakingUI.SetActive(true);
    }


    //Step 2 Tap Dam to Destroy
    //Refer to Dam Script

    //Step 3 Level End - Breaking of Dam
    //If win = true then give score and time it took and have water rush out of Dam - use particle effects with 3D models etc.
    private bool win = false;
    public bool Win { get => win; set => win = value; }
    private void Update()
    {
        if (win == true)
        {
            //Disable Dam Collider
            //Activate particle effects
            //Explode dam with broken version
            //Display Destroyed Text
        }
    }

    //Explosion of Dam do as an animation.
}