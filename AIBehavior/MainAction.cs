using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AIBehavior
{
    public class MainAction
    {
        //注：不会写真的AI，模仿一个罢了

        DispatcherTimer tmrBehavior;

        public MainAction() 
        {
            tmrBehavior = new DispatcherTimer();
            tmrBehavior.Tick += tmrBehavior_Tick;
            tmrBehavior.Interval = TimeSpan.FromSeconds(0.2);
        }

        public void Start()
        {
            tmrBehavior.IsEnabled = true;
        }

        public void Puase()
        {
            tmrBehavior.IsEnabled = false;
        }

        void tmrBehavior_Tick(object sender, EventArgs e)
        {

        }

    }
}
