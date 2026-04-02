using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SmoothSnapping : MonoBehaviour
{
    [Header("Easing Settings")]
    [Tooltip("The 'k' constant from the formula. Higher = faster snapping.")]
    public float k = 8.0f;

    private XRGrabInteractable grabInteractable;
    private bool isBeingSnapped = false;
    private Transform targetSocketTransform;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Triggered when the generator is placed into a socket
        grabInteractable.selectEntered.AddListener(OnSelectedBySocket);

        // Triggered when the generator is pulled out of the socket
        grabInteractable.selectExited.AddListener(OnExitedSocket);
    }

    private void OnSelectedBySocket(SelectEnterEventArgs args)
    {
        // check if the interactor's GameObject has the XRSocketInteractor component
        if (args.interactorObject is IXRInteractor interactor &&
            interactor.transform.GetComponent<XRSocketInteractor>() != null)
        {
            targetSocketTransform = interactor.transform;
            isBeingSnapped = true;

            grabInteractable.trackPosition = false;
            grabInteractable.trackRotation = false;
        }
    }

    private void OnExitedSocket(SelectExitEventArgs args)
    {
        isBeingSnapped = false;
        grabInteractable.trackPosition = true;
        grabInteractable.trackRotation = true;
    }

    void Update()
    {
        if (isBeingSnapped && targetSocketTransform != null)
        {
            // 1. Position Easing
            // x = current position, goal = socket position
            Vector3 positionGoal = targetSocketTransform.position;
            Vector3 currentPos = transform.position;

            // v = k * (goal - x)
            Vector3 posVelocity = k * (positionGoal - currentPos) * Time.deltaTime;
            transform.position += posVelocity;

            // 2. Rotation Easing
            transform.rotation = Quaternion.Slerp(transform.rotation, targetSocketTransform.rotation, k * Time.deltaTime);

            // 3. Final Snap
            if (Vector3.Distance(transform.position, positionGoal) < 0.001f)
            {
                transform.position = positionGoal;
                transform.rotation = targetSocketTransform.rotation;
                isBeingSnapped = false;
            }
        }
    }
}
