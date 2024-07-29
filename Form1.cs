using System;
using System.Windows.Forms;
using NAudio.Wave;
using System.Threading.Tasks;

namespace bgservice
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private DateTime scheduledTime;
        private bool alarmRinging;
        private int alarmCount;
        private readonly string alarmFilePath = @"C:\Users\ruksh\Downloads\tone.mp3";
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        public Form1()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 1000; // Check every second
            timer.Tick += Timer_Tick;
            alarmRinging = false;
            alarmCount = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Optionally initialize other settings or state
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            // Handle DateTimePicker value changes if needed
        }

        private void messageTextBox_TextChanged(object sender, EventArgs e)
        {
            // Handle TextBox text changes if needed
        }

        private void scheduleButton_Click(object sender, EventArgs e)
        {
            // Get the scheduled time and message from the controls
            scheduledTime = dateTimePicker.Value;

            DateTime now = DateTime.Now;
            DateTime scheduledDateTime = new DateTime(now.Year, now.Month, now.Day, scheduledTime.Hour, scheduledTime.Minute, 0);

            // Adjust for next day if the time has already passed today
            if (scheduledDateTime <= now)
            {
                scheduledDateTime = scheduledDateTime.AddDays(1);
            }

            scheduledTime = scheduledDateTime;

            if (scheduledTime <= DateTime.Now)
            {
                MessageBox.Show("Please select a future time.");
                return;
            }

            // Update label to show scheduled time
            label1.Text = $"Scheduled for {scheduledTime.ToShortTimeString()}";

            // Start the timer to check the time
            timer.Start();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now >= scheduledTime && !alarmRinging)
            {
                alarmRinging = true;
                int ringingTimes = (int)numericUpDownRings.Value; // Get the number of rings from the numeric up-down control
                await PlayAlarmTone(ringingTimes);

                // Update the label
                label1.Text = "Alarm ringing finished!";

                // Reset the flag and alarm count
                alarmRinging = false;
                alarmCount = 0;

                // Stop the timer
                timer.Stop();
            }
        }

        private async Task PlayAlarmTone(int ringingTimes)
        {
            for (int i = 0; i < ringingTimes; i++)
            {
                using (audioFile = new AudioFileReader(alarmFilePath))
                using (outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    await Task.Delay(audioFile.TotalTime);
                    outputDevice.Stop();
                }
                await Task.Delay(1000); // Wait for 1 second before the next ring
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Handle label click if needed
        }
    }
}
