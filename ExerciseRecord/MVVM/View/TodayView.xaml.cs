using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Threading;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace ExerciseRecord.MVVM.View
{
    /// <summary>
    /// TodayView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TodayView : UserControl
    {
        private const string BasePath = "https://dcdc-14c7d-default-rtdb.firebaseio.com/";   //본인의 FB URL
        private const string FirebaseSecret = "e54zPLcujgDI9A9LflL9zVzrgyJIWbjbfT5bszzC";    // FB 비번
        private static FirebaseClient _client;

        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dawon\source\repos\ExerciseRecord\ExerciseRecord\Memo.mdf;Integrated Security=True";
        //DispatcherTimer t = new DispatcherTimer();
        string date = DateTime.Now.ToString("yyyyMMdd");

        public TodayView()
        {
            InitializeComponent();

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePath
            };
            _client = new FirebaseClient(config);

            getfb();

            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            string sql = string.Format("SELECT COUNT(*) From MemoTable WHERE Date='{0}'", date);

            SqlCommand comm = new SqlCommand(sql, conn);
            int count = Convert.ToInt32(comm.ExecuteScalar());

            if (count == 1)
            {
                // MessageBox.Show("1");
                Memo();
            }

            conn.Close();
        }

        private async void getfb()
        {
            FirebaseResponse response = await _client.GetAsync(date + "/ET");
            if (response.ResultAs<string>() == null)
            {
                _client.Set<int>(date + "/LR", 0);
                _client.Set<int>(date + "/PU", 0);
                _client.Set<int>(date + "/PL", 0);
                _client.Set<int>(date + "/SU", 0);
                _client.Set<int>(date + "/ET", 0);
                _client.Set<int>(date + "/OT", 0);
                _client.Set<int>(date + "/OC", 0);
            }
            getfbe(); //불러온 운동을 리스트뷰에 표시
            getfbt(); //불러온 시간 표시
        }

        private async void getfbt()
        {
            FirebaseResponse response = await _client.GetAsync(date + "/ET");
            int value = response.ResultAs<int>();
            if (value > 3600)
                txtTodayTime.Text = (value / 3600).ToString("00") + "시간 " + (value % 3600 % 60).ToString("00") + "분";
            else
                txtTodayTime.Text = (value / 60).ToString("00") + "분 " + (value % 60).ToString("00") + "초";
        }

        private void Memo()
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            string sql = string.Format("SELECT * FROM MemoTable WHERE Date='{0}'", date);

            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataReader reader = comm.ExecuteReader();

            reader.Read();

            string x = (string)reader["Memo"];
            txtTodayMemo.Text = x;

            reader.Close();
        }

        List<Exercise> items = new List<Exercise>();
        public class Exercise
        {
            public string Image { get; set; }
            public string ExeName { get; set; }
            public string info { get; set; }
        }

        private async void getfbe()
        {
            FirebaseResponse response = await _client.GetAsync(date + "/LR");
            int value = response.ResultAs<int>();
            if (value > 0)
            {
                items.Add(new Exercise() { Image = "/Images/LegRaise.png", ExeName = "LegRaise", info = value.ToString() + "회" });
                todayList.ItemsSource = items;
                todayList.Items.Refresh();
            }

            response = await _client.GetAsync(date + "/PU");
            value = response.ResultAs<int>();
            if (value > 0)
            {
                items.Add(new Exercise() { Image = "/Images/PushUp.png", ExeName = "Push Up", info = value.ToString() + "회" });
                todayList.ItemsSource = items;
                todayList.Items.Refresh();
            }

            response = await _client.GetAsync(date + "/PL");
            value = response.ResultAs<int>();
            if (value > 0)
            {
                string time;
                if (value > 3600)
                    time = (value / 3600).ToString("00") + "시간 " + (value % 3600 % 60).ToString("00") + "분";
                else
                    time = (value / 60).ToString("00") + "분 " + (value % 60).ToString("00") + "초";
                items.Add(new Exercise() { Image = "/Images/Plank.png", ExeName = "Plank", info = time });
                todayList.ItemsSource = items;
                todayList.Items.Refresh();
            }

            response = await _client.GetAsync(date + "/SU");
            value = response.ResultAs<int>();
            if (value > 0)
            {
                items.Add(new Exercise() { Image = "/Images/SitUp.png", ExeName = "Sit Up", info = value.ToString() + "회" });
                todayList.ItemsSource = items;
                todayList.Items.Refresh();
            }

            response = await _client.GetAsync(date + "/OT");
            value = response.ResultAs<int>();
            if (value > 0)
            {
                string time;
                if (value > 3600)
                    time = (value / 3600).ToString("00") + "시간 " + (value % 3600 % 60).ToString("00") + "분";
                else
                    time = (value / 60).ToString("00") + "분 " + (value % 60).ToString("00") + "초";
                items.Add(new Exercise() { Image = "/Images/Timer.png", ExeName = "Timer", info = time });
                todayList.ItemsSource = items;
                todayList.Items.Refresh();
            }

            response = await _client.GetAsync(date + "/OC");
            value = response.ResultAs<int>();
            if (value > 0)
            {
                items.Add(new Exercise() { Image = "/Images/Count.png", ExeName = "Count", info = value.ToString() + "회" });
                todayList.ItemsSource = items;
                todayList.Items.Refresh();
            }
        }

        private void btnMemo_Click(object sender, RoutedEventArgs e)
        {
            if (txtTodayMemo.Text != "")
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();

                string sql = string.Format("SELECT COUNT(*) From MemoTable WHERE Date='{0}'", date);

                SqlCommand comm = new SqlCommand(sql, conn);
                int count = Convert.ToInt32(comm.ExecuteScalar());

                if (count == 1)
                {
                    //MessageBox.Show("수정");
                    Update();
                }
                else
                {
                    //MessageBox.Show("추가");
                    Add();
                }
                conn.Close();
            }
        }

        private void Add()
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            string sql = string.Format("INSERT INTO MemoTable Values ('{0}', '{1}')", date, txtTodayMemo.Text);

            SqlCommand comm = new SqlCommand(sql, conn);
            int x = comm.ExecuteNonQuery();

            //if (x == 1)
                //MessageBox.Show("성공");

            conn.Close();
        }

        private void Update()
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            string sql = string.Format("UPDATE MemoTAble SET Memo='{0}' WHERE Date='{1}'", txtTodayMemo.Text, date);

            SqlCommand comm = new SqlCommand(sql, conn);
            int x = comm.ExecuteNonQuery();

            //if (x == 1)
                //MessageBox.Show("성공");

            conn.Close();
        }
    }
}
