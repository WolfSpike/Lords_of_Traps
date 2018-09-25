using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : Life_Object {

    Rigidbody2D _P_Phys;
    public float _P_Speed_move;
    public float _Max_Speed;
    float R_Drag = 10;

    void Start () {
        _Heal_Point = _Max_Heal_Point;
        _P_Phys = this.GetComponent<Rigidbody2D>();
	}
	

	void Update () {
        Player_Controller_Movement();
    }

    void Player_Controller_Movement()
    {
        if (_P_Phys.velocity.x <= _Max_Speed)
            if (Input.GetKey(KeyCode.D))
            {
                _P_Phys.drag = R_Drag;
                _P_Phys.AddForce(Vector3.right * _P_Speed_move * Time.deltaTime, ForceMode2D.Impulse);
            }

        if (_P_Phys.velocity.x >= -_Max_Speed)
            if (Input.GetKey(KeyCode.A))
            {
                _P_Phys.drag = R_Drag;
                _P_Phys.AddForce(Vector3.left * _P_Speed_move * Time.deltaTime, ForceMode2D.Impulse);
            }

        if (_P_Phys.velocity.y <= _Max_Speed)
            if (Input.GetKey(KeyCode.W))
            {
                _P_Phys.drag = R_Drag;
                _P_Phys.AddForce(Vector3.up * _P_Speed_move * Time.deltaTime, ForceMode2D.Impulse);
            }

        if (_P_Phys.velocity.y >= -_Max_Speed)
            if (Input.GetKey(KeyCode.S))
            {
                _P_Phys.drag = R_Drag;
                _P_Phys.AddForce(Vector3.down * _P_Speed_move * Time.deltaTime, ForceMode2D.Impulse);
            }
    }
}
