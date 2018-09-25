using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Traps))]
public class Traps_Editor : Editor
{

    bool Debug;

    public override void OnInspectorGUI()
    {
        Debug = EditorGUILayout.Toggle("Debug:", Debug);

        if(Debug)
            base.OnInspectorGUI();

        Handles.color = Color.red;
        Traps _Trap = (Traps)target;

        _Trap.Trap_Time = (Traps.TrapTimeType)EditorGUILayout.EnumPopup("Timer type: ", _Trap.Trap_Time);

        if (_Trap.Trap_Time == Traps.TrapTimeType.Fixed)
            _Trap.CoolDown = EditorGUILayout.FloatField("ColdDown: ", _Trap.CoolDown);
        else
        {
            _Trap.MinCD = EditorGUILayout.FloatField("MinCD: ", _Trap.MinCD);
            _Trap.MaxCD = EditorGUILayout.FloatField("MaxCD: ", _Trap.MaxCD);
        }
                

        _Trap._Damage_Work = EditorGUILayout.Toggle("Damage", _Trap._Damage_Work);


        if (_Trap._Damage_Work)
        {
            _Trap.Type_Damage = (Traps.TrapDamageType)EditorGUILayout.EnumPopup("Damage type: ", _Trap.Type_Damage);
            _Trap.Damage_count = EditorGUILayout.FloatField("Damage count: ", _Trap.Damage_count);
        }

        _Trap._Movement_Work = EditorGUILayout.Toggle("Movement: ", _Trap._Movement_Work);

        if (_Trap._Movement_Work)
        {
            _Trap.Type_Move = (Traps.TrapMoveType)EditorGUILayout.EnumPopup("Movement type: ", _Trap.Type_Move);

            if (_Trap.Type_Move == Traps.TrapMoveType.Vector)
                _Trap.Push_Direction = EditorGUILayout.Vector2Field("Push Vector: ", _Trap.Push_Direction);
        }

        _Trap._Debuff_Work = EditorGUILayout.Toggle("Debuff: ", _Trap._Debuff_Work);

        if (_Trap._Debuff_Work)
        {
            _Trap.Type_Debuff = (Traps.TrapDebuffType)EditorGUILayout.EnumPopup("Debuff type: ", _Trap.Type_Debuff);
            _Trap.Debuff_Time = EditorGUILayout.IntField("Debuff time:", _Trap.Debuff_Time);
        }

        _Trap.Trap_Area = (Traps.TrapAreaType)EditorGUILayout.EnumPopup("Area type: ", _Trap.Trap_Area);

        if (_Trap.Trap_Area == Traps.TrapAreaType.Circle)
        {
            _Trap.Area_Size_x = EditorGUILayout.FloatField("R: ", _Trap.Area_Size_x);
            Handles.DrawWireArc(_Trap.transform.position, Vector3.forward, Vector3.zero, 360, _Trap.Area_Size_x);
        }
        if (_Trap.Trap_Area == Traps.TrapAreaType.Box)
        {
            _Trap.Area_Size_x = EditorGUILayout.FloatField("x: ", _Trap.Area_Size_x);
            _Trap.Area_Size_y = EditorGUILayout.FloatField("y: ", _Trap.Area_Size_y);
        }

        _Trap.One_Shot = EditorGUILayout.Toggle("One_Shot:", _Trap.One_Shot);

        EditorGUILayout.LabelField("TrapTimer " + _Trap._Time_to_CD);       
    }



    private void OnSceneGUI()
    {
        Traps _Trap = (Traps)target;
        Handles.color = Color.red;

        if (_Trap.Trap_Area == Traps.TrapAreaType.Box)
        {
            Handles.DrawWireCube(_Trap.transform.position, new Vector3(_Trap.Area_Size_x, _Trap.Area_Size_y));
        }
        if (_Trap.Trap_Area == Traps.TrapAreaType.Circle)
        {
            Handles.DrawWireArc(_Trap.transform.position, Vector3.forward, _Trap.transform.position, 360, _Trap.Area_Size_x);
        }

    }
}

