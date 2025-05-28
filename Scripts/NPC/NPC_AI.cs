
using System.Collections;
using UnityEngine;

public class NPC_AI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    private enum State
    {
        Roaming,
        Stopped
    }

    private State state;
    private NPCPathFinding npcPathFinding;
    private Vector2 lastDirection;

    private void Awake()
    {
        npcPathFinding = GetComponent<NPCPathFinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        StartCoroutine(RoamingRoutine());
    }

    private IEnumerator RoamingRoutine()
    {
        while (true)
        {
            if (state == State.Roaming)
            {
                // Get random position to roam to
                Vector2 roamPosition = GetRoamingPosition();
                npcPathFinding.MoveTo(roamPosition);  // Move NPC to random position
                yield return new WaitForSeconds(roamChangeDirFloat);
                lastDirection = roamPosition;  // Update the last direction moved
                yield return new WaitForSeconds(Random.Range(3f, 5f));  // Roaming duration
            }

            if (state == State.Stopped)
            {
                // Stop movement and update the last direction
                npcPathFinding.UpdateLastDirection(lastDirection);
                npcPathFinding.MoveTo(Vector2.zero);  // Stop movement, no movement
                yield return new WaitForSeconds(Random.Range(5f, 7f));  // Stop duration, stay idle longer

                // Transition back to roaming after the stop duration
                state = State.Roaming;
            }

            // Random chance to stop and roam again after a delay
            if (Random.Range(0f, 1f) > 0.7f)
            {
                state = State.Stopped;  // Switch to stopped state
            }
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}