using UnityEngine;
using System.Collections;

public class RobotFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Bones")]
    public Transform headBone;
    public Transform eyeBone;

    [Header("Blink Settings")]
    public float blinkDuration = 0.12f;
    public float minBlinkInterval = 2f;
    public float maxBlinkInterval = 5f;

    [Header("Surprised Eye")]
    public SkinnedMeshRenderer eyeRenderer;
    public Material normalEyeMaterial;
    public Material surprisedEyeMaterial;
    public float surprisedScale = 1.5f;

    private Vector3 originalEyeScale;

    [Header("RotationOffset")]
    public Vector3 rotationOffset = new Vector3(0, 0, 0);

    void Start()
    {
        originalEyeScale = eyeBone.localScale;
        StartCoroutine(BlinkLoop());

    }
    void LateUpdate()
    {
        if (target != null)
        {
            headBone.LookAt(target);

            headBone.Rotate(rotationOffset);
        }
    }
    IEnumerator BlinkEye(Transform eyeBone, float duration = 0.1f)
    {
        float half = duration * 0.5f;
        Vector3 originalScale = eyeBone.localScale;

        // close (1 ? 0.1)
        float t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float y = Mathf.Lerp(1f, 0.1f, t / half);
            eyeBone.localScale = new Vector3(originalScale.x, y, originalScale.z);
            yield return null;
        }

        // open (0.1 ? 1)
        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float y = Mathf.Lerp(0.1f, 1f, t / half);
            eyeBone.localScale = new Vector3(originalScale.x, y, originalScale.z);
            yield return null;
        }

        // reset (safety)
        eyeBone.localScale = originalScale;
    }

    IEnumerator BlinkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minBlinkInterval, maxBlinkInterval));
            StartCoroutine(BlinkEye(eyeBone, blinkDuration));
        }
    }

    public void SetSurprised(bool isSurprised)
    {
        if (eyeBone == null || eyeRenderer == null) return;

        // scale eye bone
        eyeBone.localScale = isSurprised
            ? originalEyeScale * surprisedScale
            : originalEyeScale;

        // replace material at element 1
        Material[] mats = eyeRenderer.materials;

        if (mats.Length > 1)
        {
            mats[1] = isSurprised ? surprisedEyeMaterial : normalEyeMaterial;
            eyeRenderer.materials = mats;
        }
    }

}
