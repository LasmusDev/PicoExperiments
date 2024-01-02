using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class DebugMoveBackAndForth : MonoBehaviour
{
    [Range(0.1f, 2f)]
    public float speed;
    public bool forward;
    public float distanceTravelled;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Walk());
    }

   
    public IEnumerator Walk()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (forward)
            {
                this.transform.position += this.transform.forward * Mathf.Sin(Time.deltaTime) * speed;
            }
            else
            {
                this.transform.position -= this.transform.forward * Mathf.Sin(Time.deltaTime) * speed;
            }
            distanceTravelled += Mathf.Sin(Time.deltaTime) * speed;
            if (distanceTravelled > 8)
            {
                forward = !forward;
                distanceTravelled = 0;
            }
            yield return null;
        }
    }
}
