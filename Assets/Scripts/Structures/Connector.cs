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

    public void Connect( Connector other )
    {
        if( other.Connected == null && Connected == null )
        {
            
            this.Connected = other;
            other.Connected = this;
        }
    }

    public void Disconnect()
    {
        if( Connected != null )
        {
            Connected.Connected = null;
            this.Connected = null;
        }
    }
}