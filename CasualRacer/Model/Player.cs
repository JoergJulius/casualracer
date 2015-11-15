using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CasualRacer.Model
{
    internal class Player : INotifyPropertyChanged
    {
        private float direction = 0f;
        public float Direction
        {
            get
            {
                return direction;
            }
            set
            {
                if( direction!=value )
                {
                    direction = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Direction"));
                }
            }
        }

        private Vector position = new Vector();
        public Vector Position
        {
            get
            {
                return position;
            }
            set
            {
                if( position!=value )
                {
                    position = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Position"));
                }
            }
        }

        public bool Acceleration
        {
            get; set;
        }
        public bool WheelLeft
        {
            get; set;
        }
        public bool WheelRight
        {
            get; set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Player()
        {
        }

        public void Update(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            if( WheelLeft )
                Direction -= (float)elapsedTime.TotalSeconds * 1000000;
            if( WheelRight )
                Direction += (float)elapsedTime.TotalSeconds * 1000000;
        }
    }
}
