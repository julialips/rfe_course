using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Hardware;
using Context = Android.Content.Context;
using System.Linq;


namespace App3
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
     public class MainActivity : AppCompatActivity, ISensorEventListener
    {

        protected SensorManager msensorManager;//Менеджер сенсоров аппрата
        //private final Sensor mAccelerometer;
        private float[] accelData;
        protected Button start;
        protected Button stop;
        /*
        private EditText xView;
        private EditText yView;
        private EditText zView;
        */
        private TextView xView;
        private TextView yView;
        private TextView zView;
        private TextView textch;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            msensorManager = (SensorManager)GetSystemService(Context.SensorService);
            accelData = new float[3];
            xView = (TextView)FindViewById(Resource.Id.textView4);  //
            yView = (TextView)FindViewById(Resource.Id.textView5);  // текстовые поля для вывода показаний
            zView = (TextView)FindViewById(Resource.Id.textView6);  //

            start = FindViewById<Button>(Resource.Id.button11);//ІНІЦІАЛІЗАЦІЯ
            stop = FindViewById<Button>(Resource.Id.button2);//ІНІЦІАЛІЗАЦІЯ

            textch = (TextView)FindViewById(Resource.Id.textView7);
            start.Click += delegate (object sender, EventArgs e)//
              {
                  textch.Text = "Идет снятие показаний...";
                  start.Text = "Идет снятие показаний..."; 
              };

            stop.Click += delegate (object sender, EventArgs e)//new
            {
                start.Text = "START";
                ////  start.SetTextSize = "15dp";
              //  float size = 15;
              // void start.SetTextSize(size);
                textch.Text = "Нажмите START для получения координат ";
            };
        }
        //@Override
        override protected void OnResume()
        {
            base.OnResume();
            msensorManager.RegisterListener(this, msensorManager.GetDefaultSensor(SensorType.Accelerometer),SensorDelay.Game);
          //  msensorManager.RegisterListener(this, msensorManager.GetDefaultSensor(Sensor.TYPE_MAGNETIC_FIELD), SensorManager.SENSOR_DELAY_UI);     
        }

        //Чтобы наша программа не съедала ресурсы смартфона будучи свернутой по событию onPause
        //«снимаем» получение данных с датчиков:
       
            //    @Override
            /* override protected void OnPause()
             {
                 base.OnPause();
                 // this.OnPause();

                 ISensorEventListener unlisten;
               //  ISensorEventListener listener = null;
                 msensorManager.UnregisterListener(unlisten, msensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Normal);
             }*/

            //создадим новый метод, в котором данные с датчиков будем заносить в соответствующий 
            //датчику массив. Назовем метод loadNewSensorData:
            private void LoadNewSensorData(SensorEvent e) 
        {
            /*final*/ var type = e.Sensor.Type; //Определяем тип датчика
            if ( type == SensorType.Accelerometer) 
            //Если акселерометр
            { 
                accelData = e.Values.ToArray();
            }
       
       // if (type == Sensor.TYPE_MAGNETIC_FIELD) { //Если геомагнитный датчик
       //     magnetData = event.values.clone();
    }
       
        public void OnSensorChanged(SensorEvent e)
        {
            LoadNewSensorData(e); // Получаем данные с датчика
           // SensorManager.GetRotationMatrix(rotationMatrix, null, accelData, magnetData); //Получаем матрицу поворота
            //SensorManager.GetOrientation(rotationMatrix, OrientationData); //Получаем данные ориентации устройства в пространстве
         
            if ((xView == null) || (yView == null) || (zView == null))
            {  //Без этого работать отказалось.
                xView = (TextView)FindViewById(Resource.Id.textView4);
                yView = (TextView)FindViewById(Resource.Id.textView5);
                zView = (TextView)FindViewById(Resource.Id.textView6);
            }   
            //Выводим результат
            xView.Text = (accelData[0]).ToString();
            yView.Text = (accelData[1]).ToString();
            zView.Text = (accelData[2]).ToString();      
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        }
    }
}
