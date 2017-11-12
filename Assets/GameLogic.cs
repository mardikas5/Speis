using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

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

            yield return new WaitForSeconds(.2f);
        }
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
