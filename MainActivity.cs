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
//using ZedGraph;


namespace App3
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
     public class MainActivity : AppCompatActivity, ISensorEventListener
    {
        
        double dt; // отрезое между снятіем ускоренія в 2 точках
        double allt;//все время 
        long lasttime;
        readonly double[] v= new double [3]; //скорость
        readonly double[] dr= new double [3];  //перемеўеніе

        protected SensorManager msensorManager;//Менеджер сенсоров аппрата
        //private final Sensor mAccelerometer;
       //readonly string [] x;
        private float[] accelData;
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

        //  private TextView textch;
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

            vx = (TextView)FindViewById(Resource.Id.textView10);
            vy = (TextView)FindViewById(Resource.Id.textView11);
            vz = (TextView)FindViewById(Resource.Id.textView12);

            drx = (TextView)FindViewById(Resource.Id.textView14);
            dry = (TextView)FindViewById(Resource.Id.textView22);
            drz = (TextView)FindViewById(Resource.Id.textView24);




            start = FindViewById<Button>(Resource.Id.button11);//Инициализация кнопки старт
            stop = FindViewById<Button>(Resource.Id.button2);//Инициализация кнопки стоп
            show = FindViewById<Button>(Resource.Id.button12);//Инициализация кнопки для показа другой активити

            // textch = (TextView)FindViewById(Resource.Id.textView7);
            start.Click += delegate (object sender, EventArgs e)//
              {
                //  textch.Text = "Идет снятие показаний...";
                  start.Text = "Running...";             
              };
           
            stop.Click += delegate (object sender, EventArgs e)//new
            {
                start.Text = "START";
                dr[0] = 0;
                dr[1] = 0;
                dr[2] = 0;
                v[0] = 0;
                v[1] = 0;
                v[2] = 0;
                allt = 0;


                ////  start.SetTextSize = "15dp";
                //  float size = 15;
                // void start.SetTextSize(size);

                // textch.Text = "Нажмите START для получения координат ";
            };
            //для подключенія через кнопку новой активити
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
               
                //Integrirovanie(accelData);
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
         
            if ((xView == null) || (yView == null) || (zView == null))
            {  //Без этого работать отказалось.
                xView = (TextView)FindViewById(Resource.Id.textView4);
                yView = (TextView)FindViewById(Resource.Id.textView5);
                zView = (TextView)FindViewById(Resource.Id.textView6);
            }
            //textview 10,11,12

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

            //
           // Chart myChart = new Chart();
            //GraphPane pane = zedGraph.GraphPane;

        }
       

                //int c;

                /* private void Button1_Click(object sender, EventArgs e)//для стоп
                 {
                     c++;
                     stop.Text = Convert.ToString(c);

                 }*/
                public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        }

    }
}

/*

    <TextView
        android:text= "Нажмите START для получения координат :"
        android:textColor="@color/abc_background_cache_hint_selector_material_dark"
        android:layout_width="363.0dp"
        android:layout_height="74.5dp"
        android:textSize = "19dp"
        android:layout_marginLeft="0.0dp"
        android:id="@+id/textView7"
        android:layout_marginBottom="0.0dp"
        android:layout_marginTop="0.0dp"
        android:layout_marginRight="17.0dp" />
        */