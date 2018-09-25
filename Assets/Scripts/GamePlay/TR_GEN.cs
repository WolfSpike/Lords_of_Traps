using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_GEN : MonoBehaviour {

    public bool _Debug;
    public float Time_Create_Trap;
    float Time_Create_Heal;

    public Vector2 Size_Gen_Area;

    public List<GameObject> Traps = new List<GameObject>();
    public List<GameObject> Force_Traps = new List<GameObject>();
    public GameObject Heal;
    float Time_to_Create;

    void Start () {
        Time_to_Create = Time_Create_Trap;
    }

    void Update()
    {
        if (Time_to_Create <= 0)
        {
            Time_to_Create = Time_Create_Trap;
            Traps_Create();
        }

        Time_to_Create -= Time.deltaTime;
    }

    float Create_Step;

    void Traps_Create()
    {
        Instantiate(Traps[Random.Range(0, Traps.Count)], Create_Pos(), Quaternion.identity);
        Create_Step++;

        if (Create_Step == 3)
        {
            Instantiate(Force_Traps[Random.Range(0, Force_Traps.Count)], Create_Pos(), Quaternion.identity);
            Create_Step = 0;
        }

        if (Time_Create_Trap < 0)
        {
            Time_Create_Trap = Random.Range(0.5f, 2f);
            Instantiate(Heal, Create_Pos(), Quaternion.identity);
        }
        else
            Time_Create_Trap -= Time.deltaTime;
    }

    public Vector2 Create_Pos()
    {
        float x = Random.Range(transform.position.x - Size_Gen_Area.x / 2, transform.position.x + Size_Gen_Area.x / 2);
        float y = Random.Range(transform.position.y - Size_Gen_Area.y / 2, transform.position.y + Size_Gen_Area.y / 2);
        Vector2 Pos = new Vector2(x, y);
        return Pos;
    }

    private void OnDrawGizmos()
    {
        if (_Debug)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, Size_Gen_Area);
        }
    }
}
 