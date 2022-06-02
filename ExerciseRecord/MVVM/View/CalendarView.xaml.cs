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
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace ExerciseRecord.MVVM.View
{
    
    public partial class CalendarView : UserControl
    {
        private const string BasePath = "https://dcdc-14c7d-default-rtdb.firebaseio.com/";//본인의 FB URL
        private const string FirebaseSecret = "e54zPLcujgDI9A9LflL9zVzrgyJIWbjbfT5bszzC"; // FB 비번
        private static FirebaseClient _client;
        public string ttdd;

        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dawon\source\repos\ExerciseRecord\ExerciseRecord\Memo.mdf;Integrated Security=True";

        public CalendarView()
        {
            InitializeComponent();
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = FirebaseSecret,
                BasePath = BasePath
            };
            _client = new FirebaseClient(config);
        }

        private async void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            //캘린더 날짜 
            var date = sender as Calendar;
            string td = date.SelectedDate.ToString();
            ttdd = td.Substring(0, 10);
            ttdd = ttdd.Replace("-", "");

            //날짜에 해당하는 데이터
            FirebaseResponse response = await _client.GetAsync(ttdd + "/LR");
            int value = response.ResultAs<int>();
            //MessageBox.Show(value.ToString());

            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            string sql = string.Format("SELECT COUNT(*) From MemoTable WHERE Date='{0}'", ttdd);

            SqlCommand comm = new SqlCommand(sql, conn);
            int count = Convert.ToInt32(comm.ExecuteScalar());

            if (count == 1)
            {
                // MessageBox.Show("1");
                Memo();
            }
            else
                lblMemo.Text = "";
           
            conn.Close();
        }

        private void Memo()
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            string sql = string.Format("SELECT * FROM MemoTable WHERE Date='{0}'", ttdd);

            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataReader reader = comm.ExecuteReader();

            reader.Read();

            string x = (string)reader["Memo"];
            lblMemo.Text = x;

            reader.Close();

        }
    }
}
