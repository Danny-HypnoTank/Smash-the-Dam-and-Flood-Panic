using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeByTime : MonoBehaviour
{
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private float fadeSpeed;
    private bool fadeOut = false;


    private void Start()
    {
        StartCoroutine(LifeCycle());
    }

    private void Update()
    {
        if (fadeOut == true)
        {
            Color objectColor = this.GetComponent<SpriteRenderer>().color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            this.GetComponent<SpriteRenderer>().color = objectColor;

            if (objectColor.a <= 0)
            {
                fadeOut = false;
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(lifeTime);
        fadeOut = true;
    }
}
