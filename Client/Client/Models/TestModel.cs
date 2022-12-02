using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows;

namespace PrismMVVMTestProject.Models
{

    public class WindowSettings
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }
        public int Width2 { get; set; }

    }
    class TestModel : BindableBase
    {
        private List<Keyvalue> _DataList;
       
        private DateTime _MyDateTimeProperty;
        public DateTime MyDateTimeProperty { get { return _MyDateTimeProperty; } set { SetProperty(ref _MyDateTimeProperty, value); } }

        private DateTime _MyDateTimeProperty2;
        public DateTime MyDateTimeProperty2 { get { return _MyDateTimeProperty2; } set { SetProperty(ref _MyDateTimeProperty2, value); } }
        private DateTime _DisplayEnd = DateTime.Now;
        public DateTime DisplayEnd { get { return _DisplayEnd; }}
      
        private DateTime _DisplayStart = DateTime.Now;
        public DateTime DisplayStart { get { return _DisplayStart; } }
        
        
        public List<Keyvalue> DataList { get { return _DataList; } set { SetProperty(ref _DataList, value); } }
    }




    class Keyvalue : BindableBase
    {
        private string _Key;
        public string Key { get { return _Key; } set { SetProperty(ref _Key, value); } }

        private double _Value;
        public double Value { get { return _Value; } set { SetProperty(ref _Value, value); } }

        private double _WindowSettings;
        public double WindowSettings { get { return _WindowSettings; } set { SetProperty(ref _WindowSettings, value); } }
    }
}
