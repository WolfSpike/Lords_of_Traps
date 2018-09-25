using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Traps : MonoBehaviour
{
    //Damage option

    public enum TrapTimeType { Fixed, Random }
    public enum TrapDamageType { Phys, Magic }
    public enum TrapMoveType { Vector, BlackHole, Explosion };
    public enum TrapDebuffType { Ignite, Blindness, Hipnose, Root }

    public TrapDebuffType Type_Debuff;
    public int Debuff_Time;

    public TrapDamageType Type_Damage;
    public float Damage_count;

    public List<GameObject> _Object_in_trap_zone = new List<GameObject>();

    // Time option

    public TrapTimeType Trap_Time;
    public float CoolDown;
    public float MinCD;
    public float MaxCD;

    public TrapMoveType Type_Move;
    public Vector2 Push_Direction;
    //public float Speed_Push;

    // Editor option

    public bool _Damage_Work;
    public bool _Movement_Work;
    public bool _Debuff_Work;

    //Area option

    public enum TrapAreaType { Circle, Box }
    public TrapAreaType Trap_Area;
    public float Area_Size_x;
    public float Area_Size_y;

    public bool One_Shot;
    public ParticleSystem[] P_Sys;

    // P_SYS 0 - TRAP_ACT
    // P_SYS 1 - TRAP_EFFECT

    void Awake()
    {
        Time_Changer();
        P_Sys = GetComponentsInChildren<ParticleSystem>();
        _MainCam = GameObject.FindGameObjectWithTag("MainCamera");
        P_Sys[0].Stop(false);

        if (One_Shot)
        {
            P_Sys[1].Stop(false);
            var main = P_Sys[1].main;
            main.loop = false;
            main.duration = CoolDown - 2f;
            P_Sys[1].Play(false);
        }
    }

    public bool Traps_Working = true;

    void Update()
    {
        CoolDownTimer();

        if(Traps_Working)
            Traps_work();
    }

    public void Enemy_Push(Vector2 _Direction)
    {
        if (Object_Check())
        {
            for (int count_enemy = 0; count_enemy < _Object_in_trap_zone.Count; count_enemy++)
            {
                _Object_in_trap_zone[count_enemy].GetComponent<Rigidbody2D>().AddForce(_Direction * 1000, ForceMode2D.Force);
            }
        }

    }

    public void Enemy_Push(GameObject Obj, Vector2 _Direction, float _Force)
    {
        if (CoolDownTimer())
        {
            Obj.GetComponent<Rigidbody2D>().AddForce(_Direction * _Force, ForceMode2D.Force);
        }
    }

    public void Enemy_Hit(float Damage, TrapDamageType _Dmg_Type)
    {
        if (Object_Check())
        {
            for (int count_enemy = 0; count_enemy < _Object_in_trap_zone.Count; count_enemy++)
            {
                _Object_in_trap_zone[count_enemy].GetComponent<Life_Object>()._Heal_Point -= Calc_Damage(_Object_in_trap_zone[count_enemy], Damage, _Dmg_Type);
            }
        }
    }

    public void Enemy_Debuff(int Debuff_Time_work)
    {

        if (Object_Check())
        {
            for (int count_enemy = 0; count_enemy < _Object_in_trap_zone.Count; count_enemy++)
            {
                switch (Type_Debuff)
                {
                    case TrapDebuffType.Ignite:
                        StartCoroutine(Set_Ignite(Debuff_Time_work, _Object_in_trap_zone[count_enemy]));
                        break;

                    case TrapDebuffType.Blindness:
                        StartCoroutine(Camera_Effect(Resources.Load("Effects/Spotlight") as Material));
                        break;

                    case TrapDebuffType.Hipnose:
                        StartCoroutine(Camera_Effect(Resources.Load("Effects/Gipnose") as Material));
                        break;

                    case TrapDebuffType.Root:
                        StartCoroutine(Set_Root(Debuff_Time_work, _Object_in_trap_zone[count_enemy], _Object_in_trap_zone[count_enemy].transform.position));
                        break;
                }
            }
        }

    }

    IEnumerator Set_Ignite(float _Time, GameObject obj)
    {
        float duration = _Time;

        while (duration > 0)
        {
            _Time -= 1f;
            obj.GetComponent<Life_Object>()._Heal_Point -= obj.GetComponent<Life_Object>()._Max_Heal_Point / 100;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Set_Root(float RootTime, GameObject obj, Vector2 pos)
    {
        RootTime *= 0.5f;

        while (true)
        {
            if (RootTime > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime / 2f);
                obj.transform.position = pos;
                RootTime -= Time.deltaTime;
            }
            else
                yield break;
        }
    }

    Collider2D[] _Check;

    public bool Object_Check()
    {
        if (Trap_Area == TrapAreaType.Circle)
        {
            _Check = Physics2D.OverlapCircleAll(transform.position, Area_Size_x);
        }
        if (Trap_Area == TrapAreaType.Box)
        {
            _Check = Physics2D.OverlapBoxAll(transform.position, new Vector2(Area_Size_x, Area_Size_y), 0);
        }

        _Object_in_trap_zone.Clear();

        for (int count = 0; count < _Check.Length; count++)
        {
            if (_Check[count].gameObject.tag == "Life_Object")
            {
                _Object_in_trap_zone.Add(_Check[count].gameObject);
            }
        }

        if (_Object_in_trap_zone.Count > 0)
            return true;
        else
        {
            _Object_in_trap_zone.Clear();
            return false;
        }
    }

    public float Calc_Damage(GameObject _Life_Obj, float Damage, TrapDamageType _type)
    {
        if ((int)_type == (int)TrapDamageType.Phys)
            return Damage * (1 - _Life_Obj.GetComponent<Life_Object>()._Phys_Resist);

        if ((int)_type == (int)TrapDamageType.Magic)
            return Damage * (1 - _Life_Obj.GetComponent<Life_Object>()._Magic_Resist);

        return 0;
    }

    public float _Time_to_CD;
    public bool CoolDownTimer()
    {
        _Time_to_CD -= Time.deltaTime;

        if (_Time_to_CD <= 0)
        {
            return true;
        }
        else
            return false;
    }

    Vector2 Direction_to_object(GameObject TO_obj, GameObject DO_obj)
    {
        return ((Vector2)TO_obj.transform.position - (Vector2)DO_obj.transform.position).normalized;
    }
    void Traps_work()
    {

        if (CoolDownTimer())
        {
            if (_Damage_Work)
            {
                Enemy_Hit(Damage_count, Type_Damage);
            }

            if (_Movement_Work)
            {
                switch (Type_Move)
                {
                    case TrapMoveType.Vector:
                        Enemy_Push(Push_Direction);
                        break;

                    case TrapMoveType.BlackHole:
                        GameObject[] BlackHole_Targets = GameObject.FindGameObjectsWithTag("Life_Object");
                        for (int b = 0; b < BlackHole_Targets.Length; b++)
                        {
                            Enemy_Push(BlackHole_Targets[b], Direction_to_object(gameObject, BlackHole_Targets[b]), Random.Range(1000, 3000));
                        }
                        break;

                    case TrapMoveType.Explosion:
                        GameObject[] Explosion_Targets = GameObject.FindGameObjectsWithTag("Life_Object");
                        for (int e = 0; e < Explosion_Targets.Length; e++)
                        {
                            Enemy_Push(Explosion_Targets[e], -Direction_to_object(gameObject, Explosion_Targets[e]), Random.Range(1000, 3000));
                        }
                        break;
                }
            }

            if (_Debuff_Work)
            {
                Enemy_Debuff(Debuff_Time);
            }

            P_Sys[1].Stop(false);
            Particle_Controller(P_Sys[0]);

            Time_Changer();
        }
    }

    public void Particle_Controller(ParticleSystem Part)
    {
        Part.Play(false);

        if (One_Shot)
        {
            Part.Play(false);

            if (_Debuff_Work)
                Destroy(gameObject, Debuff_Time);
            else
                Destroy(gameObject, 3f);

            if (One_Shot)
                Traps_Working = false;
        }
            
    }

    public void Time_Changer()
    {
        if (Trap_Time == TrapTimeType.Fixed)
            _Time_to_CD = CoolDown;
        else
        {
            CoolDown = Random.Range(MinCD, MaxCD);
            _Time_to_CD = CoolDown;
        }
    }

    public GameObject _MainCam;

    IEnumerator Camera_Effect(Material Image_Effect)
    {
        _MainCam.GetComponent<CamEffect>().EffectMaterial = Image_Effect;
        yield return new WaitForSeconds(Debuff_Time - 0.5f);
        _MainCam.GetComponent<CamEffect>().EffectMaterial = Resources.Load("Effects/Cam_Null_Effect") as Material;

    }
}



