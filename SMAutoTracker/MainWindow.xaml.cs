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
using System.Windows.Threading;
using System.Configuration;
using usb2snes.core;

namespace SMAutoTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Tracker _tracker;
        private DispatcherTimer _pollTimer;
        private string _comPort;

        public MainWindow()
        {
            InitializeComponent();
            _tracker = new Tracker();
            this.DataContext = _tracker;

            var devices = core.GetDeviceList();
            if (devices.Count > 0)
            {
                _comPort = devices[0].Name;
            }
            else
            {
                _comPort = ConfigurationManager.AppSettings["COMPort"];
                if (ShowInputDialog(ref _comPort) != System.Windows.Forms.DialogResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }

            try
            {
                core.Connect(_comPort);
                if (core.Connected())
                {
                    ConfigurationManager.AppSettings["COMPort"] = _comPort;
                }
                else
                {
                    MessageBox.Show("Could not connect to usb2snes, check COM-port setting.");
                    Application.Current.Shutdown();
                }
            }
            catch
            {
                MessageBox.Show("Could not connect to usb2snes, check COM-port setting.");
                Application.Current.Shutdown();
            }

            _pollTimer = new DispatcherTimer();
            _pollTimer.Tick += _pollTimer_Tick;
            _pollTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _pollTimer.Start();

        }

        private void _pollTimer_Tick(object sender, EventArgs e)
        {
            if(core.Connected())
            {
                var data = new byte[512];
                core.SendCommand(core.usbint_server_opcode_e.USBINT_SERVER_OPCODE_VGET, core.usbint_server_space_e.USBINT_SERVER_SPACE_SNES, core.usbint_server_flags_e.USBINT_SERVER_FLAGS_NONE, Tuple.Create(0xF509A0, 64), Tuple.Create(0xF5D800, 64));
                core.GetData(data, 0, 512);

                _tracker.UpdateItems(data[0x04] + (data[0x05] << 8));
                _tracker.UpdateBeams(data[0x08] + (data[0x09] << 8));

                _tracker.Missiles = (data[0x28] + (data[0x29] << 8));
                _tracker.Supers = (data[0x2C] + (data[0x2D] << 8));
                _tracker.PBs = (data[0x30] + (data[0x31] << 8));
                _tracker.ETanks = (data[0x24] + (data[0x25] << 8)) / 100;
                _tracker.Reserves = (data[0x34] + (data[0x35] << 8)) / 100;

                _tracker.UpdateBosses(data[0x69], data[0x6A], data[0x6B], data[0x6C]);
            }
        }

        private static System.Windows.Forms.DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(300, 70);
            System.Windows.Forms.Form inputBox = new System.Windows.Forms.Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "Enter USB2Snes COM-port";

            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            System.Windows.Forms.Button okButton = new System.Windows.Forms.Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            System.Windows.Forms.Button cancelButton = new System.Windows.Forms.Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            System.Windows.Forms.DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }
    }
}
