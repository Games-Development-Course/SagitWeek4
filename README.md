# Bowling2

A simple bowling game made with Unity.
[click here to play on itch](https://gamedevteamx.itch.io/sagitweek4)

## How to Run

1. Open Scens\BowlingScene
2. Hit Play
3. Hit space on keyboard

## Class Relations
```
GameManager
├─ LevelManager (loads levels, manages bottles)
├─ UIManager (displays score and messages)
└─ BowlingLane (spawns/reuses ball)
└─ BallController (detects when ball stops)

LevelManager
└─ BottleManager (tracks bottles)
└─ Bottle[] (individual bottles, checks if fallen)
```

**Key Flow:**
1. GameManager starts a level via LevelManager
2. BowlingLane spawns the ball
3. Player rolls via BallController
4. Ball stops → BallController fires OnBallStopped event
5. GameManager calls BottleManager.CountFallen()
6. UIManager displays the result

## Architecture Assumptions

- **Ball reuse:** The ball is reused between rolls (repositioned) instead of destroyed/respawned to avoid hiccups.
- **Single BottleManager per level:** Each level layout has one BottleManager that finds all Bottle children via `GetComponentsInChildren()`.
- **Stop detection:** Ball is considered stopped when it moves very little (`stationaryTime >= 0.25s` and `velocity < 0.15`) for at least 1 second after rolling.
- **Fallen criteria:** A bottle is fallen if its Y position drops below 1.35 OR it tips over (angle > 45°).
- **UI updates on stop:** Score only updates when the ball physically stops, not before.


## UML
```mermaid
classDiagram
    class GameManager {
        - levelManager: LevelManager
        - uiManager: UIManager
        - bowlingLane: BowlingLane
        - currentRoll: int
        - fallenInFirstRoll: int
        - subscribedBallController: BallController
        + StartLevel()
        + OnBallStopped()
        + LevelCompleted()
        + LevelFailed()
        - SubscribeToCurrentBall()
    }

    class LevelManager {
        - levelLayouts: GameObject[]
        - currentLayoutInstance: GameObject
        - bottleManager: BottleManager
        - CurrentLevelIndex: int
        + LoadLevel()
        + NextLevel()
        + BottleManager(): BottleManager
    }

    class BowlingLane {
        - ballPrefab: GameObject
        - ballSpawnPoint: Transform
        - currentBall: GameObject
        + SpawnBall()
        + ResetLane()
        + CurrentBall(): GameObject
    }

    class BallController {
        - rb: Rigidbody
        - rollForce: float
        - rollAction: InputAction
        - velocityThreshold: float
        - minTimeBeforeStop: float
        - hasRolled: bool
        - rollTime: float
        - lastPosition: Vector3
        - stationaryTime: float
        + OnBallStopped: Action
        + RollBall()
        + OnReset()
        - Update()
    }

    class BottleManager {
        - bottles: Bottle[]
        + CountFallen(): int
        + RemoveFallen()
        + AllCleared(): bool
        + Bottles(): Bottle[]
    }

    class Bottle {
        - FALL_ANGLE: float
        - FALL_HEIGHT: float
        + IsFallen(): bool
    }

    class UIManager {
        + UpdateLevel(levelIndex: int)
        + UpdateScore(fallen: int)
        + ShowMessage(message: string)
    }

    %% Relationships
    GameManager --> LevelManager
    GameManager --> UIManager
    GameManager --> BowlingLane
    GameManager --> BallController

    LevelManager --> BottleManager
    LevelManager --> "levelLayouts[]" GameObject

    BowlingLane --> BallController
    BowlingLane --> "ballPrefab" GameObject

    BottleManager --> "bottles[]" Bottle

    BallController --> Rigidbody
