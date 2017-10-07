using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SMAutoTracker
{
    public class Tracker : INotifyPropertyChanged
    {
        private int _missiles;
        public int Missiles { get { return _missiles; } set { _missiles = value; OnPropertyChanged(); OnPropertyChanged("HaveMissiles"); } }
        public bool HaveMissiles {  get { return _missiles > 0;  } }

        private int _supers;
        public int Supers { get { return _supers; } set { _supers = value; OnPropertyChanged(); OnPropertyChanged("HaveSupers"); } }
        public bool HaveSupers { get { return _supers > 0; } }

        private int _pbs;
        public int PBs { get { return _pbs; } set { _pbs = value; OnPropertyChanged(); OnPropertyChanged("HavePBs"); } }
        public bool HavePBs { get { return _pbs > 0; } }

        private int _etanks;
        public int ETanks { get { return _etanks; } set { _etanks = value; OnPropertyChanged(); OnPropertyChanged("HaveETanks"); } }
        public bool HaveETanks { get { return _etanks > 0; } }

        private int _reserves;
        public int Reserves { get { return _reserves; } set { _reserves = value; OnPropertyChanged(); OnPropertyChanged("HaveReserves"); } }
        public bool HaveReserves { get { return _reserves > 0; } }

        private bool _morph;
        public bool Morph { get { return _morph; } set { _morph = value; OnPropertyChanged(); } }

        private bool _bombs;
        public bool Bombs { get { return _bombs; } set { _bombs = value; OnPropertyChanged(); } }

        private bool _springBall;
        public bool SpringBall { get { return _springBall; } set { _springBall = value; OnPropertyChanged(); } }

        private bool _hiJump;
        public bool HiJump { get { return _hiJump; } set { _hiJump = value; OnPropertyChanged(); } }

        private bool _spaceJump;
        public bool SpaceJump { get { return _spaceJump; } set { _spaceJump = value; OnPropertyChanged(); } }

        private bool _varia;
        public bool Varia { get { return _varia; } set { _varia = value; OnPropertyChanged(); } }

        private bool _gravity;
        public bool Gravity { get { return _gravity; } set { _gravity = value; OnPropertyChanged(); } }

        private bool _xray;
        public bool XRay { get { return _xray; } set { _xray = value; OnPropertyChanged(); } }

        private bool _grapple;
        public bool Grapple { get { return _grapple; } set { _grapple = value; OnPropertyChanged(); } }

        private bool _speed;
        public bool Speed { get { return _speed; } set { _speed = value; OnPropertyChanged(); } }

        private bool _screw;
        public bool ScrewAttack { get { return _screw; } set { _screw = value; OnPropertyChanged(); } }

        private bool _charge;
        public bool Charge { get { return _charge; } set { _charge = value; OnPropertyChanged(); } }

        private bool _ice;
        public bool Ice { get { return _ice; } set { _ice = value; OnPropertyChanged(); } }

        private bool _wave;
        public bool Wave { get { return _wave; } set { _wave = value; OnPropertyChanged(); } }

        private bool _spazer;
        public bool Spazer { get { return _spazer; } set { _spazer = value; OnPropertyChanged(); } }

        private bool _plasma;
        public bool Plasma { get { return _plasma; } set { _plasma = value; OnPropertyChanged(); } }

        private bool _kraid;
        public bool Kraid { get { return _kraid; } set { _kraid = value; OnPropertyChanged(); } }

        private bool _phantoon;
        public bool Phantoon { get { return _phantoon; } set { _phantoon = value; OnPropertyChanged(); } }

        private bool _draygon;
        public bool Draygon { get { return _draygon; } set { _draygon = value; OnPropertyChanged(); } }

        private bool _ridley;
        public bool Ridley { get { return _ridley; } set { _ridley = value; OnPropertyChanged(); } }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }

        public Tracker()
        {
        }

        public void UpdateItems(int itemBits)
        {
            Varia = ((itemBits & 0x01) != 0);
            SpringBall = ((itemBits & 0x02) != 0);
            Morph = ((itemBits & 0x04) != 0);
            ScrewAttack = ((itemBits & 0x08) != 0);
            Gravity = ((itemBits & 0x20) != 0);

            HiJump = ((itemBits & 0x0100) != 0);
            SpaceJump = ((itemBits & 0x0200) != 0);
            Bombs = ((itemBits & 0x1000) != 0);
            Speed = ((itemBits & 0x2000) != 0);
            Grapple = ((itemBits & 0x4000) != 0);
            XRay = ((itemBits & 0x8000) != 0);
        }

        public void UpdateBeams(int beamBits)
        {
            Wave = ((beamBits & 0x0001) != 0);
            Ice = ((beamBits & 0x0002) != 0);
            Spazer = ((beamBits & 0x0004) != 0);
            Plasma = ((beamBits & 0x0008) != 0);

            Charge = ((beamBits & 0x1000) != 0);
        }

        public void UpdateBosses(int brinstar, int norfair, int ws, int maridia)
        {
            Kraid = ((brinstar & 0x01) != 0);
            Ridley = ((norfair & 0x01) != 0);
            Phantoon = ((ws & 0x01) != 0);
            Draygon = ((maridia & 0x01) != 0);
        }
    }


    [ValueConversion(typeof(bool), typeof(double))]
    public class BooleanToOpacityConverter : IValueConverter
    {
        #region Fields

        private bool _not = false;

        #endregion

        public bool Not { get { return _not; } set { _not = value; } }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (Not)
            {
                if (!(bool)value)
                {
                    return 1;
                }
                else
                {
                    double val = 0.65;

                    if (parameter != null)
                    {
                        double.TryParse(parameter.ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out val);
                    }

                    return val;
                }
            }
            else
            {
                if ((bool)value)
                {
                    return 1;
                }
                else
                {
                    double val = 0.65;

                    if (parameter != null)
                    {
                        double.TryParse(parameter.ToString(), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out val);
                    }

                    return val;
                }
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
