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
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace ExerciseRecord.MVVM.View
{
    /// <summary>
    /// HighlightView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HighlightView : UserControl
    {
        private const string BasePath = "https://dcdc-14c7d-default-rtdb.firebaseio.com/";//본인의 FB URL
        private const string FirebaseSecret = "e54zPLcujgDI9A9LflL9zVzrgyJIWbjbfT5bszzC"; // FB 비번
        private static FirebaseClient _client;
        DateTime d = DateTime.Now;

        public HighlightView()
        {
            InitializeComponent();

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePath
            };
            _client = new FirebaseClient(config);
            
            getfbg();
            getWeek();

        }

        public async void getfbg()
        {
            string date1 = d.ToString("yyyyMMdd");
            string date2 = d.AddDays(-1).ToString("yyyyMMdd");
            string date3 = d.AddDays(-2).ToString("yyyyMMdd");
            string date4 = d.AddDays(-3).ToString("yyyyMMdd");
            string date5 = d.AddDays(-4).ToString("yyyyMMdd");
            string date6 = d.AddDays(-5).ToString("yyyyMMdd");
            string date7 = d.AddDays(-6).ToString("yyyyMMdd");
           
            FirebaseResponse response = await _client.GetAsync(date1 + "/ET");
            int time1 = response.ResultAs<int>();
            response = await _client.GetAsync(date2 + "/ET");
            int time2= response.ResultAs<int>();
            response = await _client.GetAsync(date3 + "/ET");
            int time3 = response.ResultAs<int>();
            response = await _client.GetAsync(date4 + "/ET");
            int time4 = response.ResultAs<int>();
            response = await _client.GetAsync(date5 + "/ET");
            int time5 = response.ResultAs<int>();
            response = await _client.GetAsync(date6 + "/ET");
            int time6 = response.ResultAs<int>();
            response = await _client.GetAsync(date7 + "/ET");
            int time7 = response.ResultAs<int>();

            int[] time = { time1, time2, time3, time4, time5, time6, time7 };
            int max = time[0];
            int sum = 0;

            foreach (var i in time)
            {
                sum += i;
                if (max < i)
                    max = i;
            }

            timeprint(time1, txtT1); //오늘 시간
            timeprint(time2, txtT2); //어제 시간
            timeprint(sum, txtWeekTimeSum); //이번주 총 시간
            timeprint(sum / 7, txtWeekTimeAve); //이번주 평균 시간
            timeprint(sum / 7, txtWTA);
            timeprint(sum / 7, txtW1);

            void timeprint(int t, TextBlock txt)
            {
                if (t > 3600)
                    txt.Text = (t / 3600).ToString("00") + "시간 " + (t - (t / 3600) % 60).ToString("00") + "분";
                else
                    txt.Text = (t / 60).ToString("00") + "분 " + (t % 60).ToString("00") + "초";
            }

            int mg1 = 210 - (195 * time1 / max);
            int mg2 = 210 - (195 * time2 / max);
            int mg3 = 210 - (195 * time3 / max);
            int mg4 = 210 - (195 * time4 / max);
            int mg5 = 210 - (195 * time5 / max);
            int mg6 = 210 - (195 * time6 / max);
            int mg7 = 210 - (195 * time7 / max);
            int mgA = 190 - (190 * sum/7 / max);

            bd1.Margin = new System.Windows.Thickness { Left = 0, Top = mg1, Right = 0, Bottom = 0 };
            bd2.Margin = new System.Windows.Thickness { Left = 0, Top = mg2, Right = 0, Bottom = 0 };
            bd3.Margin = new System.Windows.Thickness { Left = 0, Top = mg3, Right = 0, Bottom = 0 };
            bd4.Margin = new System.Windows.Thickness { Left = 0, Top = mg4, Right = 0, Bottom = 0 };
            bd5.Margin = new System.Windows.Thickness { Left = 0, Top = mg5, Right = 0, Bottom = 0 };
            bd6.Margin = new System.Windows.Thickness { Left = 0, Top = mg6, Right = 0, Bottom = 0 };
            bd7.Margin = new System.Windows.Thickness { Left = 0, Top = mg7, Right = 0, Bottom = 0 };
            spA.Margin = new System.Windows.Thickness { Left = 0, Top = mgA, Right = 0, Bottom = 0 };

        }

        private void getWeek()
        {
            DateTime dt = DateTime.Now;
            string Week = dt.DayOfWeek.ToString();

            if(Week == "Monday")
            {
                txt1.Text = "월요일"; txt2.Text = "일요일"; txt3.Text = "토요일"; txt4.Text = "금요일"; txt5.Text = "목요일"; txt6.Text = "수요일"; txt7.Text = "화요일";
            }
            else if(Week == "Tuesday")
            {
                txt1.Text = "화요일"; txt2.Text = "월요일"; txt3.Text = "일요일"; txt4.Text = "토요일"; txt5.Text = "금요일"; txt6.Text = "목요일"; txt7.Text = "수요일";
            }
            else if (Week == "Wednesday")
            {
                txt1.Text = "수요일"; txt2.Text = "화요일"; txt3.Text = "월요일"; txt4.Text = "일요일"; txt5.Text = "토요일"; txt6.Text = "금요일"; txt7.Text = "목요일";
            }
            else if (Week == "Thursday")
            {
                txt1.Text = "목요일"; txt2.Text = "수요일"; txt3.Text = "화요일"; txt4.Text = "월요일"; txt5.Text = "일요일"; txt6.Text = "토요일"; txt7.Text = "금요일";
            }
            else if (Week == "Friday")
            {
                txt1.Text = "금요일"; txt2.Text = "목요일"; txt3.Text = "수요일"; txt4.Text = "화요일"; txt5.Text = "월요일"; txt6.Text = "일요일"; txt7.Text = "토요일";
            }
            else if (Week == "Saturday")
            {
                txt1.Text = "토요일"; txt2.Text = "금요일"; txt3.Text = "목요일"; txt4.Text = "수요일"; txt5.Text = "화요일"; txt6.Text = "월요일"; txt7.Text = "일요일";
            }
            else if (Week == "Sunday")
            {
                txt1.Text = "일요일"; txt2.Text = "토요일"; txt3.Text = "금요일"; txt4.Text = "목요일"; txt5.Text = "수요일"; txt6.Text = "화요일"; txt7.Text = "월요일";
            }
        }
    }
}
