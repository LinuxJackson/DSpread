using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using libObjProperties;
using System.Windows.Threading;
using DSpread.MainWindow_Animation;
using libData_Memory;
using ObjectControls;
using libFlags;

namespace DSpread
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        DispatcherTimer tmrStarting = new DispatcherTimer();
        int tmrStarting_Count = 0;

        private void gdMain_Loaded(object sender, RoutedEventArgs e)
        {
            gdControl.Opacity = 0;
            gdControl.IsEnabled = false;

            gdObjectCount.Opacity = 0;
            gdObjectCount.IsEnabled = false;

            gdUserObjects.Opacity = 0;
            gdEnemyObjects.Opacity = 0;

            lblNotice.Opacity = 0;

            tmrStarting.Interval = TimeSpan.FromSeconds(1);
            tmrStarting.Tick += tmrStarting_Tick;
            tmrStarting.IsEnabled = true;

            tmrCDShower = new DispatcherTimer();
            tmrCDShower.Interval = TimeSpan.FromSeconds(0.1);
            tmrCDShower.Tick += TmrCDShower_Tick;
            tmrCDShower.IsEnabled = true;

            foreach (UIElement ui in gdEnemyObjects.Children)
            {
                Border bdr = ui as Border;
                if (bdr != null)
                {
                    bdr.Tag = false;
                    bdr.BorderThickness = new Thickness(2);
                }
            }
            foreach (UIElement ui in gdUserObjects.Children)
            {
                Border bdr = ui as Border;
                if (bdr != null)
                {
                    bdr.Tag = false;
                    bdr.BorderThickness = new Thickness(2);
                }
            }
        }

        void tmrStarting_Tick(object sender, EventArgs e)
        {
            tmrStarting_Count++;

            switch (tmrStarting_Count)
            {
                case 1:
                    MainWindowStarting.ShowOpacityAutoReverse(lblNotice);
                    break;
                case 5:
                    lblNotice.Content = "优先放置你的方块";
                    MainWindowStarting.ShowOpacityAutoReverse(lblNotice);
                    MainWindowStarting.ShowOpacity(gdObjectCount, true);
                    MainWindowStarting.ShowOpacity(gdUserObjects, true);
                    gdUserObjects.IsEnabled = true;
                    gdObjectCount.IsEnabled = true;
                    break;
                case 12:
                    lblNotice.Content = "10秒后战斗开始";
                    MainWindowStarting.ShowOpacityAutoReverse(lblNotice);
                    break;
                case 22:
                    lblNotice.Content = "战斗开始";
                    MainWindowStarting.ShowOpacityAutoReverse(lblNotice);
                    break;
                case 26:
                    MainWindowStarting.ShowOpacity(gdEnemyObjects, true);
                    MainWindowStarting.ShowOpacity(gdControl, true);
                    gdEnemyObjects.IsEnabled = true;
                    gdControl.IsEnabled = true;
                    tmrStarting.IsEnabled = false;
                    GameState.IsBegun = true;
                    break;
            }
        }

        private void wdMain_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void bdrObjectCount_MouseEnter(object sender, MouseEventArgs e)
        {
            Border bdr = (Border)sender;
            bdr.Background = new SolidColorBrush(Colors.Green);
        }

        private void bdrObjectCount_MouseLeave(object sender, MouseEventArgs e)
        {
            Border bdr = (Border)sender;
            bdr.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private void bdrObjectCount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddObject(TeamSign.Teams.User);
            if (GameData.UserObjectCount > 0)
                lblObjectCount.Content = "x" + GameData.UserObjectCount;
            else
                lblObjectCount.Content = "Add";

        }

        private void AddObject(libFlags.TeamSign.Teams team)
        {
            if (GameData.UserObjectCount <= 0)
            {
                if (GameData.arrSelectedUserObjects.Count <= 0)
                {
                    //提示未选中
                    return;
                }
            }

            TeamSign.Teams theOtherTeam;
            if (team == TeamSign.Teams.User)
                theOtherTeam = TeamSign.Teams.Enemy;
            else
                theOtherTeam = TeamSign.Teams.User;

            Grid gdOtherSide;
            Grid gdAddSide;
            if (team == TeamSign.Teams.User)
            {
                gdOtherSide = gdEnemyObjects;
                gdAddSide = gdUserObjects;
            }
            else
            {
                gdOtherSide = gdUserObjects;
                gdAddSide = gdEnemyObjects;
            }

            List<DSObject> arrAddSide;
            if (team == TeamSign.Teams.User)
                arrAddSide = GameData.arrUserObjects;
            else
                arrAddSide = GameData.arrEnemyObjects;


            bool isAdded = false;

            int remainObjectCount;
            if (team == TeamSign.Teams.User)
                remainObjectCount = GameData.UserObjectCount;
            else
                remainObjectCount = GameData.EnemyObjectCount;
            isAdded = true;


            if (remainObjectCount != 0)
            {
                foreach (UIElement ui in gdAddSide.Children)
                {
                    Border bdr = ui as Border;
                    if (bdr != null)
                    {
                        if (bdr.Child == null)
                        {
                            DSObject objNew = new DSObject(TeamSign.Teams.User);
                            bdr.Child = objNew.ObjectProperties.Bdr;
                            arrAddSide.Add(objNew);

                            if (team == TeamSign.Teams.User)
                                GameData.UserObjectCount = GameData.UserObjectCount - 1;
                            else
                                GameData.EnemyObjectCount = GameData.EnemyObjectCount - 1;

                            isAdded = true;

                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (UIElement ui in gdAddSide.Children)
                {
                    Border bdr = ui as Border;
                    if (bdr != null)
                    {
                        if (bdr.Child == null)
                        {
                            DSObject objNew = CreateNewObject.Create(GameData.arrSelectedUserObjects, TeamSign.Teams.User);
                            bdr.Child = objNew.ObjectProperties.Bdr;
                            arrAddSide.Add(objNew);
                            isAdded = true;
                            break;
                        }
                    }
                }
            }
            if (!isAdded)
            {
                //添加失败：己方已经过载
                if (remainObjectCount != 0)
                {
                    foreach (UIElement ui in gdOtherSide.Children)
                    {
                        Border bdr = ui as Border;
                        if (bdr != null)
                        {
                            if (bdr.Child == null)
                            {
                                DSObject objNew = new DSObject(TeamSign.Teams.User);
                                bdr.Child = objNew.ObjectProperties.Bdr;
                                arrAddSide.Add(objNew);

                                if (team == TeamSign.Teams.User)
                                    GameData.UserObjectCount = GameData.UserObjectCount - 1;
                                else
                                    GameData.EnemyObjectCount = GameData.EnemyObjectCount - 1;

                                isAdded = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (UIElement ui in gdOtherSide.Children)
                    {
                        Border bdr = ui as Border;
                        if (bdr != null)
                        {
                            if (bdr.Child == null)
                            {
                                DSObject objNew = CreateNewObject.Create(GameData.arrSelectedUserObjects, TeamSign.Teams.User);
                                bdr.Child = objNew.ObjectProperties.Bdr;
                                arrAddSide.Add(objNew);
                                isAdded = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (!isAdded)
            {
                bdrObjectCount.Background = new SolidColorBrush(Colors.Gray);
            }

            //检查是否越界
            foreach (Border bdr in gdOtherSide.Children)
            {
                if (bdr.Child != null)
                {
                    Border bdrThis = (Border)bdr.Child;
                    DSObject ds = (DSObject)bdrThis.Tag;
                    if (ds.Team == team)
                    {
                        bool isOverride;
                        if (team == TeamSign.Teams.User)
                            isOverride = GameData.IsUserOverride;
                        else
                            isOverride = GameData.IsEnemyOverride;

                        if (isOverride)
                            return;
                        else
                        {
                            OverrideSet.SetOn(team);
                            if (team == TeamSign.Teams.User)
                                GameData.IsUserOverride = true;
                            else
                                GameData.IsEnemyOverride = true;
                        }
                    }
                }
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border bdr = (Border)sender;
            bdr.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Border bdr = (Border)sender;
            bdr.Background = new SolidColorBrush(Colors.White);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border bdr = (Border)sender;
            if (bdr.Child == null)
                return;

            Border bdrObject = (Border)bdr.Child;
            DSObject ds = (DSObject)bdrObject.Tag;
            if (ds.Team == TeamSign.Teams.User)
            {
                if (!(bool)bdr.Tag)
                {
                    bdr.Tag = true;
                    bdr.BorderBrush = new SolidColorBrush(Colors.Blue);
                    bool isInList = false;
                    foreach (DSObject dsInList in GameData.arrSelectedUserObjects)
                    {
                        if (dsInList.ObjectProperties.Bdr == bdrObject)
                        {
                            isInList = true;
                            break;
                        }
                    }

                    if (!isInList)
                    {
                        GameData.arrSelectedUserObjects.Add((DSObject)bdrObject.Tag);
                    }
                }
                else
                {
                    List<DSObject> arrNew = new List<DSObject>();
                    foreach (DSObject dsNew in GameData.arrSelectedUserObjects)
                    {
                        if (dsNew.ObjectProperties.Bdr == bdrObject)
                            continue;
                        arrNew.Add(dsNew);
                    }
                    GameData.arrSelectedUserObjects = arrNew;

                    bdr.Tag = false;
                    bdr.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
            else
            {
                if (!(bool)bdr.Tag)
                {
                    bdr.Tag = true;
                    bdr.BorderBrush = new SolidColorBrush(Colors.Red);
                    bool isInList = false;
                    foreach (DSObject dsInList in GameData.arrSelectedEnemiesObjects)
                    {
                        if (dsInList.ObjectProperties.Bdr == bdrObject)
                        {
                            isInList = true;
                            break;
                        }
                    }

                    if (!isInList)
                    {
                        GameData.arrSelectedEnemiesObjects.Add((DSObject)bdrObject.Tag);
                    }
                }
                else
                {
                    List<DSObject> arrNew = new List<DSObject>();
                    foreach (DSObject dsNew in GameData.arrSelectedEnemiesObjects)
                    {
                        if (dsNew.ObjectProperties.Bdr == bdrObject)
                            continue;
                        arrNew.Add(dsNew);
                    }
                    GameData.arrSelectedEnemiesObjects = arrNew;

                    bdr.Tag = false;
                    bdr.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
        }

        DispatcherTimer tmrCDShower;

        private void TmrCDShower_Tick(object sender, EventArgs e)
        {
            if (GameData.arrSelectedUserObjects.Count == 0)
            {
                gdControl.IsEnabled = false;
                lblATK.Content = "ATK: ";
                lblArmor.Content = "Armor: ";
                lblPH.Content = "[--/--]";
                lblAttack.Content = "";
                lblAvoid.Content = "";
                lblArmorEX.Content = "";
                lblCrit.Content = "";
                return;
            }
            gdControl.IsEnabled = true;

            int maxCDATK = 0;
            int maxCDAvoid = 0;
            int maxCDArmor = 0;
            int maxCDCrit = 0;

            bool CDATK = true, CDAvoid = true, CDArmor = true, CDCrit = true;

            double averATK = 0;
            double averArmor = 0;
            double averActualArmor = 0;
            double PH = 0;

            foreach (DSObject ds in GameData.arrSelectedUserObjects)
            {
                averATK = ds.ObjectProperties.Atk + averATK;
                averArmor = ds.ObjectProperties.Armor + averArmor;
                averActualArmor = ds.ObjectProperties.ActualArmor + averActualArmor;
                PH = ds.ObjectProperties.Ph + PH;
                pbrPH.Value = 0;

                bdrAttack.IsEnabled = true;
                bdrAvoid.IsEnabled = true;
                bdrExtraArmor.IsEnabled = true;
                bdrViolatedAttack.IsEnabled = true;

                if (!ds.ObjectProperties.AttacksAvailable)
                {
                    CDATK = false;
                    bdrAttack.IsEnabled = false;
                    lblAttack.Content = "";
                    if (ds.ObjectProperties.AttacksCD > maxCDATK)
                    {
                        maxCDATK = ds.ObjectProperties.AttacksCD;
                        lblAttack.Content = maxCDATK;
                        lblAttack.FontSize = 20;
                        lblAttack.HorizontalAlignment = HorizontalAlignment.Center;
                        lblAttack.VerticalAlignment = VerticalAlignment.Center;
                        lblAttack.Foreground = new SolidColorBrush(Colors.Pink);
                    }
                }
                if (!ds.ObjectProperties.AvoidingAttackAvailable)
                {
                    CDAvoid = false;
                    bdrAvoid.IsEnabled = false;
                    lblAvoid.Content = "";
                    if (ds.ObjectProperties.AvoidAttackCD > maxCDAvoid)
                    {
                        maxCDAvoid = ds.ObjectProperties.AvoidAttackCD;
                        lblAvoid.Content = maxCDAvoid;
                        lblAvoid.FontSize = 20;
                        lblAvoid.HorizontalAlignment = HorizontalAlignment.Center;
                        lblAvoid.VerticalAlignment = VerticalAlignment.Center;
                        lblAvoid.Foreground = new SolidColorBrush(Colors.Pink);
                    }
                }
                if (!ds.ObjectProperties.ExtraArmorAvailable)
                {
                    CDArmor = false;
                    bdrExtraArmor.IsEnabled = false;
                    lblArmorEX.Content = "";
                    if (ds.ObjectProperties.ExtraArmorCD > maxCDArmor)
                    {
                        maxCDArmor = ds.ObjectProperties.ExtraArmorCD;
                        lblArmorEX.Content = maxCDArmor;
                        lblArmorEX.FontSize = 20;
                        lblArmorEX.HorizontalAlignment = HorizontalAlignment.Center;
                        lblArmorEX.VerticalAlignment = VerticalAlignment.Center;
                        lblArmorEX.Foreground = new SolidColorBrush(Colors.Pink);
                    }
                }
                if (!ds.ObjectProperties.ViolentAttacksAvailable)
                {
                    CDCrit = false;
                    bdrViolatedAttack.IsEnabled = false;
                    lblArmorEX.Content = "";
                    if (ds.ObjectProperties.ViolentAttacksCD > maxCDCrit)
                    {
                        maxCDCrit = ds.ObjectProperties.ViolentAttacksCD;
                        lblCrit.Content = maxCDCrit;
                        lblCrit.FontSize = 20;
                        lblCrit.HorizontalAlignment = HorizontalAlignment.Center;
                        lblCrit.VerticalAlignment = VerticalAlignment.Center;
                        lblCrit.Foreground = new SolidColorBrush(Colors.Pink);
                    }
                }
            }
            if (CDATK)
            {
                bdrAttack.IsEnabled = true;
                lblAttack.Content = "Attack";
                lblAttack.FontSize = 10;
                lblAttack.HorizontalAlignment = HorizontalAlignment.Left;
                lblAttack.VerticalAlignment = VerticalAlignment.Top;
                lblAttack.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (CDAvoid)
            {
                bdrAvoid.IsEnabled = true;
                lblAvoid.Content = "Avoid";
                lblAvoid.FontSize = 10;
                lblAvoid.HorizontalAlignment = HorizontalAlignment.Left;
                lblAvoid.VerticalAlignment = VerticalAlignment.Top;
                lblAvoid.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (CDArmor)
            {
                bdrExtraArmor.IsEnabled = true;
                lblArmorEX.Content = "Armor";
                lblArmorEX.FontSize = 10;
                lblArmorEX.HorizontalAlignment = HorizontalAlignment.Left;
                lblArmorEX.VerticalAlignment = VerticalAlignment.Top;
                lblArmorEX.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (CDCrit)
            {
                bdrViolatedAttack.IsEnabled = true;
                lblCrit.Content = "Crit";
                lblCrit.FontSize = 10;
                lblCrit.HorizontalAlignment = HorizontalAlignment.Left;
                lblCrit.VerticalAlignment = VerticalAlignment.Top;
                lblCrit.Foreground = new SolidColorBrush(Colors.Black);
            }

            //averATK = averATK / GameData.arrSelectedUserObjects.Count;
            //averArmor = averArmor / GameData.arrSelectedUserObjects.Count;
            //averActualArmor = averActualArmor / GameData.arrSelectedUserObjects.Count;

            this.lblATK.Content = "ATK: " + (int)averATK;
            this.lblArmor.Content = "Armor: " + (int)averArmor + "/" + (int)averActualArmor;
            this.lblPH.Content = "[" + (int)PH + "/" + (int)(DefaultParameters.PH * GameData.arrSelectedUserObjects.Count) + "]";

            pbrPH.Maximum = DefaultParameters.PH * GameData.arrSelectedUserObjects.Count;
            pbrPH.Value = PH;
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void lblCancelAllSelection_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach(UIElement ui in gdUserObjects.Children)
            {
                Border bdr = ui as Border;
                if (bdr != null)
                    bdr.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            foreach (UIElement ui in gdEnemyObjects.Children)
            {
                Border bdr = ui as Border;
                if (bdr != null)
                    bdr.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            GameData.arrSelectedUserObjects.Clear();
            GameData.arrSelectedEnemiesObjects.Clear();
        }

        private void bdrAttack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (GameData.arrSelectedEnemiesObjects.Count == 0 || GameData.arrSelectedUserObjects.Count == 0)
            {
                //提示未选中
                return;
            }
            Attack.AttackTarget(GameData.arrSelectedUserObjects, GameData.arrSelectedEnemiesObjects, TeamSign.Teams.User);
        }

        private void bdrAvoid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AvoidAttack.Avoid(GameData.arrSelectedUserObjects);
        }
    }
}
