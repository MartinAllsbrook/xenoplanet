using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class WaypointController : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Transform targetTransform;
    
    // Update is called once per frame
    void Update()
    {
        float minX = image.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = image.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 position = Camera.main.WorldToScreenPoint(targetTransform.position);

        if (Vector3.Dot(targetTransform.position - transform.position, transform.forward) < 0)
        {
            if (position.x < Screen.width / 2)
                position.x = maxX;
            else
                position.x = minX;
        }
        
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        image.transform.position = position;
    }
}
