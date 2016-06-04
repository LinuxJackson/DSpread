using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Controls;

namespace DSpread.MainWindow_Animation
{
    internal static class MainWindowStarting
    {
        internal static void ShowOpacityAutoReverse(Label lbl)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = TimeSpan.FromSeconds(2);
            da.From = 0;
            da.To = 1;
            da.AccelerationRatio = 0.1;
            da.DecelerationRatio = 0.9;
            da.AutoReverse = true;
            lbl.BeginAnimation(Label.OpacityProperty, da);
        }

        internal static void ShowOpacity(Grid gd, bool UpOrDown)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = TimeSpan.FromSeconds(2);
            da.From = gd.Opacity;
            if (UpOrDown)
                da.To = 1;
            else
                da.To = 0;
            da.AccelerationRatio = 0.1;
            da.DecelerationRatio = 0.9;
            gd.BeginAnimation(Label.OpacityProperty, da);
        }
    }
}
