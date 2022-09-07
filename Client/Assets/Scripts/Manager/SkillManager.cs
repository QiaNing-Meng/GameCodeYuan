using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
public class SkillManager : TS_Singleton<SkillManager>
{
    public Dictionary<int, int> selfSkill = new Dictionary<int, int>();
    public Dictionary<int, SkillBase> AllSkills = new Dictionary<int, SkillBase>();



    public SkillManager()
    {
        AllSkills.Add(50001, new BezierFireSkill());


        selfSkill.Add(1, 50001);
    }

    public void Init()
    {
        

    }


    public bool ReleaseSkill(int skillID)
    {
        if (selfSkill.TryGetValue(skillID,out int val))
        {
            if (val!=0)
            {
                Debug.LogError($"�ҵ�����{val},��ʼִ��");

                //���ñ��ѯ��������TODO
                //ִ�м���
                OpenSkill(val);
                return true;


            }

        }
        return false;
    }

    public void OpenSkill(int id)
    {
        if (AllSkills.TryGetValue(id,out SkillBase val))
        {

            val.startSkill();

        }

    }

}
