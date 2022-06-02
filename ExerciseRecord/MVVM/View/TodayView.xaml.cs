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
        private const string BasePath = "https://dcdc-14c7d-default-rtdb.firebaseio.com/";//본인의 FB URL
        private const string FirebaseSecret = "e54zPLcujgDI9A9LflL9zVzrgyJIWbjbfT5bszzC"; // FB 비번
        private static FirebaseClient _client;
        public string today;

        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dawon\source\repos\ExerciseRecord\ExerciseRecord\Memo.mdf;Integrated Security=True";
        DispatcherTimer t = new DispatcherTimer();

        public TodayView()
        {
            InitializeComponent();

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePath
            };
            _client = new FirebaseClient(config);

            DateTime datetime = DateTime.Now;
            today = datetime.ToString();
            today = today.Substring(0, 10);
            today = today.Replace("-", "");

            getfb();

            t.Interval = new TimeSpan(0, 0, 1); //1초
        }

        private async void getfb()
        {
            FirebaseResponse response = await _client.GetAsync(today + "/LR");
            int value = response.ResultAs<int>();
            MessageBox.Show(value.ToString());
        }

        List<Exercise> items = new List<Exercise>();
        public class Exercise
        {
            public Image Image { get; set; }
            public string ExeName { get; set; }
            public int Time { get; set; }
            public int Set { get; set; }
        }

        private void btnMemo_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection(connStr);
            string date = DateTime.Now.ToString("yyyyMMdd");

            string sqld = string.Format("SELECT COUNT(*) From MemoTable WHERE Date='{0}'", date);
            if()


            string sql = string.Format("INSERT INTO MemoTable Values ('{0}', '{1}')", date, txtTodayMemo.Text);
            MessageBox.Show(sql);

            
        }
    }
}
