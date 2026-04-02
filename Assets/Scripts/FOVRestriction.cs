using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FOVRestriction : MonoBehaviour
{
    public Volume volume;
    public CharacterController playerController; // Or use a Rigidbody
    public float maxIntensity = 0.5f;
    public float speedThreshold = 5f; // Speed at which FOV starts narrowing

    private Vignette vignette;

    void Start()
    {
        // Fetch the Vignette component from the volume profile
        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
        }
    }

    void Update()
    {
        // Get current horizontal speed
        float currentSpeed = new Vector3(playerController.velocity.x, 0, playerController.velocity.z).magnitude;

        // Calculate intensity: 0 at idle, maxIntensity at speedThreshold
        float targetIntensity = Mathf.Clamp01(currentSpeed / speedThreshold) * maxIntensity;

        // Smoothly interpolate to avoid flickering
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, Time.deltaTime * 5f);
    }
}