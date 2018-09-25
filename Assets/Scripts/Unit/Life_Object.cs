using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life_Object : MonoBehaviour
{
    [Range(0f,1f)]
    public float _Phys_Resist;
    [Range(0f,1f)]
    public float _Magic_Resist;
    public float _Max_Heal_Point;
    public float _Heal_Point;
}
