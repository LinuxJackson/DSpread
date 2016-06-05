using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace libObjProperties
{
    public class Skills : DefaultProperties
    {
        public Skills()
        {
            AttacksAvailable = true;
            ExtraArmorAvailable = false;
            AvoidingAttackAvailable = false;
            ViolentAttacksAvailable = false;

            Atk = DefaultParameters.StartingATK;
            Armor = DefaultParameters.StartingArmor;
            Ph = DefaultParameters.PH;
        }

        //普通攻击
        private DispatcherTimer tmrAttacksCD = new DispatcherTimer();
        int attacksCD;

        public int AttacksCD
        {
            get { return attacksCD; }
            set { attacksCD = value; }
        }

        private bool attacksAvailable;

        public bool AttacksAvailable
        {
            get { return attacksAvailable; }
            set { 
                attacksAvailable = value;
                if (!attacksAvailable)
                {
                    attacksCD = (int)(10 - 0.05 * Atk);
                    tmrAttacksCD = new DispatcherTimer();
                    tmrAttacksCD.Interval = TimeSpan.FromSeconds(1);
                    tmrAttacksCD.Tick += new EventHandler(tmrAttacksCD_Tick);
                    tmrAttacksCD.IsEnabled = true;
                }
            }
        }

        void tmrAttacksCD_Tick(object sender, EventArgs e)
        {
            attacksCD--;
            if (attacksCD <= 0)
            {
                AttacksAvailable = true;
                tmrAttacksCD.IsEnabled = false;
                tmrAttacksCD.Tick -= tmrAttacksCD_Tick;
            }
        }

        //护甲
        private bool extraArmor = false;
        private double preArmor;

        public bool ExtraArmor
        {
            get { return extraArmor; }
            set { 
                extraArmor = value;
                if (extraArmor)
                {
                    //开启护甲，开始倒计时
                    extraArmorAvailable = false;
                    preArmor = Armor;
                    Armor = Armor * 3;
                    extraArmorTime = (int)(20 + 0.1 * Armor);
                    tmrExtraArmorLastTime = new DispatcherTimer();
                    tmrExtraArmorLastTime.Interval = TimeSpan.FromSeconds(1);
                    tmrExtraArmorLastTime.Tick += tmrExtraArmorLastTime_Tick;
                    tmrExtraArmorLastTime.IsEnabled = true;
                }
                else
                {
                    Armor = preArmor;
                    tmrExtraArmorLastTime.IsEnabled = false;
                    //护甲有效时间过期，开启CD
                    extraArmorCD = (int)(40 + 0.15 * Armor);
                    tmrExtraArmorCD = new DispatcherTimer();
                    tmrExtraArmorCD.Interval = TimeSpan.FromSeconds(1);
                    tmrExtraArmorCD.Tick += tmrExtraArmorCD_Tick;
                    tmrExtraArmorCD.IsEnabled = true;
                }
            }
        }

        //额外护甲持续时间
        DispatcherTimer tmrExtraArmorLastTime = new DispatcherTimer();

        int extraArmorTime;

        public int ExtraArmorTime
        {
            get { return extraArmorTime; }
            set { extraArmorTime = value; }
        }

        void tmrExtraArmorLastTime_Tick(object sender, EventArgs e)
        {
            extraArmorTime--;
            if (extraArmorTime <= 0)
            {
                tmrExtraArmorLastTime.IsEnabled = false;
                tmrExtraArmorLastTime.Tick -= tmrExtraArmorLastTime_Tick;
                ExtraArmor = false;
            }
        }

        //额外护甲CD
        DispatcherTimer tmrExtraArmorCD = new DispatcherTimer();
        bool extraArmorAvailable = false;

        public bool ExtraArmorAvailable
        {
            get { return extraArmorAvailable; }
            set { extraArmorAvailable = value; }
        }

        int extraArmorCD;

        public int ExtraArmorCD
        {
            get { return extraArmorCD; }
            set { extraArmorCD = value; }
        }

        void tmrExtraArmorCD_Tick(object sender, EventArgs e)
        {
            extraArmorCD--;
            if (extraArmorCD <= 0)
            {
                //CD完成，技能可使用
                extraArmorAvailable = true;
                tmrExtraArmorCD.IsEnabled = false;
                tmrExtraArmorCD.Tick -= tmrExtraArmorCD_Tick;
            }
        }


        ////额外伤害
        //private double extraATK;

        //public double ExtraATK
        //{
        //    get {
        //        if (extraATKVisibility)
        //        {
        //            return extraATK;
        //        }
        //        else
        //            return 0;
        //    }
        //    set { 
        //        extraATK = value;
        //        Atk = Atk + extraATK;
        //    }
        //}

        //private bool extraATKVisibility;

        //public bool ExtraATKVisibility
        //{
        //    get { return extraATKVisibility; }
        //    set { extraATKVisibility = value; }
        //}

        //躲闪攻击
        private bool avoidingAttack;

        //躲闪攻击有效时间
        int avoidingAttackTime;

        public int AvoidingAttackTime
        {
            get { return avoidingAttackTime; }
            set { avoidingAttackTime = value; }
        }

        DispatcherTimer tmrAvoidingAttackTime = new DispatcherTimer();

        public bool AvoidingAttack
        {
            get { return avoidingAttack; }
            set { 
                avoidingAttack = value;
                if (avoidingAttack)
                {
                    //闪避启动，开始倒计时
                    avoidingAttackTime = 30;
                    tmrAvoidingAttackTime = new DispatcherTimer();
                    tmrAvoidingAttackTime.Tick += tmrAvoidingAttack_Tick;
                    tmrAvoidingAttackTime.Interval = TimeSpan.FromSeconds(1);
                    tmrAvoidingAttackTime.IsEnabled = true;
                }
                if (!avoidingAttack)
                {
                    //时间过期,开启CD
                    tmrAvoidingAttackTime.IsEnabled = false;
                    avoidingAttackCD = (int)(20 + 0.15 * Armor);
                    tmrAvoidAttackCD = new DispatcherTimer();
                    tmrAvoidAttackCD.Interval = TimeSpan.FromSeconds(1);
                    tmrAvoidAttackCD.Tick += tmrAvoidAttackCD_Tick;
                    tmrAvoidAttackCD.IsEnabled = true;
                }
            }
        }
        
        void tmrAvoidingAttack_Tick(object sender, EventArgs e)
        {
            avoidingAttackTime--;
            if (avoidingAttackTime <= 0)
            {
                //技能过期
                tmrAvoidingAttackTime.IsEnabled = false;
                tmrAvoidingAttackTime.Tick -= tmrExtraArmorCD_Tick;
                AvoidingAttack = false; 
            }
        }

        //躲闪攻击CD
        DispatcherTimer tmrAvoidAttackCD = new DispatcherTimer();
        bool avoidingAttackAvailable = false;
        int avoidingAttackCD;

        public bool AvoidingAttackAvailable
        {
            get { return avoidingAttackAvailable; }
            set { avoidingAttackAvailable = value; }
        }

        public int AvoidAttackCD
        {
            get { return avoidingAttackCD; }
            set { avoidingAttackCD = value; }
        }

        void tmrAvoidAttackCD_Tick(object sender, EventArgs e)
        {
            avoidingAttackCD--;
            if (avoidingAttackCD <= 0)
            {
                //CD完成，技能可使用
                avoidingAttackAvailable = true;
                tmrAvoidAttackCD.IsEnabled = false;
                tmrAvoidAttackCD.Tick -= tmrAvoidAttackCD_Tick;
            }
        }


        //一次猛烈进攻
        int violentAttacksCD;
        private DispatcherTimer tmrViolentAttacksCD = new DispatcherTimer();

        public int ViolentAttacksCD
        {
            get { return violentAttacksCD; }
            set { violentAttacksCD = value; }
        }

        private bool violentAttacksAvailable;

        public bool ViolentAttacksAvailable
        {
            get { return violentAttacksAvailable; }
            set { 
                violentAttacksAvailable = value;
                if (!violentAttacksAvailable)
                {
                    violentAttacksCD = (int)(10 + 0.1 * Atk);
                    tmrViolentAttacksCD = new DispatcherTimer();
                    tmrViolentAttacksCD.Interval = TimeSpan.FromSeconds(1);
                    tmrViolentAttacksCD.Tick += new EventHandler(tmrViolentAttacksCD_Tick);
                    tmrViolentAttacksCD.IsEnabled = true;
                }
            }
        }

        void tmrViolentAttacksCD_Tick(object sender, EventArgs e)
        {
            violentAttacksCD--;
            if (violentAttacksCD <= 0)
            {
                ViolentAttacksAvailable = true;
                tmrViolentAttacksCD.IsEnabled = false;
                tmrViolentAttacksCD.Tick -= tmrViolentAttacksCD_Tick;
            }
        }
    }
}
