using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_offset_tracing : MonoBehaviour {

    Vector2 last_pos;
    public bool Left_move;
    public bool Right_move;

    void Start () {
		
	}
	
	void Update () {
        VectorTracing();
	}

    void VectorTracing()
    {
        float offset_x;

        if((Vector2)transform.position != last_pos)
        {
            offset_x = transform.position.x - last_pos.x;
            //Debug.Log("y: " + offset_y + " x: " + offset_x);

            if (offset_x > 0)
            {
                transform.eulerAngles = Vector3.zero;
                Left_move = false;
                Right_move = true;
            }
            if (offset_x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                Left_move = true;
                Right_move = false;
            }
        }

        last_pos = transform.position;
    }
}
