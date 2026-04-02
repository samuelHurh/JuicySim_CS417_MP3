using UnityEngine;

public class RefinedOreMachineFeedback : MonoBehaviour
{
    [SerializeField] private ParticleSystem myPS;
    public void FireParticles()
    {
        myPS.Play();
    }
}
