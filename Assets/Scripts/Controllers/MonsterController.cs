using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    /**********************************************************************/
    // fields
    private Stat _stat;

    [SerializeField] private float _scanRange = 10f;
    
    [SerializeField] private float _attackRange = 2f;
    
    /**********************************************************************/
    // system
    public override void init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = GetComponent<Stat>();

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }
    
    /**********************************************************************/
    // methods
    protected override void UpdateIdle()
    {

        GameObject player = Managers.Game.GetPlayer;
        if (player == null)
            return;

        float dist = (player.transform.position - transform.position).magnitude;
        if (dist <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }
    
    protected override void UpdateMoving()
    {
        
        // 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float dist = (_destPos - transform.position).magnitude;
            if (dist <= _attackRange)
            {
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);
                State = Define.State.Skill;
                return;
            }
        }
        
        // 이동
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
            State = Define.State.Idle;
        
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpeed;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }
    
    protected override void UpdateSkill()
    {
       if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }
    
    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            // 체력
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);

            // if (targetStat.Hp <= 0)
                // Managers.Game.Despawn(targetStat.gameObject);

            if (targetStat.Hp > 0)
            {
                float dist = (_lockTarget.transform.position - transform.position).magnitude;
                if (dist <= _attackRange)
                    State = Define.State.Skill;
                else
                    State = Define.State.Moving;
            }
            else
                State = Define.State.Idle;
        }
        else
            State = Define.State.Idle;
    }

}
