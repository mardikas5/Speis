using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    

    public int TicksPerSecond = 5;
    
    private int TickRate
    {
        get 
        {
            return 1f / (float)TicksPerSecond;
        }
    }
    
    
	// Use this for initialization
	void Start ()
    {
		Program.StandardStart();
        Program.EnterTestValues();

        StartCoroutine(Run());
	}
	
    public IEnumerator Run()
    {
        while (true)
        {
            Simulation.Instance.Tick();

            yield return new WaitForSeconds(TickRate);
        }
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
