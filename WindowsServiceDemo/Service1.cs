using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsServiceDemo
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Tạo 1 timer từ libary System.Timers
            timer = new Timer();
            // Execute mỗi 60s
            timer.Interval = 60000;
            // Những gì xảy ra khi timer đó dc tick
            timer.Elapsed += timer_Tick;
            // Enable timer
            timer.Enabled = true;
            // Ghi vào log file khi services dc start lần đầu tiên
            Utilities.WriteLogError("Test for 1st run WindowsService");
        }
        private void timer_Tick(object sender, ElapsedEventArgs args)
        {
            // Xử lý một vài logic ở đây
            if (DateTime.Now.Minute == 40)
            {
                try
                {
                    BackupService _backup = new BackupService(@"Data Source=WEB\Sqlexpress;Initial Catalog=master;User ID = sa;Password=diamond@123;", "D:\\");
                    _backup.BackupDatabase("demo");
                    Utilities.WriteLogError("Đã backup!!!");
                }
                catch (Exception e) {
                    Utilities.WriteLogError("Lỗi:"+e+"!!!");
                }
                
            }
            Utilities.WriteLogError("Timer has ticked for doing something!!!");
        }
       
        protected override void OnStop()
        {
            timer.Enabled = true;
            Utilities.WriteLogError("1st WindowsService has been stop");
        }
    }
}
