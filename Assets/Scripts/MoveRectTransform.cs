using UnityEngine;
using System.Collections;

public class MoveRectTransform : MonoBehaviour
{
    [Tooltip("Reference to the black RectTransform")]
    public RectTransform blackRectTransform;
    [Tooltip("Step size for each movement")]
    public float stepSize = 10f;
    [Tooltip("Number of steps to move")]
    public int numberOfSteps = 5;
    [Tooltip("Time between each step")]
    public float stepTime = 0.5f;

    private IEnumerator MoveDown()
    {
        for (int i = 0; i < numberOfSteps; i++)
        {
            blackRectTransform.anchoredPosition -= new Vector2(0, stepSize);
            yield return new WaitForSeconds(stepTime);
        }
    }

    private void Start()
    {
        StartCoroutine(MoveDown());
    }
}
