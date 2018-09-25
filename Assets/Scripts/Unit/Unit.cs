using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Life_Object
{    
    public Transform target;
    public float speed;
    public Vector3[] path;
    public float TimePathFind;
    int targetIndex;

    Rigidbody2D _Phys;
    Vector2 Point_to_move;

    public bool Follow_the_path;
    public string Move_set;

    void Start()
    {
        _Phys = this.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move_Controller();

        if (RefreshPathRequest())
        {
            path = new Vector3[0];
            if(Follow_the_path)
                PathManager.Path_Request(transform.position, target.position, OnPathFound);
        }

        if (_Heal_Point <= 0)
        {
            Destroy(gameObject);
        }


    }

    

    public void Move_Controller()
    {
        if (!Physics2D.Linecast(transform.position, target.position, 520))
        {
            if (Vector2.Distance(transform.position, target.transform.position) < 10f)
            {
                Move_set = "Ray";
            }
            else
                Move_set = "Path";
        }
        else
            Move_set = "Path";




        switch (Move_set)
        {
            case "Path":
                Follow_the_path = true;
                break;

            case "Ray":
                Follow_the_path = false;
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 3);                      
                break;
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;

            Delay = 5;

            if (path.Length > 100)
                Delay = 10;
            if (path.Length > 50)
                Delay = 5;
            if (path.Length < 20)
                Delay = 2;

            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path == null)
            yield return null;

        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (Follow_the_path)
            {
                if (Physics2D.OverlapCircle(currentWaypoint, .005f))
                //if(transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                    Point_to_move = currentWaypoint - transform.position;
                    //_Phys.velocity = Vector2.zero;
                }

                //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                _Phys.AddForce((currentWaypoint - transform.position) * speed, ForceMode2D.Force);
            }
            yield return null;
        }
    }

    

    public float Delay;
    public bool RefreshPathRequest()
    {
        Delay -= Time.deltaTime;
        if (Delay <= 0)
        {
            return true;    
        }
        else
            return false;
    }
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one / 3);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}

