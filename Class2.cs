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
    [Activity(Label="Class2",Theme = "@style/AppTheme", MainLauncher = true)]
    class Class2 : AppCompatActivity
    {
        protected Button back;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout2);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);//?

            back = FindViewById<Button>(Resource.Id.buttonback);//Инициализация кнопки для показа другой активити
            
            back.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);

            };
        }
    }
}