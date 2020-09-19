using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickEggController : MonoBehaviour
{
    [Tooltip("The temperature of the environment the egg is [degC]")]
    public float environmentTemperature = 22.0f;

    [Tooltip("The body temperature of the parent roosting [degC]")]
    public float roostTemperature = 40.0f;

    [Tooltip("The nominal temperature the eggs must be kept at.  Also "
             + "serves as the initial temperature [degC]")]
    public float incubationTemperature = 37.5f;

    [Tooltip("How well heat transfers from environment to egg "
             + "[W / (m^2 degC)]")]
    public float heatTransferCoefficient = 25.0f;

    [Tooltip("How well the egg holds onto heat [J / (kg degC)]")]
    public float specificHeat = 3180;

    [Tooltip("When an egg reaches this temperature, it dies [degC]")]
    public float minEggTemperature = 22.1f;

    private bool isBeingRoosted;
    private float currentTemperature;

    void Start()
    {
        isBeingRoosted = false;
        currentTemperature = incubationTemperature;
    }

    void FixedUpdate()
    {
        UpdateEggTemperature();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "WarmBody")
        {
            isBeingRoosted = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "WarmBody")
        {
            isBeingRoosted = false;
        }
    }

    private void UpdateEggTemperature()
    {
        if (isBeingRoosted)
        {
            currentTemperature = NextEggTemperature(currentTemperature,
                                                    roostTemperature);
        }
        else
        {
            currentTemperature = NextEggTemperature(currentTemperature,
                                                    environmentTemperature);
        }

        if (currentTemperature < minEggTemperature)
        {
            Die();
        }

        if (currentTemperature >= incubationTemperature)
        {
            Debug.Log("Incubation temperature reached!");
        }
    }

    private float NextEggTemperature(float eggTemperature,
                                     float contactingTemperature)
    {
        float heatTransferRate = heatTransferCoefficient
            * (contactingTemperature - eggTemperature);
        return currentTemperature
            + heatTransferRate * Time.fixedDeltaTime / specificHeat;
    }

    private void Die()
    {
        Debug.Log("Too cold! I must die.");
        Destroy(gameObject);
    }
}
