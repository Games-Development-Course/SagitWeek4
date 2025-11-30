using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BowlingLane lane;
    public LevelManager levelManager;

    private BallController ball;
    private bool ballLaunched = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartLevel();
    }

    // ================= START LEVEL =================
    public void StartLevel()
    {
        levelManager.LoadLevel();
        UIManager.Instance.ShowMessage(
            "Aim with the mouse.\nHold SPACE to charge power.\nRelease to roll the ball!"
        );

        GameObject b = lane.SpawnBall();
        ball = b.GetComponent<BallController>();

        ballLaunched = false;
    }

    // ================= BALL LAUNCHED ================
    public void OnBallLaunched()
    {
        ballLaunched = true;
        levelManager.RegisterThrow();
    }

    // ================= UPDATE ======================
    void Update()
    {
        if (!ballLaunched)
            return;

        if (ball.ShouldForceStop())
        {
            EndThrow();
        }
    }

    // ================= END THROW ====================
    private void EndThrow()
    {
        ballLaunched = false;

        int fallen = levelManager.bottleManager.CountFallen();
        UIManager.Instance.SetScore(fallen);
        levelManager.bottleManager.RemoveFallen();

        bool cleared = levelManager.bottleManager.AllCleared();

        // ====== CLEAR ALL PINS ======
        if (cleared)
        {
            // Final level completed
            if (levelManager.IsLastLevel())
            {
                UIManager.Instance.ShowWin();
                return;
            }

            // Go to next level
            levelManager.AdvanceLevel();
            StartLevel();
            return;
        }

        // ====== NOT CLEARED ALL PINS ======
        if (levelManager.HasMoreShots())
        {
            // throw again
            lane.ResetBall();
        }
        else
        {
            // restart same level
            StartLevel();
        }
    }
}
