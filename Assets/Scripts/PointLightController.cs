using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PointLightController : MonoBehaviour
{
    [SerializeField] Light2D light;
    private float outerRadius;
    private float intensity;
    [SerializeField] private float randomOffsetPercentage;
    [SerializeField] float delayInSeconds;
    private void Start()
    {
        outerRadius = light.pointLightOuterRadius;
        intensity = light.intensity;
        StartCoroutine(AnimateLight());
    }

    IEnumerator AnimateLight()
    {
        while (true)
        {
            UpdateLight();
            yield return new WaitForSeconds(delayInSeconds);
            UpdateLight();
        }
    }

    private void UpdateLight()
    {
        float offsetPercentage = Random.Range(0, randomOffsetPercentage);
        light.pointLightOuterRadius = outerRadius + outerRadius * offsetPercentage / 100;
        light.intensity = intensity + intensity * offsetPercentage / 100;
    }
}
