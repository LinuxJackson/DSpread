﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libObjProperties;
using libData_Memory;
using libFlags;

namespace ObjectControls
{
    static public class Attack
    {
        static public void AttackTarget(List<DSObject> arrAttacker, List<DSObject> arrBeAttacked, TeamSign.Teams attacker, bool isviolated = false)
        {
            double averageATK = 0;

            foreach (DSObject ds in arrAttacker)
                averageATK = ds.ObjectProperties.Atk + averageATK;
            averageATK = averageATK / arrAttacker.Count;

            if(isviolated)
                averageATK = averageATK * 3;

            List<DSObject> arrDeadObjects = new List<DSObject>();

            foreach (DSObject ds in arrBeAttacked)
            {
                if(ds.ObjectProperties.AvoidingAttack)
                {
                    ds.ObjectProperties.AvoidingAttack = false;
                    continue;
                }
                ds.ObjectProperties.Ph = ds.ObjectProperties.Ph - averageATK * (100 / (100 + ds.ObjectProperties.ActualArmor));
                if (!ds.ObjectProperties.IsAlive)
                {
                    arrDeadObjects.Add(ds);
                    List<DSObject> listOld = new List<DSObject>();
                    if (attacker == TeamSign.Teams.User)
                        listOld = GameData.arrEnemyObjects;
                    else
                        listOld = GameData.arrUserObjects;

                    List<DSObject> listNew = new List<DSObject>();
                    foreach (DSObject dsDelete in listOld)
                    {
                        if (dsDelete == ds)
                            continue;
                        listNew.Add(dsDelete);
                    }
                    listOld = listNew;
                }
            }

            if (arrDeadObjects.Count != 0)
            {
                double addATK = 0;
                double addArmor = 0;
                
                foreach(DSObject ds in arrDeadObjects)
                {
                    addATK = addATK + 0.5 * ds.ObjectProperties.Atk;
                    addArmor = addArmor + 0.5 * ds.ObjectProperties.Armor;
                }

                addATK = addATK / arrAttacker.Count;
                addArmor = addArmor / arrAttacker.Count;

                foreach(DSObject ds in arrAttacker)
                {
                    ds.ObjectProperties.Atk = ds.ObjectProperties.Atk + addATK;
                    ds.ObjectProperties.Armor = ds.ObjectProperties.Armor + addArmor;
                }
            }

            //检测胜利
            if (GameData.arrEnemyObjects.Count == 0)
            {
                GameState.IsEnd = true;
                GameState.WinTeam = TeamSign.Teams.User;
            }
            if (GameData.arrUserObjects.Count == 0)
            {
                GameState.IsEnd = true;
                GameState.WinTeam = TeamSign.Teams.Enemy;
            }
        }
    }
}
