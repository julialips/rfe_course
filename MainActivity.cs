using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Hardware;
using Context = Android.Content.Context;
using System.Linq;
using Android.Content;



namespace App3
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
     public class MainActivity : AppCompatActivity, ISensorEventListener
    {
        
        double dt; // отрезое между снятием ускорения в 2 точках
        double allt; //все время 
        long lasttime;
        readonly double[] v= new double [3]; //скорость
        readonly double[] dr= new double [3];  //перемемещение

        protected SensorManager msensorManager; //Менеджер сенсоров 
        //private final Sensor mAccelerometer;
        //readonly string [] x;
        private float[] accelData; // массив ускорений по 3-м осям в формате xyzxyz...
        protected Button start;
        protected Button stop;
        protected Button show;

        private TextView xView;
        private TextView yView;
        private TextView zView;

        private TextView vx;
        private TextView vy;
        private TextView vz;

        private TextView drx;
        private TextView dry;
        private TextView drz;

        protected override void OnCreate(Bundle savedInstanceState)
        {
         
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            msensorManager = (SensorManager)GetSystemService(Context.SensorService);

            accelData = new float[3];
            xView = (TextView)FindViewById(Resource.Id.textView4);  //
            yView = (TextView)FindViewById(Resource.Id.textView5);  // текстовые поля для вывода показаний ускорений
            zView = (TextView)FindViewById(Resource.Id.textView6);  //

            vx = (TextView)FindViewById(Resource.Id.textView10);
            vy = (TextView)FindViewById(Resource.Id.textView11);   // поля для значений скоростей
            vz = (TextView)FindViewById(Resource.Id.textView12);

            drx = (TextView)FindViewById(Resource.Id.textView14);
            dry = (TextView)FindViewById(Resource.Id.textView22);  // поля для значений перемещений
            drz = (TextView)FindViewById(Resource.Id.textView24);


            start = FindViewById<Button>(Resource.Id.button11); //Инициализация кнопки старт
            stop = FindViewById<Button>(Resource.Id.button2);   //Инициализация кнопки стоп
            show = FindViewById<Button>(Resource.Id.button12);  //Инициализация кнопки для перехода в другую активити(страницу)

            // textch = (TextView)FindViewById(Resource.Id.textView7);
            start.Click += delegate (object sender, EventArgs e)
              {     
                  start.Text = "Running...";  
                  //кнопка просто меняет текст, никакого другого смысла в ней, к сожалению, не реализовано
              };
           
            stop.Click += delegate (object sender, EventArgs e)
            {
                // на кнопку Стоп происходит обнуление накопленных значение по скорости и перемещению
                start.Text = "START";
                dr[0] = 0;
                dr[1] = 0;
                dr[2] = 0;
                v[0] = 0;
                v[1] = 0;
                v[2] = 0;
                allt = 0;
            };

            //для подключения через кнопку новой активити
            show.Click += delegate
            {
                Intent intent = new Intent(this, typeof(Class2));
                StartActivity(intent);             
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
        /*    override protected void OnPause()
            {
                base.OnPause();
                // this.OnPause();

                msensorManager.UnregisterListener(this, msensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Game);
            }
            */

        //создадим новый метод, в котором данные с датчиков будем заносить в соответствующий 
        //датчику массив. Назовем метод loadNewSensorData:
        private void LoadNewSensorData(SensorEvent e)
        {
            var type = e.Sensor.Type; //Определяем тип датчика
            if (type == SensorType.Accelerometer)
            //Если акселерометр
            {
                accelData = e.Values.ToArray();
               
                //Получение времени Integrirovanie(accelData);
                dt = (e.Timestamp - lasttime) * 1e-9;
                lasttime = e.Timestamp;

                allt += dt;   //все время от нажатия на сброс
                v[0] += accelData[0] * dt;
                v[1] += accelData[1] * dt;
                v[2] += accelData[2] * dt;

                dr[0] += v[0] * dt;
                dr[1] += v[1] * dt;
                dr[2] += v[2] * dt;
            }
        }
   

        public void OnSensorChanged(SensorEvent e)
        {
            LoadNewSensorData(e); // Получаем данные с датчика
           // SensorManager.GetRotationMatrix(rotationMatrix, null, accelData, magnetData); //Получаем матрицу поворота
            //SensorManager.GetOrientation(rotationMatrix, OrientationData); //Получаем данные ориентации устройства в пространстве
         
            //заполняем поля соответствующими значениями
            if ((xView == null) || (yView == null) || (zView == null))
            {  
                xView = (TextView)FindViewById(Resource.Id.textView4);
                yView = (TextView)FindViewById(Resource.Id.textView5);
                zView = (TextView)FindViewById(Resource.Id.textView6);
            }

                vx = (TextView)FindViewById(Resource.Id.textView10);
                vy = (TextView)FindViewById(Resource.Id.textView11);
                vz = (TextView)FindViewById(Resource.Id.textView12);
            //Выводим результат
        
            xView.Text = (accelData[0]).ToString();
            yView.Text = (accelData[1]).ToString();
            zView.Text = (accelData[2]).ToString();

            vx.Text = (v[0]).ToString();
            vy.Text = (v[1]).ToString();
            vz.Text = (v[2]).ToString();

            drx.Text = (dr[0]).ToString();
            dry.Text = (dr[1]).ToString();
            drz.Text = (dr[2]).ToString();
        }
       
            public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        { }

    }
}

