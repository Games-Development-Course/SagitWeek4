// ==================== BallController.cs ====================
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Throw Settings")]
    public float minForce = 5f;
    public float maxForce = 20f;
    public float chargeSpeed = 20f;

    [Header("Auto Stop Settings")]
    public float stopAfterSeconds = 8f; // זמן עד שהכדור נעצר בכוח

    private float currentForce;
    private bool isCharging = false;
    private bool isLaunched = false;

    private Rigidbody rb;
    private Transform laneForwardPoint;
    private Camera cam;

    private Vector3 throwDirection;
    private float autoStopTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    public void Init(Transform forwardTarget)
    {
        laneForwardPoint = forwardTarget;
        transform.position = forwardTarget.position;
        transform.rotation = forwardTarget.rotation;
        PrepareForIdle();
    }

    private void Update()
    {
        // אם הכדור כבר נזרק – רק סופרים זמן
        if (isLaunched)
        {
            autoStopTimer += Time.deltaTime;
            return;
        }

        // לפני הזריקה – רק כשהוא קינמטי
        if (!rb.isKinematic)
            return;

        UpdateAiming();
        UpdateCharging();
    }

    // ---------------- AIMING ----------------
    private void UpdateAiming()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f))
        {
            Vector3 dir = hit.point - transform.position;
            dir.y = 0;
            throwDirection = dir.normalized;
        }
        else
        {
            throwDirection = laneForwardPoint.forward;
        }
    }

    // --------------- CHARGING & THROW --------
    private void UpdateCharging()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            currentForce = minForce;
        }

        if (Input.GetKey(KeyCode.Space) && isCharging)
        {
            currentForce += chargeSpeed * Time.deltaTime;
            currentForce = Mathf.Clamp(currentForce, minForce, maxForce);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            Launch();
        }
    }

    private void Launch()
    {
        isCharging = false;
        isLaunched = true;
        autoStopTimer = 0f;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(throwDirection * currentForce, ForceMode.Impulse);

        GameManager.Instance.OnBallLaunched();
    }

    // --------------- IDLE --------------------
    public void PrepareForIdle()
    {
        isLaunched = false;
        isCharging = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        autoStopTimer = 0f;
    }

    // --------------- AUTO STOP --------------
    public bool ShouldForceStop()
    {
        return isLaunched && autoStopTimer >= stopAfterSeconds;
    }
}
