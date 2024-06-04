using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster/Create Monster", order = 0)]
public class MonsterInfo : ScriptableObject
{
    //public string name;
    public int level;
    public int hp;
    public float moveSpeed;
    public float fieldOfView;
    public float attackRange;
    public float monsterDistance;
}
