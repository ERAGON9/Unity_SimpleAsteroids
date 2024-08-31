using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : Singleton<WarpManager>
{
    [SerializeField] private Camera m_Camera;


    public void KeepInBounds(Transform i_keptTransform)
    {
        var screenPoint = m_Camera.WorldToViewportPoint(i_keptTransform.position);
        var outOfBounds = false;

        if (screenPoint.x < 0)
        {
            screenPoint.x = 1;
            outOfBounds = true;
        }

        if (screenPoint.x > 1)
        {
            screenPoint.x = 0;
            outOfBounds = true;
        }

        if (screenPoint.y < 0)
        {
            screenPoint.y = 1;
            outOfBounds = true;
        }

        if (screenPoint.y > 1)
        {
            screenPoint.y = 0;
            outOfBounds = true;
        }

        if (!outOfBounds)
            return;

        var updatedWordPosition = m_Camera.ViewportToWorldPoint(screenPoint);
        i_keptTransform.position = updatedWordPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
