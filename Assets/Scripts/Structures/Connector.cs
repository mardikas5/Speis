using System;
using System.Collections.Generic;
using UnityEngine;


public class Connector : MonoBehaviour
{
    public Structure Owner;

    public Connector Connected;
    
    public Transform ConnectionPoint;

    private void Start()
    {
        ConnectorEnd t = ConnectionPoint.gameObject.AddComponent<ConnectorEnd>();
        t.connector = this;
    }
}