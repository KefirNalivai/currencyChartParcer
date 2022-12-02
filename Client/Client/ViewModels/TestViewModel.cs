using PrismMVVMTestProject.Models;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Input;
using Prism.Commands;
using System.Windows;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace PrismMVVMTestProject.ViewModels
{
    class TestViewModel : BindableBase
    {
        public class data
        {
            public int Cur_ID { get; set; }
            public DateTime Date { get; set; }
            public Double Cur_OfficialRate { get; set; }
        }
        private ObservableCollection<Person> _persons;

        public ObservableCollection<Person> Persons
        {
            get { return _persons; }
            set { _persons = value; }
        }


        private Person _sperson;

        public Person SPerson
        {
            get {
                if (_sperson == null)
                {
                    if (string.IsNullOrEmpty(Properties.Settings.Default.Value))
                    {
                        _sperson = Persons[0];
                    }
                    else
                    {
                        for (int i = 0; i < Persons.Count; i++)
                        {
                            if (Persons[i].Name == Properties.Settings.Default.Value)
                            {
                                _sperson = Persons[i];
                                break;
                            }
                        }
                    }


                }
                return _sperson;
            }
            set {
                Properties.Settings.Default.Value = value.Name;
                Properties.Settings.Default.Save();
                _sperson = value;
            }
        }



        private Nullable<DateTime> myDateTimeProperty = null;
        public Nullable<DateTime> MyDateTimeProperty
        {
            get
            {

                if (myDateTimeProperty == null)
                {
                    if (Properties.Settings.Default.date1.Year <= 1970)
                    {
                        myDateTimeProperty = DateTime.Now;
                    }
                    else
                    {
                        myDateTimeProperty = Properties.Settings.Default.date1;
                    }
                }
                return myDateTimeProperty;
            }
            set
            {
               
                if (myDateTimeProperty > myDateTimeProperty2)
                {
                    myDateTimeProperty = myDateTimeProperty2;
                   
                }
                else
                {
                    myDateTimeProperty = value;
                    
                }
                Properties.Settings.Default.date1 = (DateTime)myDateTimeProperty;
                Properties.Settings.Default.Save();


            }
        }

        private Nullable<DateTime> myDateTimeProperty2 = null;
        public Nullable<DateTime> MyDateTimeProperty2
        {
            get
            {
                if (myDateTimeProperty2 == null)
                {
                    if (Properties.Settings.Default.date2.Year <= 1970)
                    {
                        myDateTimeProperty2 = DateTime.Now;
                    }
                    else
                    {
                        myDateTimeProperty2 = Properties.Settings.Default.date2;
                    }
                }
                return myDateTimeProperty2;
            }
            set
            {
                
                if (myDateTimeProperty > myDateTimeProperty2)
                {
                    myDateTimeProperty2 = myDateTimeProperty;
                }
                else
                {
                    myDateTimeProperty2 = value;
                }
                Properties.Settings.Default.date1 = (DateTime)myDateTimeProperty2;
                Properties.Settings.Default.Save();

            }
        }






        private Nullable<DateTime> displayEnd = DateTime.Today;
        public Nullable<DateTime> DisplayEnd
        {
            get
            {               
                return displayEnd;
            }
            set
            { }
        }

        private Nullable<DateTime> displayStart = DateTime.Now.AddYears(-5);
        public Nullable<DateTime> DisplayStart
        {
            get
            {               
                return displayStart;
            }
            set
            { }
        }


        private Nullable<WindowState> _curWindowState = null;
        public WindowState CurWindowState
        {
            get
            {
                if (_curWindowState == null)
                {
                    if (Properties.Settings.Default.winstate == "Normal")
                    {
                        _curWindowState = WindowState.Normal;
                    }
                    else
                    {
                        _curWindowState = WindowState.Maximized;
                    }
                        
                }

                return (WindowState)_curWindowState;
            }
            set
            {
                Properties.Settings.Default.winstate = value.ToString();
                Properties.Settings.Default.Save();
                _curWindowState = value;
                base.OnPropertyChanged("CurWindowState");
            }
        }

        private bool _butEnable = true;
        public bool ButEnable
        {
            get
            {
                

                return _butEnable;
            }
            set
            {
               
                _butEnable = value;
                base.OnPropertyChanged("ButEnable");
            }
        }

        private TestModel testModel = new TestModel();


        public ICommand ClickCommand { get; private set; }

        public TestViewModel()
        {
            Persons = new ObservableCollection<Person>() {
            new Person(){Id=145, Name="USD"}
            ,new Person(){Id=292 , Name="EUR"}
            ,new Person(){Id=298 , Name="RUB"}
            ,new Person(){Id=1 , Name="BTC"}};

            ClickCommand = new DelegateCommand(ClickedMethod);

           
        


        }
        public TestModel TestModel
        {
            get { return testModel; }
            set { SetProperty(ref testModel, value); }
        }

        private WindowSettings _windowSettings = new WindowSettings() { Width = Properties.Settings.Default.width, Height = Properties.Settings.Default.heig, Left = Properties.Settings.Default.left, Top = Properties.Settings.Default.top, Width2 = Properties.Settings.Default.width - 140 };
        public WindowSettings WindowSettings
        {
            get
            {

                Properties.Settings.Default.width = _windowSettings.Width;
                Properties.Settings.Default.heig = _windowSettings.Height;
                Properties.Settings.Default.top = _windowSettings.Top;
                Properties.Settings.Default.left = _windowSettings.Left;
                Properties.Settings.Default.Save();
                
                return _windowSettings;
            }
            set
            {

                SetProperty(ref _windowSettings, value);
            }

        }
        private async void ClickedMethod()
        {


            
            ButEnable = false;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {



                bool ch = false;

                await Task.Run(() =>
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:5001/api/values/" + SPerson.Id.ToString() + "&" + MyDateTimeProperty.Value.Year.ToString() + "-" + MyDateTimeProperty.Value.Month.ToString() + "-" + MyDateTimeProperty.Value.Day.ToString() + "&" + MyDateTimeProperty2.Value.Year.ToString() + "-" + MyDateTimeProperty2.Value.Month.ToString() + "-" + MyDateTimeProperty2.Value.Day.ToString());
                        request.Timeout = 5000;
                        request.Credentials = CredentialCache.DefaultNetworkCredentials;
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                            ch = true;
                        else
                            ch = false;
                    }
                    catch
                    {
                        ch = false;
                    }
                });


                if (ch)
                {
                    var responce = await client.GetAsync("https://localhost:5001/api/values/" + SPerson.Id.ToString() + "&" + MyDateTimeProperty.Value.Year.ToString() + "-" + MyDateTimeProperty.Value.Month.ToString() + "-" + MyDateTimeProperty.Value.Day.ToString() + "&" + MyDateTimeProperty2.Value.Year.ToString() + "-" + MyDateTimeProperty2.Value.Month.ToString() + "-" + MyDateTimeProperty2.Value.Day.ToString());

                    responce.EnsureSuccessStatusCode();

                    if (responce.IsSuccessStatusCode)
                    {
                        string message = await responce.Content.ReadAsStringAsync();
                        string output = JsonConvert.SerializeObject(message);
                        Console.WriteLine(message.Substring(1, message.Length - 2));

                        List<data> m = JsonConvert.DeserializeObject<List<data>>(message);
                        if (m.Count != 0)
                        {
                            List<Keyvalue> tempList = new List<Keyvalue>();

                            int chislodob = m.Count / 30;
                            if (chislodob > 1)
                            {
                                WindowSettings = new WindowSettings() { Width = _windowSettings.Width, Height = _windowSettings.Height, Left = _windowSettings.Left, Top = _windowSettings.Top, Width2 = _windowSettings.Width + 1650 * chislodob };
                            }
                            else
                            {
                                WindowSettings = new WindowSettings() { Width = _windowSettings.Width, Height = _windowSettings.Height, Left = _windowSettings.Left, Top = _windowSettings.Top, Width2 = 1550 };
                            }

                            for (int i = 0; i < m.Count; i++)
                                tempList.Add(new Keyvalue() { Key = m[i].Date.ToShortDateString(), Value = m[i].Cur_OfficialRate });
                            TestModel.DataList = tempList;
                        }
                        else
                        {
                            string messageBoxText = "За данный временной промежуток данных нет!";


                            string caption = "Внимание";
                            MessageBoxButton button = MessageBoxButton.OK;
                            MessageBoxImage icon = MessageBoxImage.Information;
                            MessageBoxResult result;

                            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                        }
                    }
                    else
                    {
                        string messageBoxText = "Сбой в подключении";


                        string caption = "Ошибка";
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Warning;
                        MessageBoxResult result;

                        result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                    }
                }
                else
                {
                    string messageBoxText = "Не удалось подключиться к серверу. Проверьте его работу";


                    string caption = "Ошибка";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result;

                    result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                }
            }
            ButEnable = true;







        }

        

    }
}

