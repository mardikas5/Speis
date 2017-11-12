using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable object? : turn into composition style?
public class StructureTemplate
{       
    public string Name;
    public List<Resource> BuildingCost;
}

public class ResourceProducerTemplate : StructureTemplate
{
    public List<Resource> Inputs;
    public List<Resource> Outputs;
}

public class StorageTemplate : StructureTemplate
{       
    public float Volume;
}