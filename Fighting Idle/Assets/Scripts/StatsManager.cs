using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatsManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    public Slider speedStat;
    public TextMeshProUGUI speedCurrentLevel;

    public float speedValue;
    public int upgradesPointsAvailable; // New field to keep track of available upgrade points

    

    // Start is called before the first frame update
    void Start()
    {
        speedValue = speedStat.value;
        speedCurrentLevel.text = "1";
        
    }

    public void UpgradeSpeed()
    {
        // Check if there are available upgrade points
        if (upgradesPointsAvailable > 0)
        {
            // Upgrade speed
            speedValue += 0.1f;
            speedStat.value = speedValue;

            // Decrease available upgrade points by 1
            upgradesPointsAvailable--;
            player.UpgradesPointsAvaible.text = upgradesPointsAvailable.ToString();

            // Update UI to reflect the changes
            speedCurrentLevel.text = (int.Parse(speedCurrentLevel.text) + 1).ToString();
            
            
        }
        else
        {
            // If there are no available upgrade points, do nothing or display a message to the player
            Debug.Log("No available upgrade points.");
        }
    }
}
