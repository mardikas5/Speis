using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Singleton<T> : SingletonBase where T : class
{
    public override void Start()
    {
        base.Start();
        TryCreateInstance();
    }

    public void TryCreateInstance()
    {
        if( _instance != null )
        {
            //Console.WriteLine( "Instance already exists" );
            return;
        }
        _instance = this as T;
    }

    private static T _instance;

    public override void Initialize()
    {
        base.Initialize();
        TryCreateInstance();
    }

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }
}

public abstract class SingletonBase : MonoBehaviour
{
    public bool Initialized = false;

    public virtual void Start()
    {

    }

    public virtual void Initialize()
    {
        Initialized = true;
    }
}