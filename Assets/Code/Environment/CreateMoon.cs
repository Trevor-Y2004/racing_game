using UnityEngine;

public class CreateMoon : MonoBehaviour
{
    public Material moonMaterial;

    [Header("Moon Settings")]
    public Vector3 moonPosition = new Vector3(0, 80, 90);
    public float moonSize = 8f;

    [Header("Light Settings")]
    public float lightIntensity = 0.35f;
    public Color lightColor = new Color(0.65f, 0.7f, 1f);
    public Vector3 lightRotation = new Vector3(50f, -30f, 0f);

    private GameObject moon;
    private GameObject lightObject;
    private Light moonLight;

    void Start()
    {
        moon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        moon.name = "Moon";

        if (moonMaterial != null)
            moon.GetComponent<Renderer>().material = moonMaterial;

        lightObject = new GameObject("Moon Light");
        moonLight = lightObject.AddComponent<Light>();
        moonLight.type = LightType.Directional;

        ApplySettings();
    }

    void Update()
    {
        ApplySettings();
    }

    void ApplySettings()
    {
        if (moon != null)
        {
            moon.transform.position = moonPosition;
            moon.transform.localScale = Vector3.one * moonSize;
        }

        if (moonLight != null)
        {
            moonLight.intensity = lightIntensity;
            moonLight.color = lightColor;
        }

        if (lightObject != null)
        {
            lightObject.transform.rotation = Quaternion.Euler(lightRotation);
        }
    }
}