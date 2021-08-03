using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;
    [SerializeField]
    float _scanRange = 10;
    [SerializeField]
    float _attackRange = 2;

    [SerializeField]
    float distance;


    public override void Init()
    {
        WorldObject = Define.WorldObject.Monster;
        _stat = gameObject.GetComponent<Stat>();
        if(gameObject.GetComponentInChildren<UI_HpBar>() == null)
        Managers.UI.MakeWordSpaceUI<UI_HpBar>(transform);
    }

    protected override void Idle()
    {
        GameObject player = Managers.Game.GetPlayer();
        if (player == null) return;

        distance = (player.transform.position - transform.position).magnitude;
        if(distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Run;
            return;
        }
    }

    protected override void Run()
    {
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            distance = (_destPos - transform.position).magnitude;
            if (distance <= _attackRange)
            {
                NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
                nav.SetDestination(transform.position);
                State = Define.State.Attack;
                return;
            }
        }

        //방향을 먼저 구하자
        Vector3 dir = _destPos - transform.position;
        //오차 범위 허용
        if (dir.magnitude < 0.1f)
        { 
            State = Define.State.Idle;
        }
        else
        {
            //float _moveDist = Mathf.Clamp(Time.deltaTime * _stat.MoveSpeed, 0, dir.magnitude);
            //transform.position += dir.normalized * _moveDist;
            NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
            nav.SetDestination(_destPos);
            nav.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
            nav.speed = _stat.MoveSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
        
    }
    protected override void Skill()
    {
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);

            if(targetStat.Hp > 0)
            {
              distance = (_lockTarget.transform.position - transform.position).magnitude;
                if (distance <= _attackRange)
                {
                    State = Define.State.Attack;
                }
                else
                {
                    State = Define.State.Run;
                }
            }
            else
            {
                State = Define.State.Idle;
            }

        }
        else
        {
            State = Define.State.Idle;
        }
    }
}
