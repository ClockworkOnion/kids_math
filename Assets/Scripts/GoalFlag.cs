using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component on the goal flag, when it collides with the player car,
/// triggers the end of the round
/// </summary>
public class GoalFlag : MonoBehaviour
{
    public StageManager stageManager;
    private void OnTriggerEnter(Collider other)
    {
        stageManager?.EndRound();
    }
}
