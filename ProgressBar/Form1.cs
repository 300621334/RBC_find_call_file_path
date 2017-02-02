using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ProgressBar//had to change namespace to my project's name from copied code from https://msdn.microsoft.com/en-us/library/system.componentmodel.backgroundworker(v=vs.110).aspx
{
    /*AFter copying the code, go to lightening-bolt for bgWorker to assign all 3 events. or else it doesn't work.
     */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void startAsyncButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the Lengthy Time Consuming asynchronous operation as a bgWorker.
                backgroundWorker1.RunWorkerAsync();//will call "DoWork" of "backgroundWorker1"
                //could pass a param in RunWorkerAsync(arg) w becomes EventArgs for the next method!!
            }
        }

        private void cancelAsyncButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
            }
        }

        // This event handler is where the time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;//probably start & cancel btns are BOTH senders

            for (int i = 1; i <= 10; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(1000 /*500*/ );

                    worker.ReportProgress(i * 10);//10% 20% 30% ..... so on

                    //this will call "ProgressChanged" of "backgroundWorker1"!!!//https://msdn.microsoft.com/en-us/library/system.componentmodel.backgroundworker.reportprogress(v=vs.110).aspx
                    //when bgWorker finishes, it calls "RunWorkerCompleted"!!!
                }
            }
        }

        // This event handler updates the progress.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            resultLabel.Text = (e.ProgressPercentage.ToString() + "%");
        }

        // This event handler deals with the results of the background operation.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                resultLabel.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                resultLabel.Text = "Error: " + e.Error.Message;
            }
            else
            {
                resultLabel.Text = "Done!";
            }
        }
    }
}