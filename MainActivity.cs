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

//
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
//


namespace App3
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ISensorEventListener
    {

        double dt; // отрезое между снятием ускорения в 2 точках
        double allt; //все время 
        long lasttime;
        readonly double[] v = new double[3]; //скорость
        readonly double[] dr = new double[3];  //перемемещение

        protected SensorManager msensorManager; //Менеджер сенсоров 
        //private final Sensor mAccelerometer;
        //readonly string [] x;
        private float[] accelData; // массив ускорений по 3-м осям в формате xyzxyz...
        //05.05
        private float[] giroscopeData;
        private float[] magnitometrData;
        //05.05

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

        public TextView QuaterionField;

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

            QuaterionField = (TextView)FindViewById(Resource.Id.textView_Value_Quaterion);

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
            msensorManager.RegisterListener(this, msensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Game);
            // 05.05
            msensorManager.RegisterListener(this, msensorManager.GetDefaultSensor(SensorType.Gyroscope), SensorDelay.Game);
            msensorManager.RegisterListener(this, msensorManager.GetDefaultSensor(SensorType.MagneticField), SensorDelay.Game);
            //05.05
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
            //Определяем тип датчика
            var type = e.Sensor.Type;
            if (type == SensorType.Accelerometer)
            //Если акселерометр
            {
                //Записываем данные с датчика в массив
                accelData = e.Values.ToArray();

                //Получение времени Integrirovanie(accelData);
                dt = (e.Timestamp - lasttime) * 1e-9;
                //время между двумя последними событиями(снятиями показаний с датчика)
                lasttime = e.Timestamp;

                //все время от нажатия на сброс
                allt += dt;
                //первое интегрирование, получение скорости
                v[0] += accelData[0] * dt;
                v[1] += accelData[1] * dt;
                v[2] += accelData[2] * dt;

                //второе интегрирование, получение перемещения по каждой из координат
                dr[0] += v[0] * dt;
                dr[1] += v[1] * dt;
                dr[2] += v[2] * dt;
            }

            //05.05
            //Определяем тип датчика, если гироскоп
            var typee = e.Sensor.Type;
            if (typee == SensorType.Gyroscope)
            {
                //Записываем данные с гироскопа в массив
                giroscopeData = e.Values.ToArray();
            }

            //Определяем тип датчика, если магнетометр
            var type_mag = e.Sensor.Type;
            if (type_mag == SensorType.MagneticField)
            {
                magnitometrData = e.Values.ToArray();
            }
            //05.05
        }
        public void OnSensorChanged(SensorEvent e)
        {
            // Получаем данные с датчика
            LoadNewSensorData(e);
            // SensorManager.GetRotationMatrix(rotationMatrix, null, accelData, magnetData); //Получаем матрицу поворота
            // SensorManager.GetOrientation(rotationMatrix, OrientationData); //Получаем данные ориентации устройства в пространстве

            //заполняем поля полученными с датчиков значениями
            //if ((xView == null) || (yView == null) || (zView == null))
            //{  
            // xView = (TextView)FindViewById(Resource.Id.textView4);
            //  yView = (TextView)FindViewById(Resource.Id.textView5); уже определены вначале
            //  zView = (TextView)FindViewById(Resource.Id.textView6);
            // }

            //  vx = (TextView)FindViewById(Resource.Id.textView10);
            //  vy = (TextView)FindViewById(Resource.Id.textView11);
            // vz = (TextView)FindViewById(Resource.Id.textView12);
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
            // 06.05
            //создание объекта класса фильтра Маджвика
            MadgwickAHRS madgwick = new MadgwickAHRS(1f / 256f);
            madgwick.Update(deg2rad(giroscopeData[0]), deg2rad(giroscopeData[1]), deg2rad(giroscopeData[2]), accelData[0], accelData[1], accelData[2]);

            //выводим в текстовое поле значение кватерниона из свойства класса MadgwickAHRS {get;set}
            QuaterionField.Text = (madgwick.Quaternion).ToString();

            //преобразование кватерyионов в углы эйлера
            static float deg2rad(float degrees)
            {
                return (float)(Math.PI / 180) * degrees;
            }
            //06.05
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        { }

        // new code 05.05
        //
        //
        //07.05//
    }
       //07.05//
    public class MadgwickAHRS
        {       
            /// Gets or sets the sample period.
           public float SamplePeriod { get; set; }

            /// Gets or sets the algorithm gain beta.
            public float Beta { get; set; }

            /// Gets or sets the Quaternion output.
            // public float[] Quaternion { get; set; } так в оригинате, 07.05

            public float[] Quaternion 
            {
                get { return Quaternion; }//так я сделала, по сути просто явно определила, кажется это не обязаельно
                set { }
            }

            /// <summary>
            /// Инициализация нового экземпляра класса <see cref="MadgwickAHRS"/> 
            /// </summary>
            /// <param name="samplePeriod">
            /// Период выборки
            /// </param>
            public MadgwickAHRS(float samplePeriod): this(samplePeriod, 1f)// этот я использую
            { }

            /// <summary>
            /// Инициализация нового экземпляра класса <see cref="MadgwickAHRS"/> 
            /// </summary>
            /// <param name="samplePeriod">
            /// Период выборки.
            /// </param>
            /// <param name="beta">
            /// Algorithm gain beta.
            /// </param>
            public MadgwickAHRS(float samplePeriod, float beta)
            {
                SamplePeriod = samplePeriod;
                Beta = beta;
                Quaternion = new float[] { 1f, 0f, 0f, 0f };
            }

            /// <summary>
            /// Algorithm AHRS update method. Requires only gyroscope and accelerometer data.
            /// </summary>
            /// <param name="gx">
            /// Gyroscope x axis measurement in radians/s.
            /// </param>
            /// <param name="gy">
            /// Gyroscope y axis measurement in radians/s.
            /// </param>
            /// <param name="gz">
            /// Gyroscope z axis measurement in radians/s.
            /// </param>
            /// <param name="ax">
            /// Accelerometer x axis measurement in any calibrated units.
            /// </param>
            /// <param name="ay">
            /// Accelerometer y axis measurement in any calibrated units.
            /// </param>
            /// <param name="az">
            /// Accelerometer z axis measurement in any calibrated units.
            /// </param>
            /// <param name="mx">
            /// Magnetometer x axis measurement in any calibrated units.
            /// </param>
            /// <param name="my">
            /// Magnetometer y axis measurement in any calibrated units.
            /// </param>
            /// <param name="mz">
            /// Magnetometer z axis measurement in any calibrated units.
            /// </param>
            /// Optimised for minimal arithmetic.
            /// Total ±: 160
            /// Total *: 172
            /// Total /: 5
            /// Total sqrt: 5
            public void Update(float gx, float gy, float gz, float ax, float ay, float az, float mx, float my, float mz)
            {
                float q1 = Quaternion[0], q2 = Quaternion[1], q3 = Quaternion[2], q4 = Quaternion[3];   
                float norm;
                float hx, hy, _2bx, _2bz;
                float s1, s2, s3, s4;
                float qDot1, qDot2, qDot3, qDot4;

            // Вспомогательные переменные, чтобы избежать повторной арифметики
                float _2q1mx;
                float _2q1my;
                float _2q1mz;
                float _2q2mx;
                float _4bx;
                float _4bz;
                float _2q1 = 2f * q1;
                float _2q2 = 2f * q2;
                float _2q3 = 2f * q3;
                float _2q4 = 2f * q4;
                float _2q1q3 = 2f * q1 * q3;
                float _2q3q4 = 2f * q3 * q4;
                float q1q1 = q1 * q1;
                float q1q2 = q1 * q2;
                float q1q3 = q1 * q3;
                float q1q4 = q1 * q4;
                float q2q2 = q2 * q2;
                float q2q3 = q2 * q3;
                float q2q4 = q2 * q4;
                float q3q3 = q3 * q3;
                float q3q4 = q3 * q4;
                float q4q4 = q4 * q4;

                // Нормализация измерений акселерометра
                norm = (float)Math.Sqrt(ax * ax + ay * ay + az * az);
                if (norm == 0f) return; // handle NaN
                norm = 1 / norm;        // use reciprocal for division
                ax *= norm;
                ay *= norm;
                az *= norm;
 
                // Нормализация измерений магнетометра
                norm = (float)Math.Sqrt(mx * mx + my * my + mz * mz);
                if (norm == 0f) return; // handle NaN
                norm = 1 / norm;        // use reciprocal for division
                mx *= norm;
                my *= norm;
                mz *= norm;

               // Контрольное направление магнитного поля Земли
                _2q1mx = 2f * q1 * mx;
                _2q1my = 2f * q1 * my;
                _2q1mz = 2f * q1 * mz;
                _2q2mx = 2f * q2 * mx;
                hx = mx * q1q1 - _2q1my * q4 + _2q1mz * q3 + mx * q2q2 + _2q2 * my * q3 + _2q2 * mz * q4 - mx * q3q3 - mx * q4q4;
                hy = _2q1mx * q4 + my * q1q1 - _2q1mz * q2 + _2q2mx * q3 - my * q2q2 + my * q3q3 + _2q3 * mz * q4 - my * q4q4;
                _2bx = (float)Math.Sqrt(hx * hx + hy * hy);
                _2bz = -_2q1mx * q3 + _2q1my * q2 + mz * q1q1 + _2q2mx * q4 - mz * q2q2 + _2q3 * my * q4 - mz * q3q3 + mz * q4q4;
                _4bx = 2f * _2bx;
                _4bz = 2f * _2bz;

                // Метод градиентного спуска
                 s1 = -_2q3 * (2f * q2q4 - _2q1q3 - ax) + _2q2 * (2f * q1q2 + _2q3q4 - ay) - _2bz * q3 * (_2bx * (0.5f - q3q3 - q4q4) + _2bz * (q2q4 - q1q3) - mx) + (-_2bx * q4 + _2bz * q2) * (_2bx * (q2q3 - q1q4) + _2bz * (q1q2 + q3q4) - my) + _2bx * q3 * (_2bx * (q1q3 + q2q4) + _2bz * (0.5f - q2q2 - q3q3) - mz);
                s2 = _2q4 * (2f * q2q4 - _2q1q3 - ax) + _2q1 * (2f * q1q2 + _2q3q4 - ay) - 4f * q2 * (1 - 2f * q2q2 - 2f * q3q3 - az) + _2bz * q4 * (_2bx * (0.5f - q3q3 - q4q4) + _2bz * (q2q4 - q1q3) - mx) + (_2bx * q3 + _2bz * q1) * (_2bx * (q2q3 - q1q4) + _2bz * (q1q2 + q3q4) - my) + (_2bx * q4 - _4bz * q2) * (_2bx * (q1q3 + q2q4) + _2bz * (0.5f - q2q2 - q3q3) - mz);
                s3 = -_2q1 * (2f * q2q4 - _2q1q3 - ax) + _2q4 * (2f * q1q2 + _2q3q4 - ay) - 4f * q3 * (1 - 2f * q2q2 - 2f * q3q3 - az) + (-_4bx * q3 - _2bz * q1) * (_2bx * (0.5f - q3q3 - q4q4) + _2bz * (q2q4 - q1q3) - mx) + (_2bx * q2 + _2bz * q4) * (_2bx * (q2q3 - q1q4) + _2bz * (q1q2 + q3q4) - my) + (_2bx * q1 - _4bz * q3) * (_2bx * (q1q3 + q2q4) + _2bz * (0.5f - q2q2 - q3q3) - mz);
                s4 = _2q2 * (2f * q2q4 - _2q1q3 - ax) + _2q3 * (2f * q1q2 + _2q3q4 - ay) + (-_4bx * q4 + _2bz * q2) * (_2bx * (0.5f - q3q3 - q4q4) + _2bz * (q2q4 - q1q3) - mx) + (-_2bx * q1 + _2bz * q3) * (_2bx * (q2q3 - q1q4) + _2bz * (q1q2 + q3q4) - my) + _2bx * q2 * (_2bx * (q1q3 + q2q4) + _2bz * (0.5f - q2q2 - q3q3) - mz);
                norm = 1f / (float)Math.Sqrt(s1 * s1 + s2 * s2 + s3 * s3 + s4 * s4);    // normalise step magnitude
                s1 *= norm;
                s2 *= norm;
                s3 *= norm;
                s4 *= norm;

                // Вычисление скорости изменения кватерниона
                qDot1 = 0.5f * (-q2 * gx - q3 * gy - q4 * gz) - Beta * s1;
                qDot2 = 0.5f * (q1 * gx + q3 * gz - q4 * gy) - Beta * s2;
                qDot3 = 0.5f * (q1 * gy - q2 * gz + q4 * gx) - Beta * s3;
                qDot4 = 0.5f * (q1 * gz + q2 * gy - q3 * gx) - Beta * s4;

                // Интегрирование, для получения кватерниона
                q1 += qDot1 * SamplePeriod;
                q2 += qDot2 * SamplePeriod;
                q3 += qDot3 * SamplePeriod;
                q4 += qDot4 * SamplePeriod;
                norm = 1f / (float)Math.Sqrt(q1 * q1 + q2 * q2 + q3 * q3 + q4 * q4);    // нормализация кватерниона
                

                Quaternion[0] = q1 * norm;
                Quaternion[1] = q2 * norm;
                Quaternion[2] = q3 * norm;
                Quaternion[3] = q4 * norm;
            }

            /// <summary>
            /// Algorithm IMU update method. Requires only gyroscope and accelerometer data.
            /// </summary>
            /// <param name="gx">
            /// Gyroscope x axis measurement in radians/s.
            /// </param>
            /// <param name="gy">
            /// Gyroscope y axis measurement in radians/s.
            /// </param>
            /// <param name="gz">
            /// Gyroscope z axis measurement in radians/s.
            /// </param>
            /// <param name="ax">
            /// Accelerometer x axis measurement in any calibrated units.
            /// </param>
            /// <param name="ay">
            /// Accelerometer y axis measurement in any calibrated units.
            /// </param>
            /// <param name="az">
            /// Accelerometer z axis measurement in any calibrated units.
            /// </param>

            /// Optimised for minimal arithmetic. Total ±: 45. Total *: 85. Total /: 3. Total sqrt: 3

            public void Update(float gx, float gy, float gz, float ax, float ay, float az)
            {
                float q1 = Quaternion[0], q2 = Quaternion[1], q3 = Quaternion[2], q4 = Quaternion[3];   
                float norm;
                float s1, s2, s3, s4;
                float qDot1, qDot2, qDot3, qDot4;

                // Вспомогательные переменные, чтобы избежать повторной арифметики
                float _2q1 = 2f * q1;
                float _2q2 = 2f * q2;
                float _2q3 = 2f * q3;
                float _2q4 = 2f * q4;
                float _4q1 = 4f * q1;
                float _4q2 = 4f * q2;
                float _4q3 = 4f * q3;
                float _8q2 = 8f * q2;
                float _8q3 = 8f * q3;
                float q1q1 = q1 * q1;
                float q2q2 = q2 * q2;
                float q3q3 = q3 * q3;
                float q4q4 = q4 * q4;

                // Нормализация измерений акселерометра
                norm = (float)Math.Sqrt(ax * ax + ay * ay + az * az);
                if (norm == 0f) return; // handle NaN
                norm = 1 / norm;        // use reciprocal for division
                ax *= norm;
                ay *= norm;
                az *= norm;

                // Метод градиентного спуска
                s1 = _4q1 * q3q3 + _2q3 * ax + _4q1 * q2q2 - _2q2 * ay;
                s2 = _4q2 * q4q4 - _2q4 * ax + 4f * q1q1 * q2 - _2q1 * ay - _4q2 + _8q2 * q2q2 + _8q2 * q3q3 + _4q2 * az;
                s3 = 4f * q1q1 * q3 + _2q1 * ax + _4q3 * q4q4 - _2q4 * ay - _4q3 + _8q3 * q2q2 + _8q3 * q3q3 + _4q3 * az;
                s4 = 4f * q2q2 * q4 - _2q2 * ax + 4f * q3q3 * q4 - _2q3 * ay;
                norm = 1f / (float)Math.Sqrt(s1 * s1 + s2 * s2 + s3 * s3 + s4 * s4);    // normalise step magnitude
                s1 *= norm;
                s2 *= norm;
                s3 *= norm;
                s4 *= norm;

                // Вычисление скорости изменения кватерниона
                qDot1 = 0.5f * (-q2 * gx - q3 * gy - q4 * gz) - Beta * s1;
                qDot2 = 0.5f * (q1 * gx + q3 * gz - q4 * gy) - Beta * s2;
                qDot3 = 0.5f * (q1 * gy - q2 * gz + q4 * gx) - Beta * s3;
                qDot4 = 0.5f * (q1 * gz + q2 * gy - q3 * gx) - Beta * s4;

                //  Интегрирование для получения кватерниона
                q1 += qDot1 * SamplePeriod;
                q2 += qDot2 * SamplePeriod;
                q3 += qDot3 * SamplePeriod;
                q4 += qDot4 * SamplePeriod;

                //нормализация кватерниона
                norm = 1f / (float)Math.Sqrt(q1 * q1 + q2 * q2 + q3 * q3 + q4 * q4);    
                Quaternion[0] = q1 * norm;
                Quaternion[1] = q2 * norm;
                Quaternion[2] = q3 * norm;
                Quaternion[3] = q4 * norm;
            }
        }

        //
        //05.05
        //
        //

   // } 07.05
}

