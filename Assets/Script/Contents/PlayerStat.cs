using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    float _jumpPower;
    [SerializeField]
    int _gold;
    [SerializeField]
    int _exp;

    public int Gold { get { return _gold; } set { _gold = value; } }
    public int Exp
    {
        get { return _exp; }
        set
        { 
            _exp = value;

            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                level++;
            }

            if(level != Level)
            {
                Debug.Log("Level up!");
                Level = level;
                SetStat(Level);
            }
        }
    }

    private void Start()
    {
        _level = 1;
        SetStat(_level);        
        _defense = 5;
        _speed = 5.0f;
        _gold = 0;
        _jumpPower = 10.0f;
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;

    }
    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
