#define unity
using System.Collections;
using System.Collections.Generic;

#if unity
using UnityEngine;
#endif

public static class MyDebug
{

#if unity
    public static void DebugWrite(string s)
    {
        Debug.Log(s);
    }
#else
    public static void DebugWrite(string s)
    {
        Console.WriteLine(s);
    }
#endif
}
