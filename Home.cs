using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
//using System;
using Android.Hardware;
using Context = Android.Content.Context;
//using System.Linq;


namespace App3
{
    [Activity(Label = "Home", Theme = "@style/AppTheme", MainLauncher = true)]
    class Home : AppCompatActivity
    {
        protected Button Navigation;
      //  protected Button Measurement; пока нет функционала
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Home);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            Navigation = FindViewById<Button>(Resource.Id.button1);//Инициализация кнопки для показа другой активити
          //  Measurement = FindViewById<Button>(Resource.Id.button2); пока нет функционала
            Navigation.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };

            /*Measurement.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
            */
        }
    }
}