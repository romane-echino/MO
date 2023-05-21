using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public List<Skill> Skills = new List<Skill>();
    private bool _isPreparing = false;
    private Skill _currentSkill;

    public List<Skill> GetSkills()
    {
        return this.Skills;
    }

    public Skill GetCurrentSkill()
    {
        return this._currentSkill;
    }

    public bool IsPreparing()
    {
        return _isPreparing;
    }

    public void PrepareAttack(Skill s)
    {
        if (!_isPreparing)
        {
            _isPreparing = true;
            this._currentSkill = s;
        }
        else if (s != this._currentSkill)
        {
            this._currentSkill = s;
        }
        else
        {
            _isPreparing = false;
        }
    }

    public void Slay(){
        this._isPreparing = false;
    }

    void Awake()
    {
        Skills.Add(new Skill("Attack", "ATQ", new string[] { "1_0:5","2_0:2","2_1:2" }, .5f));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
