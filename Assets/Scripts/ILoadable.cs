using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoadable<T, W> where T : class where W : PersistentData
{
    W Data { get; set; }

    T Load( PersistentData data );

    W Save( T obj );
}

public abstract class PersistentData
{

}
