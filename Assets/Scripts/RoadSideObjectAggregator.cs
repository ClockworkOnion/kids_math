using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSideObjectAggregator : MonoBehaviour
{
    [SerializeField] private float maxZPosition = 0;

    public void SetMaxZ(Vector3 position) {
        float newZ = position.z;
        maxZPosition = newZ > maxZPosition ? newZ : maxZPosition;
    }

    public float GetMaxZ() {
        return maxZPosition;
    }
} 
