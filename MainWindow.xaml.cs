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
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;

namespace BCyfr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateError(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal value1;
                decimal value2;

                if (Value1.Text.Contains("√"))
                {
                    string tempValue1 = Value1.Text.Replace("√", "");
                    value1 = (decimal)Math.Sqrt(double.Parse(tempValue1));
                }
                else
                {
                    value1 = decimal.Parse(Value1.Text);
                }

                if (Value2.Text.Contains("√"))
                {
                    string tempValue2 = Value2.Text.Replace("√", "");
                    value2 = (decimal)Math.Sqrt(double.Parse(tempValue2));
                }
                else
                {
                    value2 = decimal.Parse(Value2.Text);
                }

                decimal roundedValue1 = Math.Round(value1, 2);
                decimal roundedValue2 = Math.Round(value2, 2);

                decimal absoluteError1 = Math.Abs(value1 - roundedValue1);
                decimal relativeError1 = (absoluteError1 / value1) * 100;

                decimal absoluteError2 = Math.Abs(value2 - roundedValue2);
                decimal relativeError2 = (absoluteError2 / value2) * 100;

                AbsoluteError1.Text = absoluteError1.ToString();
                RelativeError1.Text = relativeError1.ToString("F2") + "%";

                AbsoluteError2.Text = absoluteError2.ToString();
                RelativeError2.Text = relativeError2.ToString("F2") + "%";

                // Определение точности на основе значащих цифр и расстояния от целого числа
                int significantDigits1 = GetSignificantDigits(value1);
                int significantDigits2 = GetSignificantDigits(value2);

                decimal distanceFromWhole1 = Math.Abs(value1 - Math.Round(value1));
                decimal distanceFromWhole2 = Math.Abs(value2 - Math.Round(value2));

                bool isValue1MoreAccurate = significantDigits1 > significantDigits2 || (significantDigits1 == significantDigits2 && distanceFromWhole1 > distanceFromWhole2);

                string comparisonResult = isValue1MoreAccurate ? "Значение 1 точнее" : "Значение 2 точнее";
                MessageBox.Show(comparisonResult);

                PlotErrors(value1, roundedValue1, absoluteError1, value2, roundedValue2, absoluteError2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка ввода данных: " + ex.Message);
            }
        }

        private int GetSignificantDigits(decimal value)
        {
            string valueStr = value.ToString().TrimEnd('0');
            int decimalPointIndex = valueStr.IndexOf('.');
            return decimalPointIndex == -1 ? valueStr.Length : valueStr.Length - 1;
        }

        private void PlotErrors(decimal value1, decimal roundedValue1, decimal absoluteError1, decimal value2, decimal roundedValue2, decimal absoluteError2)
        {
            var plotModel = new PlotModel { Title = "График погрешностей" };

            var series1 = new LineSeries { Title = "Значение 1", MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, (double)value1));
            series1.Points.Add(new DataPoint(1, (double)roundedValue1));

            var series2 = new LineSeries { Title = "Значение 2", MarkerType = MarkerType.Circle };
            series2.Points.Add(new DataPoint(0, (double)value2));
            series2.Points.Add(new DataPoint(1, (double)roundedValue2));

            plotModel.Series.Add(series1);
            plotModel.Series.Add(series2);

            PlotView.Model = plotModel;
        }
    }
}
