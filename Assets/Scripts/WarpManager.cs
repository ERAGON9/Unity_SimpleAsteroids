using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : Singleton<WarpManager>
{
    [SerializeField] private Camera m_Camera;

    private List<Transform> m_Trasnforms = new();

    public void SubscribeTransform(Transform i_AddedTransform)
    {
        m_Trasnforms.Add(i_AddedTransform);
    }
    
    public void UnsubscribeTransform(Transform i_RemovedTransform)
    {
        m_Trasnforms.Remove(i_RemovedTransform);
    }
    
    // Update is called once per frame
    private void Update()
    {
        foreach (var keptTransform in m_Trasnforms)
        {
            if (keptTransform != null)
            {
                KeepInBounds(keptTransform);
            }
        }   
    }
    
    public void KeepInBounds(Transform i_keptTransform)
    {
        var screenPoint = m_Camera.WorldToViewportPoint(i_keptTransform.position);
        var outOfBounds = false;

        switch (screenPoint.x)
        {
            case < 0:
                screenPoint.x = 1;
                outOfBounds = true;
                break;
            case > 1:
                screenPoint.x = 0;
                outOfBounds = true;
                break;
        }
        
        switch (screenPoint.y)
        {
            case < 0:
                screenPoint.y = 1;
                outOfBounds = true;
                break;
            case > 1:
                screenPoint.y = 0;
                outOfBounds = true;
                break;
        }

        if (!outOfBounds)
        {
            return;
        }

        var updatedWordPosition = m_Camera.ViewportToWorldPoint(screenPoint);
        i_keptTransform.position = updatedWordPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
