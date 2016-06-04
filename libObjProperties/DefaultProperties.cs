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
        private Border bdrSymble_AKT = new Border();

        public Border Bdr
        {
            get { return bdrSymble_AKT; }
            set { bdrSymble_AKT = value; }
        }

        private Grid gdArmor = new Grid();
        private Grid gdPH = new Grid();


        public DefaultProperties()
        {
            //bdr：攻击显示
            //gd：防御显示
            bdrSymble_AKT.Child = gdArmor;
            bdrSymble_AKT.BorderBrush = new SolidColorBrush(Colors.Green);
            gdArmor.HorizontalAlignment = HorizontalAlignment.Center;
            gdArmor.VerticalAlignment = VerticalAlignment.Center;

            bdrSymble_AKT.Background = new SolidColorBrush(Colors.Red);
            gdArmor.Background = new SolidColorBrush(Colors.Blue);
            gdPH.Background = gdArmor.Background;

            Bdr.HorizontalAlignment = HorizontalAlignment.Center;
            bdrSymble_AKT.VerticalAlignment = VerticalAlignment.Center;

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

                if (atk < 20)
                {
                    bdrSymble_AKT.Width = 20 + 20 / 2;
                    bdrSymble_AKT.Height = 20 + 20 / 2;
                    bdrSymble_AKT.Opacity = 0.3 + 20 / 100;
                    return;
                }
                if (atk > 50)
                {
                    bdrSymble_AKT.Width = 20 + 50 / 2;
                    bdrSymble_AKT.Height = 20 + 50 / 2;
                    bdrSymble_AKT.Opacity = 0.3 + 50 / 100;
                    return;
                }

                bdrSymble_AKT.Width = 20 + atk / 2;
                bdrSymble_AKT.Height = 20 + atk / 2;
                bdrSymble_AKT.Opacity = 0.3 + atk / 100;
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

                if (armor < 30)
                {
                    gdArmor.Width = 30 + 30 / 2;
                    gdArmor.Height = 30 + 30 / 2;
                    gdArmor.Opacity = 0.3 + 30 / 120;

                    gdPH.Height = gdArmor.Height;
                    gdPH.Width = gdArmor.Width;
                    return;
                }
                if (armor > 60)
                {
                    gdArmor.Width = 30 + 60 / 2;
                    gdArmor.Height = 30 + 60 / 2;
                    gdArmor.Opacity = 0.3 + 60 / 120;

                    gdPH.Height = gdArmor.Height;
                    gdPH.Width = gdArmor.Width;
                    return;
                }

                gdArmor.Width = 30 + armor / 2;
                gdArmor.Height = 30 + armor / 2;
                gdArmor.Opacity = 0.3 + armor / 120;

                gdPH.Height = gdArmor.Height;
                gdPH.Width = gdArmor.Width;
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
        private bool isAlive;

        public bool IsAlive
        {
            get { return isAlive; }
        }
    }
}
