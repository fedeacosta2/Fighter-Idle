using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUnScale : MonoBehaviour
{
    // Set the time scale for this GameObject
    private float independentTimeScale = 1.0f;
    public bool startIndependentTime = false;

    // Update is called once per frame
    void Update()
    {
        if(startIndependentTime){
        // Use unscaledDeltaTime to ensure this object is not affected by Time.timeScale
        float deltaTime = Time.unscaledDeltaTime * independentTimeScale;

        // Update your GameObject using deltaTime
        transform.Translate(Vector3.forward * deltaTime);
        }
    }
    public void independentTime()
    {
        startIndependentTime = true;
    }
    
    public void independentTimefalse()
    {
        startIndependentTime = false;
    }
    
    
}


