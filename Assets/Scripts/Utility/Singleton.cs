using System;
using System.Collections.Generic;

[Serializable]
public class Singleton<T> where T : class
{
    public Singleton()
    {
         Console.WriteLine(typeof(T).ToString());
         if (_instance != null)
         {
             Console.WriteLine("Instance already exists");
             return;
         }
        _instance = this as T;
    }
    
    private static T _instance;

    public static T Instance 
    { 
        get 
        { 
            return _instance;
        }
    }
}
