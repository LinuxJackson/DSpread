using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using libFlags;

namespace libObjProperties
{
    public class DefaultProperties
    {
        private Border bdrSymble_Armor = new Border();

        public Border Bdr
        {
            get { return bdrSymble_Armor; }
            set { bdrSymble_Armor = value; }
        }

        private Grid gdATK = new Grid();
        private Grid gdPH = new Grid();


        public DefaultProperties()
        {
            //bdr：攻击显示
            //gd：防御显示
            bdrSymble_Armor.Child = gdATK;
            bdrSymble_Armor.BorderBrush = new SolidColorBrush(Colors.Green);
            gdATK.HorizontalAlignment = HorizontalAlignment.Center;
            gdATK.VerticalAlignment = VerticalAlignment.Center;

            bdrSymble_Armor.Background = new SolidColorBrush(Colors.LightGreen);
            gdATK.Background = new SolidColorBrush(Colors.Red);
            gdPH.Background = gdATK.Background;
            bdrSymble_Armor.BorderThickness = new Thickness(2);

            Bdr.HorizontalAlignment = HorizontalAlignment.Center;
            bdrSymble_Armor.VerticalAlignment = VerticalAlignment.Center;

            tmrPHRefresh = new DispatcherTimer();
            tmrPHRefresh.Interval = TimeSpan.FromSeconds(1);
            tmrPHRefresh.Tick += tmrPHRefresh_Tick;
            tmrPHRefresh.IsEnabled = true;
        }

        void tmrPHRefresh_Tick(object sender, EventArgs e)
        {
            if (ph >= 100)
                ph = 100;
            else
            {
                ph = ph + 0.2;
            }
        }

        //攻击力
        private double atk;

        public double Atk
        {
            get { return atk; }
            set
            {
                atk = value;

                if (atk < 10)
                {
                    gdATK.Width = 10 + 10 / 2;
                    gdATK.Height = 10 + 10 / 2;
                    gdATK.Opacity = 0.1 + 10 / 100;
                    return;
                }
                if (atk > 80)
                {
                    gdATK.Width = 10 + 80 / 2;
                    gdATK.Height = 10 + 80 / 2;
                    gdATK.Opacity = 0.1 + 80 / 100;
                    return;
                }

                gdATK.Width = 10 + atk / 2;
                gdATK.Height = 10 + atk / 2;
                gdATK.Opacity = 0.1 + atk / 100;
            }
        }

        //防御力
        private double armor;

        public double Armor
        {
            get { return armor; }
            set
            {
                armor = value;

                if (ActualArmor < 15)
                {
                    bdrSymble_Armor.Width = 30 + 15 / 2;
                    bdrSymble_Armor.Height = 30 + 15 / 2;
                    bdrSymble_Armor.Opacity = 0.1 + 15 / 120;

                    gdPH.Height = bdrSymble_Armor.Height;
                    gdPH.Width = bdrSymble_Armor.Width;
                    return;
                }
                if (ActualArmor > 85)
                {
                    bdrSymble_Armor.Width = 30 + 85 / 2;
                    bdrSymble_Armor.Height = 30 + 85 / 2;
                    bdrSymble_Armor.Opacity = 0.1 + 85 / 120;

                    gdPH.Height = bdrSymble_Armor.Height;
                    gdPH.Width = bdrSymble_Armor.Width;
                    return;
                }

                bdrSymble_Armor.Width = 30 + ActualArmor / 2;
                bdrSymble_Armor.Height = 30 + ActualArmor / 2;
                bdrSymble_Armor.Opacity = 0.1 + ActualArmor / 120;

                gdPH.Height = bdrSymble_Armor.Height;
                gdPH.Width = bdrSymble_Armor.Width;
            }
        }

        //实际防御力（与血量有关）
        public double ActualArmor
        {
            get { return armor * ((ph / DefaultParameters.PH)); }
        }
        
        //血量
        private double ph = DefaultParameters.PH;

        public double Ph
        {
            get { return ph; }
            set {
                ph = value;

                gdPH.Opacity = (DefaultParameters.PH - ph) / 100;

                if (ph <= 0)
                    isAlive = false;
            }
        }

        DispatcherTimer tmrPHRefresh;

        //存活状态
        private bool isAlive = true;

        public bool IsAlive
        {
            get { return isAlive; }
        }
    }
}
