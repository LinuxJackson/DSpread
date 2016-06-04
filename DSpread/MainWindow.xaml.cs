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

            foreach (UIElement ui in gdEnemyObjects.Children)
            {
                Border bdr = ui as Border;
                if (bdr != null)
                {
                    bdr.Tag = false;
                }
            }
            foreach (UIElement ui in gdUserObjects.Children)
            {
                Border bdr = ui as Border;
                if (bdr != null)
                {
                    bdr.Tag = false;
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
            lblObjectCount.Content = "x" + GameData.UserObjectCount;
        }

        private void AddObject(libFlags.TeamSign.Teams team)
        {
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

            if (!(bool)bdr.Tag)
            {
                bdr.Tag = true;
                bdr.BorderBrush = new SolidColorBrush(Colors.Blue);
                bool isInList = false;
                foreach (DSObject ds in GameData.arrSelectedUserObjects)
                {
                    if (ds.ObjectProperties.Bdr == bdrObject)
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
                foreach (DSObject ds in GameData.arrSelectedUserObjects)
                {
                    if (ds.ObjectProperties.Bdr == bdrObject)
                        continue;
                    arrNew.Add(ds);
                }
                GameData.arrSelectedUserObjects = arrNew;

                bdr.Tag = false;
                bdr.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }

        private void Border_MouseDown_Enemy(object sender, MouseButtonEventArgs e)
        {
            Border bdr = (Border)sender;
            if (bdr.Child == null)
                return;

            Border bdrObject = (Border)bdr.Child;

            if (!(bool)bdr.Tag)
            {
                bdr.Tag = true;
                bdr.BorderBrush = new SolidColorBrush(Colors.Red);
                bool isInList = false;
                foreach (DSObject ds in GameData.arrEnemyObjects)
                {
                    if (ds.ObjectProperties.Bdr == bdrObject)
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
                foreach (DSObject ds in GameData.arrEnemyObjects)
                {
                    if (ds.ObjectProperties.Bdr == bdrObject)
                        continue;
                    arrNew.Add(ds);
                }
                GameData.arrEnemyObjects = arrNew;

                bdr.Tag = false;
                bdr.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
