using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Camera ) )]
public class CameraMovement : MonoBehaviour
{
    public Transform CameraBoom;
    private Camera cam;
    public Vector3 WorldMovementForce;

    public bool CumulativeBtnSpeed = true;
    public bool isSmooth = true;

    public float LerpSmooth = .1f;

    public float SpeedCumulation = .1f;
    public float Speed = 10f;
    public float TimeBtnDown = 0f;

    public float LimitViewAngleDeg = 80f;
    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        bool cumulate = false;
        Vector3 movement = ProcessInput( out cumulate );
        movement *= Speed;

        ProcessMouseInput();

        if( cumulate && CumulativeBtnSpeed )
        {
            TimeBtnDown += Time.deltaTime;
            CumulateSpeed( ref movement );
        }
        else
        {
            TimeBtnDown = 0f;
        }

        WorldMovementForce += movement;//cam.transform.TransformDirection( movement );

        if( isSmooth )
        {
            SmoothMovemement();
        }
        CameraBoom.transform.position += WorldMovementForce * Time.deltaTime;
    }

    public void CumulateSpeed( ref Vector3 movement )
    {
        movement *= ( 1f + ( TimeBtnDown * SpeedCumulation ) );
    }

    public void SmoothMovemement()
    {
        LerpSmooth = Mathf.Clamp01( LerpSmooth );

        WorldMovementForce *= ( 1f - LerpSmooth );

    }

    public Vector3 ProcessInput( out bool InputActive )
    {
        InputActive = false;
        Vector3 movement = Vector3.zero;

        if( Input.GetKey( KeyCode.W ) )
        {
            InputActive = true;
            movement += cam.transform.forward;
        }
        if( Input.GetKey( KeyCode.A ) )
        {
            InputActive = true;
            movement -= cam.transform.right;
        }
        if( Input.GetKey( KeyCode.S ) )
        {
            InputActive = true;
            movement -= cam.transform.forward;
        }
        if( Input.GetKey( KeyCode.D ) )
        {
            InputActive = true;
            movement += cam.transform.right;
        }

        return movement;
    }

    public void ProcessMouseInput()
    {
        Vector3 eulerRot = new Vector3( -Input.GetAxis( "Mouse Y" ), Input.GetAxis( "Mouse X" ) );
        CameraBoom.transform.eulerAngles += eulerRot;
        LimitView();
    }

    public void LimitView()
    {
        Vector3 CamAngles = CameraBoom.transform.localEulerAngles;

        if( CamAngles.x < 180f && CamAngles.x > LimitViewAngleDeg  )
        {
            CamAngles = new Vector3( LimitViewAngleDeg , CamAngles.y, CamAngles.z );
        }
        if( CamAngles.x > 180f && CamAngles.x < 360f - LimitViewAngleDeg  )
        {
            CamAngles = new Vector3( 360f - LimitViewAngleDeg , CamAngles.y, CamAngles.z );
        }
        CameraBoom.transform.localEulerAngles = CamAngles;
    }
}
