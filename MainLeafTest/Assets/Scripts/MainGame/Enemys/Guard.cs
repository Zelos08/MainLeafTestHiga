using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public GameObject[] path;
    enum State { walking, idle};
    private State e_state;
    public bool b_startWalking;
    //Walk variables
    public int i_actualPath; //Use to define start point
    public float f_velocity;
    public float f_timeInIdle;
    private float f_timeToWalk;
    private float f_timeWalking;
    private Vector3 v3_startPos;

    //Animations variables
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        //Get the Animator attached to the GameObject.
        _animator = gameObject.GetComponentInChildren<Animator>();

        if (_animator == null)
        {
            Debug.LogException(new UnityException("Add an animator to the guard."));
            return;
        }

        transform.position = path[i_actualPath].transform.position;

        if (b_startWalking)
            StartWalkToNextPointOnPath();
        else
            StardIdle();
    }

    private void FixedUpdate()
    {
        if (!GameController.GetPaused())
        {
            switch (e_state)
            {
                case State.walking:
                    WalkToNextPointOnPath();
                    break;
                case State.idle:
                    break;
            }
        }
    }

    //Animations functions
    private void StartWalkToNextPointOnPath()
    {
        SetMoveVelocity(f_velocity/5);
        i_actualPath++;
        if (i_actualPath == path.Length) i_actualPath = 0;

        v3_startPos = transform.position;
        e_state = State.walking;
        transform.LookAt(path[i_actualPath].transform);
        CalcTimeToWalk();
    }

    private void CalcTimeToWalk()
    {
        f_timeToWalk = Vector3.Magnitude(v3_startPos - path[i_actualPath].transform.position) / f_velocity;
        f_timeWalking = 0;
    }
    private void WalkToNextPointOnPath()
    {
        f_timeWalking += Time.deltaTime;
        transform.position = Vector3.Lerp(v3_startPos, path[i_actualPath].transform.position, f_timeWalking/ f_timeToWalk);

        if (f_timeWalking > f_timeToWalk) StardIdle();
    }

    private void StardIdle()
    {
        SetMoveVelocity(0);
        e_state = State.idle;
        StartCoroutine(BackToWalk(f_timeInIdle));
    }

    IEnumerator BackToWalk(float f_time)
    {
        yield return new WaitForSeconds(f_time);
        
        StartWalkToNextPointOnPath();
    }

    //Animations functions
    private void SetMoveVelocity(float f_moveVelocity)
    {
        _animator.SetFloat("Forward", f_moveVelocity);
    }
}
