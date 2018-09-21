using System;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace ShutdownTimer
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Timers.Timer aTimer = new System.Timers.Timer();
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Interval = 60000;
            //aTimer.Enabled = true;

            //Console.WriteLine("Press \'q\' to quit the sample.");
            //while (Console.Read() != 'q') ;

            ConfigureService.Configure();

        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            int first, year, month, day, hour, minute, minutesElapsedLimit;
            DateTime now = DateTime.Now;

            if (!File.Exists("ShutDownTimerLimit.txt"))
            {
                string[] lines4 = { "60" };
                System.IO.File.WriteAllLines(@"ShutDownTimerLimit.txt", lines4);
                minutesElapsedLimit = 60;
            }
            else
            {
                string[] lines4 = System.IO.File.ReadAllLines(@"ShutDownTimerLimit.txt");
                minutesElapsedLimit = Convert.ToInt16(lines4[0]);
            }
            

            if (File.Exists("ShutDownTimer.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines(@"ShutDownTimer.txt");

                first = Convert.ToInt16(lines[0]);
                year = Convert.ToInt16(lines[1]);
                month = Convert.ToInt16(lines[2]);
                day = Convert.ToInt16(lines[3]);
                hour = Convert.ToInt16(lines[4]);
                minute = Convert.ToInt16(lines[5]);
            }
            else
            {
                first = 1; //es la primera vez en el día
                year = now.Year;
                month = now.Month;
                day = now.Day;
                hour = now.Hour;
                minute = now.Minute;

                string[] lines = { first.ToString(), year.ToString(), month.ToString(), day.ToString(), hour.ToString(), minute.ToString() };
                System.IO.File.WriteAllLines(@"ShutDownTimer.txt", lines);
            }

            if (first == 0)
            {
                var fileDateArchivo = new DateTime(year, month, day);
                var fileDateNow = new DateTime(now.Year, now.Month, now.Day);
                if ((fileDateNow - fileDateArchivo).Days > 0)
                {
                    first = 1; //es la primera vez en el día
                    year = now.Year;
                    month = now.Month;
                    day = now.Day;
                    hour = now.Hour;
                    minute = now.Minute;

                    string[] lines3 = { first.ToString(), year.ToString(), month.ToString(), day.ToString(), hour.ToString(), minute.ToString() };
                    System.IO.File.WriteAllLines(@"ShutDownTimer.txt", lines3);
                }
                else
                {
                    Process.Start("shutdown", "/s /f /t 30");
                    System.Console.WriteLine("Limit reached, shutdowning the system");
                }
                
            }
            else
            {
                var fileDate = new DateTime(year, month, day, hour, minute, 0);
                double minutesElapsed = (now - fileDate).TotalMinutes;

                Console.WriteLine("fileDate      : {0}", fileDate.ToString());
                Console.WriteLine("now           : {0}", now.ToString());
                Console.WriteLine("minutesElapsed: {0}", minutesElapsed.ToString());
                Console.WriteLine("---------------");

                if (minutesElapsed > minutesElapsedLimit)
                {
                    System.Console.WriteLine("Shutdown the system");
                    first = 0;
                    string[] lines2 = { first.ToString(), year.ToString(), month.ToString(), day.ToString(), hour.ToString(), minute.ToString() };
                    System.IO.File.WriteAllLines(@"ShutDownTimer.txt", lines2);
                    Process.Start("shutdown", "/s /f /t 30");
                }
            }
        }
    }
}
