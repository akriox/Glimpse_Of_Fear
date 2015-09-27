using UnityEngine;
using System.Collections;

public class ProjectorObject : MonoBehaviour
{

    private static Light _light;
    private static float _initIntensity;
    private static float _flickerFrequency = 0.1f;

    public void Start()
    {
        _light = GetComponentInChildren<Light>();
        _initIntensity = _light.intensity;
    }


    public static IEnumerator Flicker(int times)
    {
        int count = 0;
        while (count < times)
        {
            count += 1;
            _light.intensity -= Random.value * _initIntensity;
            print(_light.intensity);
            yield return new WaitForSeconds(_flickerFrequency);
            _light.intensity = _initIntensity;
        }
       _light.intensity = 0.0f;
    }
}
