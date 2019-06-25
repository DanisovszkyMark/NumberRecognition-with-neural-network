using NumberRecognition.Neural_Network;
using NumberRecognition.Paint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace NumberRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NeuralNetwork net = new NeuralNetwork(new int[] { 27 * 27, 50, 50, 10 }, "tanh");
        private bool mouseDown = false;
        private bool rightDown = false;

        public MainWindow()
        {
            InitializeComponent();

            GridFeltoltese();
        }

        private void GridFeltoltese()
        {
            for (int i = 0; i < 27; i++)
            {
                for (int j = 0; j < 27; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Fill = Brushes.Gray;

                    r.MouseUp += R_MouseUp;
                    r.MouseMove += R_MouseMove;

                    Grid.SetRow(r, i);
                    Grid.SetColumn(r, j);
                    gridPaint.Children.Add(r);
                }
            }
        }

        private void R_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (!rightDown)
                {
                    if (mouseDown && (sender as Rectangle).Fill == Brushes.Gray)
                    {
                        (sender as Rectangle).Fill = Brushes.White;
                    }
                }
                else
                {
                    if (mouseDown && (sender as Rectangle).Fill == Brushes.White)
                    {
                        (sender as Rectangle).Fill = Brushes.Gray;
                    }
                }
            }
        }

        private void R_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            rightDown = false;
        }

        public void GridResetColor(Grid grid)
        {
            foreach (Rectangle r in grid.Children)
            {
                r.Fill = Brushes.Gray;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            GridResetColor(this.gridPaint);
        }

        private void gridPaint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = true;
            if (e.RightButton == MouseButtonState.Pressed) rightDown = true;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (cbSave.IsChecked == true && tbSaveValue.Text.Length == 1)
            {
                PaintProcessor.SavePaint(gridPaint, Convert.ToInt32(tbSaveValue.Text.ToString()), "datas.txt");
                btnReset_Click(sender, e);
            }
            else
            {
                float[] atmInput = PaintProcessor.PaintToInput(gridPaint);

                lbResults.Items.Clear();

                List<float> eredmenyek = new List<float>();

                for (int i = 0; i < 10; i++)
                {
                    eredmenyek.Add(NetworkProcessor.Try(net, atmInput, i));
                    ListViewItem lvi = new ListViewItem();
                    lvi.Content = i + "-> " + eredmenyek[i];
                    lbResults.Items.Add(lvi);
                }

                for (int i = 0; i < eredmenyek.Count; i++)
                {
                    if (eredmenyek[i] >= 0.5f && eredmenyek[i] == eredmenyek.Max()) lblResult.Content = i;
                }
            }
        }

        private void MenuLoadNetwork_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NetworkProcessor.LoadNetFromFile(ref net, "net.txt");
                MessageBox.Show("Háló sikeresen betöltve!");
            }
            catch { MessageBox.Show("Hiba történt a fileból való net betöltése közben!"); }
        }

        private void MenuSaveNetwork_Click(object sender, RoutedEventArgs e)
        {
            NetworkProcessor.SaveNetToFile(net, "net.txt");
            MessageBox.Show("Háló sikeresen mentve!");
        }

        private void MenuFileExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuLearnLearning_Click(object sender, RoutedEventArgs e)
        {
            btnOk.IsEnabled = false;

            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork; ;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProgress.Value = e.ProgressPercentage;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0, "Tanulás ...");

            for (int i = 0; i < 100; i++)
            {
                if (i % 10 == 0)
                {
                    NetworkProcessor.SaveNetToFile(net, "net.txt");
                    worker.ReportProgress(i / 1, "%");
                }
                NetworkProcessor.Learning(net, "datas.txt");
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbProgress.Value = 0;
            btnOk.IsEnabled = true;
        }

    }
}
