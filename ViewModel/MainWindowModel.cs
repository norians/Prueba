using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TiempoPueblosUniversal.Model;
using Windows.UI.Popups;

namespace TiempoPueblosUniversal.ViewModel
{
    class MainWindowModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MainWindowModel()
        {
            ListaPueblos = new ObservableCollection<PuebloAux>();

            LeerJson();

        }

        private ObservableCollection<PuebloAux> listaPueblos;

        public ObservableCollection<PuebloAux> ListaPueblos
        {
            get { return listaPueblos; }
            set
            {
                listaPueblos = value;
                OnPropertyChanged("ListaPueblos");
            }
        }


        private async void LeerJson()
        {
            var client = new System.Net.Http.HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024;

            string URLData = "https://docs.google.com/spreadsheets/d/1U_HLDlJXrbxMThe4GVwRb4u2UAzFYnYLRePWKhYadM8/gviz/tq";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(new Uri(URLData));

            if (response.IsSuccessStatusCode)
            {
                var data1 = response.Content.ReadAsStringAsync();


                int indice = (data1.Result.ToString()).IndexOf("{");

                string cadena = (data1.Result.ToString()).Remove(0, indice);

                string cadenaResult = cadena.Substring(0, cadena.Length - 2);

                var pueblodata = JsonConvert.DeserializeObject<RootObjectPueblo>(cadenaResult);



                int c = ((RootObjectPueblo)pueblodata).table.rows.Count();

                for (int i = 0; i <= c - 1; i++)
                {
                    PuebloAux T = new PuebloAux();


                    T.pueblo = ((RootObjectPueblo)pueblodata).table.rows[i].c[1].v.ToString();

                    ListaPueblos.Add(T);
                }

            }

        }

        private async void LeerJsonTiempo(string url)
        {
            var client = new System.Net.Http.HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024;

            string URLData = url;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(new Uri(URLData));

            if (response.IsSuccessStatusCode)
            {
                var data1 = response.Content.ReadAsStringAsync();


                int indice = (data1.Result.ToString()).IndexOf("{");

                string cadena = (data1.Result.ToString()).Remove(0, indice);

                string cadenaResult = cadena.Substring(0, cadena.Length);

                var diadata = JsonConvert.DeserializeObject<RootObjectTiempo>(cadenaResult);

                if (((RootObjectTiempo)diadata).list.Count == 0)
                {
                    var dialog = new MessageDialog("No hay datos para " + PuebloSeleccionado.pueblo);
                    await dialog.ShowAsync();
                    Temp = "";
                    Temp_max = "";
                    Temp_min = "";
                    Lon = "";
                    Lat = "";
                    Viento = "";
                    Ico = "";
                }
                else
                {
                    Temp = ((RootObjectTiempo)diadata).list[0].main.temp.ToString();
                    Temp_max = ((RootObjectTiempo)diadata).list[0].main.temp_max.ToString();
                    Temp_min = ((RootObjectTiempo)diadata).list[0].main.temp_min.ToString();
                    Lon = ((RootObjectTiempo)diadata).list[0].coord.lon.ToString();
                    Lat = ((RootObjectTiempo)diadata).list[0].coord.lat.ToString();
                    Viento = ((RootObjectTiempo)diadata).list[0].wind.speed.ToString();
                    Ico = "http://openweathermap.org/img/wn/" + ((RootObjectTiempo)diadata).list[0].weather[0].icon + ".png";
                }


            }
        }

        private PuebloAux puebloSeleccionado;
        public PuebloAux PuebloSeleccionado
        {
            get
            { return puebloSeleccionado; }
            set
            {
                puebloSeleccionado = value;

                if (puebloSeleccionado != null)
                {
                    Url = "http://api.openweathermap.org/data/2.5/find?&q=" + puebloSeleccionado.pueblo + ",eslang=es&units=metric&APPID=278857e8dee51f914026df21d0d40c19";
                    LeerJsonTiempo(Url);
                }
                OnPropertyChanged("PuebloSeleccionado");
            }
        }

        private string temp;
        public string Temp
        {
            get { return temp; }
            set
            {
                temp = value;
                OnPropertyChanged("Temp");

            }
        }
        private string temp_max;
        public string Temp_max
        {
            get { return temp_max; }
            set
            {
                temp_max = value;
                OnPropertyChanged("Temp_max");

            }
        }
        private string temp_min;
        public string Temp_min
        {
            get { return temp_min; }
            set
            {
                temp_min = value;
                OnPropertyChanged("Temp_min");

            }
        }
        private string lon;
        public string Lon
        {
            get { return lon; }
            set
            {
                lon = value;
                OnPropertyChanged("Lon");

            }
        }
        private string lat;
        public string Lat
        {
            get { return lat; }
            set
            {
                lat = value;
                OnPropertyChanged("Lat");

            }
        }
        private string viento;
        public string Viento
        {
            get { return viento; }
            set
            {
                viento = value;
                OnPropertyChanged("Viento");

            }
        }
        private string ico;
        public string Ico
        {
            get { return ico; }
            set
            {
                ico = value;
                OnPropertyChanged("Ico");

            }
        }
        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                OnPropertyChanged("Url");

            }
        }
    }
}
