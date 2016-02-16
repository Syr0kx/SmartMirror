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
using System.Windows.Forms;
using System.Device.Location;
using System.Xml.Linq;

namespace SmartMirrow
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String latitute="0", longitute="0";
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            this._time.Start();
            debug();
        }
        Timer _time = new Timer();
        public void InitializeTimer()
        {
            this._time.Interval = 1000;
            this._time.Tick += new EventHandler(TimerTick);
        }

        void TimerTick(object sender, EventArgs e)
        {
            lbl_uhr.Content = string.Format("{0:00}:{1:00}:{2:00}",
                                                  DateTime.Now.Hour,
                              DateTime.Now.Minute,
                              DateTime.Now.Second);
        }
       
        public void debug()
        {
            GetLocationProperty();
            int displayheight, displaywidth;
            displayheight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            displaywidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int ggt = berechneGgt(displayheight, displaywidth);
            label_resolution.Content = "Auflösung " + displaywidth + ":" + displayheight + "\n" + "Bildverhältnis " + displaywidth / ggt + "/" + displayheight / ggt;



        }
        public int berechneGgt(int _zahl1, int _zahl2)
        {
            int zahl1 = _zahl1;
            int zahl2 = _zahl2;
            //Diese Variable wird bei Wertzuweisungen zwischen den Zahlen benutzt
            int temp = 0;
            //Der Rückgabewert zweier gegebener Zahlen.
            int ggt = 0;//Solange der Modulo der zwei zahlen nicht 0 ist,
                        //werden Zuweisungen entsprechend demEuklidischen Algorithmus ausgeführt.
            while (zahl1 % zahl2 != 0)
            {
                temp = zahl1 % zahl2;
                zahl1 = zahl2;
                zahl2 = temp;
            }

            ggt = zahl2;

            return ggt;
        }
        public async void GetLocationProperty()
        {
            await Task.Run(() => System.Threading.Thread.Sleep(20000));
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {
               
                label_resolution.Content += "\nLatitude: "+coord.Latitude + "\nLongitude" + coord.Longitude;
                latitute = coord.Latitude.ToString();
                longitute = coord.Longitude.ToString();

            }
            else
            {
                label_resolution.Content += "\nLatitude: " + "could not be resolved" + "\nLongitude" + "could not be resolved";
            }
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        public void calcularRota(string latitude, string longitude)
        {
            //URL do distancematrix - adicionando endereco de origem e destino
            string url = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false", latitude, longitude);
            XElement xml = XElement.Load(url);

            // verifica se o status é ok
            if (xml.Element("status").Value == "OK")
            {
                //Formatar a resposta
                ////Label3.Text = string.Format("<strong>Origem</strong>: {0}",
                    //Pegar endereço de origem 
                    ////xml.Element("result").Element("formatted_address").Value);
                //Pegar endereço de destino                    
            }
            else
            {
                ////Label3.Text = String.Concat("Ocorreu o seguinte erro: ", xml.Element("status").Value);
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.calcularRota(latitute, longitute);
        }
    }
}
