using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dam : MonoBehaviour
{
    [SerializeField]
    private int damHealth;
    [SerializeField]
    private int ability1dmg;
    [SerializeField]
    private int ability2dmg;

    //ProgressBar
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private GameObject[] ParticleEffects;
    [SerializeField]
    private GameObject[] meteors;
    [SerializeField]
    private GameObject[] dynamite;
    private int currentDynamite = 0;
    private int currentMeteor = 0;
    [SerializeField]
    private GameObject[] rockParticles;
    [SerializeField]
    private GameObject[] damStages;
    [SerializeField]
    public Queue<GameObject> damageObjects = new Queue<GameObject>();
    [SerializeField]
    private List<GameObject> currentObjects = new List<GameObject>();
    [SerializeField]
    private CameraShake cam;


    //Game Manager
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject[] fistIcon;
    [SerializeField]
    private GameObject tapParticle;
    private int tapCounter;

    private bool noHealth = false;
    public bool NoHealth { get => noHealth; set => noHealth = value; }
    public Vector3 WorldPos { get => worldPos; set => worldPos = value; }

    public GameObject handAnim;
    public GameObject animLocation;
    private int handAnimCheck = 0;
    [SerializeField]
    public Queue<int> healthTriggers = new Queue<int>(new [] { 499, 489, 479, 459 });//{400, 300, 200, 100, 0}); //{499, 489, 479, 459});
    public GameObject healthBar;
    public float healthBarAnimTime;
    public float healthBarAnimSpeed;
    public Vector3 healthBarStartSize;
    public Vector3 healthBarEndSize;
    private bool healthBarAnimating = false;

    [SerializeField]
    private Vector3 worldPos;

    //Need an int to change icon as player hits Dam

    private void Start()
    {
        tapCounter = 0;
        //handAnimCheck = 0;
        cam = FindObjectOfType<CameraShake>();
        for(int i = 0; i < currentObjects.Count; i++)
        {
            damageObjects.Enqueue(currentObjects[i]);
        }
    }

    private void OnMouseDown()
    {
        if (gameManager.Win == false)
        {
            worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,4.5f));
            Debug.Log(Input.mousePosition);
            DamageDam();

            //tapCounter++;
            //IconSpawn();
        }
    }

    private void DamageDam()
    {
        if (gameManager.AbilityType == 1)
        {
            meteors[currentMeteor].SetActive(true);
            currentMeteor++;
            StartCoroutine(DamageTimer());
            //damHealth -= ability1dmg;
        }
        else if (gameManager.AbilityType == 2)
        {
            damHealth -= ability2dmg;
        }
        else if (gameManager.AbilityType == 3)
        {
            dynamite[currentDynamite].SetActive(true);
            currentDynamite++;
        }
        else
        {

        }
        if(healthTriggers.Count > 0 && gameManager.AbilityType == 2)
        {
            if(damHealth <= healthTriggers.Peek())
            {
                healthTriggers.Dequeue();
                MoveNextObj();
                cam.StartShake();
                StartCoroutine(HealthBarAnimate());
            }
        }
    }

    private void MoveNextObj()
    {
        if(damageObjects.Count != 0)
        {
            GameObject o = damageObjects.Dequeue();
            o.GetComponent<Rigidbody>().AddForce(new Vector3(0, Random.Range(20, 50), Random.Range(20, 50)), ForceMode.Impulse);
            o.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(30, 0, 0), ForceMode.Impulse);
            //o.transform.Rotate(0,0)
        }
    }
    private IEnumerator HealthBarAnimate()
    {
        healthBarAnimating = true;
        bool inverse = false;
        float t = 0;
        float accumalated = 0;
        while(healthBarAnimating)
        {
            accumalated += Time.deltaTime + 0.1f;
            if(accumalated > healthBarAnimTime) {healthBarAnimating = false;}
            t = accumalated * healthBarAnimTime;
            while(t > 1)
            {
                t -= 1;
                inverse = !inverse;
            }
            healthBar.transform.localScale = inverse ? Vector3.Lerp(healthBarEndSize, healthBarStartSize, t) : 
                                                        Vector3.Lerp(healthBarStartSize, healthBarEndSize, t);
            yield return new WaitForSeconds(0.1f);
        }
        healthBar.transform.localScale = healthBarStartSize;
    }

    private void ManageDespawns()
    {
        for(int i = 0; i < currentObjects.Count; i++)
        {
            if(currentObjects[i].transform.position.y < 0)
            {
                Destroy(currentObjects[i]);
                currentObjects.RemoveAt(i);
                break;
            }
        }
    }

    private void Update()
    {
        DamTextureChanging();
        healthSlider.value = damHealth;
        if (damHealth <= 0)
        {
            gameManager.Win = true;
            Debug.Log("Win");
            noHealth = true;
        }
    }

    private void DamTextureChanging()
    {
        if (gameManager.AbilityType == 1)
        {
            if (damHealth == 400) // 1 crack
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(true);
                damStages[2].SetActive(false);
                damStages[3].SetActive(false);
                damStages[4].SetActive(false);
                //Enable Rock particle for dam crack - single crack
                rockParticles[0].SetActive(true);
                //Enable water particles
                ParticleEffects[0].SetActive(true);
            }
            if (damHealth == 300) // 2 cracks
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(true);
                damStages[3].SetActive(false);
                damStages[4].SetActive(false);
                //Enable Rock particle for dam crack - single crack - second crack
                rockParticles[1].SetActive(true);
                //Enable water particles
                ParticleEffects[1].SetActive(true);
                ParticleEffects[2].SetActive(true);

            }
            if (damHealth == 200) // 3 cracks
            {
                //3rd stage of dam breaking before big stage
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(false);
                damStages[3].SetActive(true);
                damStages[4].SetActive(false);
                //Enable Rock particle for dam crack - single crack - second crack
                rockParticles[2].SetActive(true);
                //Enable water particles
                ParticleEffects[3].SetActive(true);
                ParticleEffects[4].SetActive(true);
            }
            if (damHealth == 100) // lots of cracks
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(false);
                damStages[3].SetActive(false);
                damStages[4].SetActive(true);
                //Enable Rock particle for dam cracks
                rockParticles[3].SetActive(true);
                rockParticles[4].SetActive(true);
                rockParticles[5].SetActive(true);
                rockParticles[6].SetActive(true);
                //Enable Water Particles
                ParticleEffects[5].SetActive(true);
                ParticleEffects[6].SetActive(true);
                ParticleEffects[7].SetActive(true);
                ParticleEffects[8].SetActive(true);
            }
            if (damHealth == 0) // fully broken dam
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(false);
                damStages[3].SetActive(false);
                damStages[4].SetActive(false);
                damStages[5].SetActive(true);               
                //Enable Water Particles
                ParticleEffects[9].SetActive(true);
                ParticleEffects[10].SetActive(true);
                ParticleEffects[11].SetActive(true);
                StartCoroutine(DisableTimer());
            }
        }

        //Dam breaking for tapping
        if (gameManager.AbilityType == 2)
        {
            if (damHealth == 499)
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(true);
                damStages[2].SetActive(false);
                damStages[3].SetActive(false);
                damStages[4].SetActive(false);
                //Enable Rock particle for dam crack - single crack
                rockParticles[0].SetActive(true);
                //Enable water particles
                ParticleEffects[0].SetActive(true);
            }
            else if (damHealth == 489)
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(true);
                damStages[3].SetActive(false);
                damStages[4].SetActive(false);
                //Enable Rock particle for dam crack - single crack - second crack
                rockParticles[1].SetActive(true);
                //Enable water particles
                ParticleEffects[1].SetActive(true);
                ParticleEffects[2].SetActive(true);

            }
            else if (damHealth == 479)
            {
                //3rd stage of dam breaking before big stage
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(false);
                damStages[3].SetActive(true);
                damStages[4].SetActive(false);
                //Enable Rock particle for dam crack - single crack - second crack
                rockParticles[2].SetActive(true);
                //Enable water particles
                ParticleEffects[3].SetActive(true);
                ParticleEffects[4].SetActive(true);
            }
            else if (damHealth == 459) //&& handAnimCheck == 0)
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(false);
                damStages[3].SetActive(false);
                damStages[4].SetActive(true);
                //Enable Rock particle for dam cracks
                rockParticles[3].SetActive(true);
                rockParticles[4].SetActive(true);
                rockParticles[5].SetActive(true);
                rockParticles[6].SetActive(true);
                //Enable Water Particles
                ParticleEffects[5].SetActive(true);
                ParticleEffects[6].SetActive(true);
                ParticleEffects[7].SetActive(true);
                ParticleEffects[8].SetActive(true);
            }
        }

        if (gameManager.AbilityType == 3)
        {
            if (damHealth == 400) // 1 crack
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(true);
                damStages[2].SetActive(false);
                damStages[3].SetActive(false);
                damStages[4].SetActive(false);
                //Enable Rock particle for dam crack - single crack
                rockParticles[0].SetActive(true);
                //Enable water particles
                ParticleEffects[0].SetActive(true);
            }
            if (damHealth == 300) // 2 cracks
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(true);
                damStages[3].SetActive(false);
                damStages[4].SetActive(false);
                //Enable Rock particle for dam crack - single crack - second crack
                rockParticles[1].SetActive(true);
                //Enable water particles
                ParticleEffects[1].SetActive(true);
                ParticleEffects[2].SetActive(true);

            }
            if (damHealth == 200) // 3 cracks
            {
                //3rd stage of dam breaking before big stage
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(false);
                damStages[3].SetActive(true);
                damStages[4].SetActive(false);
                //Enable Rock particle for dam crack - single crack - second crack
                rockParticles[2].SetActive(true);
                //Enable water particles
                ParticleEffects[3].SetActive(true);
                ParticleEffects[4].SetActive(true);
            }
            if (damHealth == 100) // lots of cracks
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(false);
                damStages[3].SetActive(false);
                damStages[4].SetActive(true);
                //Enable Rock particle for dam cracks
                rockParticles[3].SetActive(true);
                rockParticles[4].SetActive(true);
                rockParticles[5].SetActive(true);
                rockParticles[6].SetActive(true);
                //Enable Water Particles
                ParticleEffects[5].SetActive(true);
                ParticleEffects[6].SetActive(true);
                ParticleEffects[7].SetActive(true);
                ParticleEffects[8].SetActive(true);
            }
            if (damHealth == 0) // fully broken dam
            {
                damStages[0].SetActive(false);
                damStages[1].SetActive(false);
                damStages[2].SetActive(false);
                damStages[3].SetActive(false);
                damStages[4].SetActive(false);
                damStages[5].SetActive(true);
                //Enable Water Particles
                ParticleEffects[9].SetActive(true);
                ParticleEffects[10].SetActive(true);
                ParticleEffects[11].SetActive(true);
                StartCoroutine(DisableTimer());
            }
        }
    }

    private void IconSpawn()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.0f;       // we want 2m away from the camera position
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        //if (tapCounter <= 10)
        //{
        //    //Instantiate(fistIcon[0], objectPos, Quaternion.identity);
        //    //Instantiate(tapParticle, objectPos, Quaternion.identity);
        //}
        //else if (tapCounter > 10 && tapCounter <= 20)
        //{
        //    //Instantiate(fistIcon[1], objectPos, Quaternion.identity);
        //    //Instantiate(tapParticle, objectPos, Quaternion.identity);
        //}
        //else if (tapCounter > 20 && tapCounter <= 30)
        //{
        //    //Instantiate(fistIcon[2], objectPos, Quaternion.identity);
        //    //Instantiate(tapParticle, objectPos, Quaternion.identity);
        //}
        //else if (tapCounter > 30 && tapCounter <= 40)
        //{
        //    //Instantiate(fistIcon[3], objectPos, Quaternion.identity);
        //    //Instantiate(tapParticle, objectPos, Quaternion.identity);
        //}
        //else if (tapCounter > 40 && tapCounter <= 50)
        //{
        //    //Instantiate(fistIcon[4], objectPos, Quaternion.identity);
        //    //Instantiate(tapParticle, objectPos, Quaternion.identity);
        //}
        //else if (tapCounter > 50)
        //{
        //    //Instantiate(fistIcon[4], objectPos, Quaternion.identity);
        //    //Instantiate(tapParticle, objectPos, Quaternion.identity);
        //}
    }

    private IEnumerator DamageTimer()
    {
        yield return new WaitForSeconds(0.7f);
        damHealth -= ability1dmg;
        if (healthTriggers.Count > 0)
        {
            if (damHealth <= healthTriggers.Peek())
            {
                healthTriggers.Dequeue();
                MoveNextObj();
                cam.StartShake();
                StartCoroutine(HealthBarAnimate());
            }
        }
    }

    private IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dynamite"))
        {
            damHealth -= ability1dmg;
            if (healthTriggers.Count > 0)
            {
                if (damHealth <= healthTriggers.Peek())
                {
                    healthTriggers.Dequeue();
                    MoveNextObj();
                    cam.StartShake();
                    StartCoroutine(HealthBarAnimate());
                }
            }
        }
    }
}