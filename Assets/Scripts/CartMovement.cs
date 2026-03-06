using UnityEngine;
using UnityEngine.Splines;

public class CartMovement : MonoBehaviour
{
    [Header("Spline")]
    public SplineContainer spline;
    [Min(64)] public int samplesPerLoop = 2000; // increase if you see tiny speed wobble
    public bool closedLoop = true;

    [Header("Motion")]
    public float speedMps = 2.5f;     // meters/second
    public float startDistance = 0f;  // meters along the loop

    [Header("Orientation")]
    public bool faceForward = true;
    public Vector3 up = default;      // leave default to use Vector3.up

    float[] cumulative;   // cumulative distances for each sample
    float totalLength;
    float distance; 
    float speedMultiplier = 1f;
    float speedBoostEndTime = 0f;
    public GameObject boostIndicator;

    void Awake()
    {
        if (up == default) up = Vector3.up;
        RebuildTable();
        distance = startDistance;
    }

    void OnValidate()
    {
        if (Application.isPlaying) return;
        if (up == default) up = Vector3.up;
    }

    void Update()
    {

        if (!spline || cumulative == null || totalLength <= 0f) return;
 
        bool boosting = speedMultiplier != 1f && Time.time < speedBoostEndTime;
        if (boostIndicator != null)
            boostIndicator.SetActive(boosting);

        if (!boosting && speedMultiplier != 1f)
        {
            speedMultiplier = 1f;
        }
        distance += speedMps * speedMultiplier * Time.deltaTime;

        if (closedLoop)
        {
            distance %= totalLength;
            if (distance < 0) distance += totalLength;
        }
        else
        {
            distance = Mathf.Clamp(distance, 0f, totalLength);
        }

        float t = DistanceToT(distance);

        Vector3 pos = spline.EvaluatePosition(t);
        transform.position = pos;

        if (faceForward)
        {
            Vector3 tan = spline.EvaluateTangent(t);
            if (tan.sqrMagnitude > 1e-6f)
                transform.rotation = Quaternion.LookRotation(tan.normalized, up);
        }
    }

    [ContextMenu("Rebuild Distance Table")]
    public void RebuildTable()
    {
        if (!spline) return;

        cumulative = new float[samplesPerLoop + 1];
        cumulative[0] = 0f;
        totalLength = 0f;

        Vector3 prev = spline.EvaluatePosition(0f);
        for (int i = 1; i <= samplesPerLoop; i++)
        {
            float t = (float)i / samplesPerLoop;
            Vector3 p = spline.EvaluatePosition(t);
            totalLength += Vector3.Distance(prev, p);
            cumulative[i] = totalLength;
            prev = p;
        }
    }

    float DistanceToT(float d)
    {
        // Binary search cumulative distance table
        int lo = 0;
        int hi = cumulative.Length - 1;
        while (lo < hi)
        {
            int mid = (lo + hi) >> 1;
            if (cumulative[mid] < d) lo = mid + 1;
            else hi = mid;
        }

        int i = Mathf.Clamp(lo, 1, cumulative.Length - 1);
        float d0 = cumulative[i - 1];
        float d1 = cumulative[i];
        float alpha = (d1 > d0) ? (d - d0) / (d1 - d0) : 0f;

        float t0 = (float)(i - 1) / samplesPerLoop;
        float t1 = (float)i / samplesPerLoop;
        return Mathf.Lerp(t0, t1, alpha);
    }
 
        public void IncreaseSpeedTemporarily(float duration)
        {
            speedMultiplier = 2f;
            speedBoostEndTime = Time.time + duration;
        }
}