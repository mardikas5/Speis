using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProtoMono : MonoBehaviour
{
    public abstract T SaveObject<T>() where T : ProtoBase;
}
