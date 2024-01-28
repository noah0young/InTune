using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAdder : MonoBehaviour
{
    [SerializeField] private Transform maxPos;
    [SerializeField] private Transform minPos;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Transform parent;

    public void AddRandomTarget()
    {
        Vector2 max = maxPos.position;
        Vector2 min = minPos.position;
        Vector3 chosenPos = new Vector3(Random.Range(max.x, min.x), Random.Range(max.y, min.y), 0);
        GameObject newTarget = Instantiate(targetPrefab, parent);
        newTarget.transform.position = chosenPos;
    }
}
