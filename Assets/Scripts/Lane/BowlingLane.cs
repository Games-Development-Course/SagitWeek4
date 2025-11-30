// ==================== BowlingLane.cs ====================
using UnityEngine;

public class BowlingLane : MonoBehaviour
{
    public Transform ballSpawnPoint;
    public GameObject ballPrefab;

    private GameObject currentBall;
    public GameObject CurrentBall => currentBall;

    public GameObject SpawnBall()
    {
        if (currentBall != null)
            Destroy(currentBall);

        currentBall = Object.Instantiate(
            ballPrefab,
            ballSpawnPoint.position,
            ballSpawnPoint.rotation
        );

        BallController bc = currentBall.GetComponent<BallController>();
        bc.Init(ballSpawnPoint);

        return currentBall;
    }

    public void ResetBall()
    {
        if (currentBall == null)
            return;

        BallController bc = currentBall.GetComponent<BallController>();
        bc.PrepareForIdle();

        currentBall.transform.position = ballSpawnPoint.position;
        currentBall.transform.rotation = ballSpawnPoint.rotation;
    }
}
