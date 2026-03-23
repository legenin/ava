using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using HorizontalAlignment = OxyPlot.HorizontalAlignment;
using OxyPlot.Legends;
using System.Collections.Generic;

namespace ava.Models
{
    public class LineChartModel
    {
        public PlotModel ChartModel { get; }

        public LineChartModel()
        {
            ChartModel = new PlotModel 
            { 
                Title = "Линейный график",
            };

            var legend = new Legend()
            {
                LegendBorder = OxyColors.Black,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Vertical,
                LegendItemOrder = LegendItemOrder.Normal,
                LegendItemAlignment = HorizontalAlignment.Left,
                LegendSymbolPlacement = LegendSymbolPlacement.Left,
                LegendMaxWidth = 200,
                LegendMaxHeight = 800,
            };
            ChartModel.Legends.Add(legend);

            var series = new LineSeries
            {
                Title = "Первые данные",
                MarkerType = MarkerType.Circle,
                LabelFormatString = "{1:0.00}",
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
            };
            series.Points.Add(new DataPoint(0, 0));
            series.Points.Add(new DataPoint(1, 2));
            series.Points.Add(new DataPoint(2, 1));
            series.Points.Add(new DataPoint(3, 4));
            series.Points.Add(new DataPoint(4, 3));

            var series2 = new LineSeries
            {
                Title = "Вторые данные",
                MarkerType = MarkerType.Square,
                Color = OxyColors.Black,
            };
            series2.Points.Add(new DataPoint(0, 2));
            series2.Points.Add(new DataPoint(1, 1));
            series2.Points.Add(new DataPoint(2, 3));
            series2.Points.Add(new DataPoint(3, 2));
            series2.Points.Add(new DataPoint(4, 0));

            ChartModel.Series.Add(series);
            ChartModel.Series.Add(series2);

            ChartModel.Axes.Add(new LinearAxis 
            {   
                Position = AxisPosition.Bottom, 
                Title = "X",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
            });
            ChartModel.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Title = "Y",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot, 
            });
        }
        public void UpdateData(List<(double X, double Y)> newData)
        {
            if (ChartModel.Series.Count > 0 && ChartModel.Series[0] is LineSeries lineSeries)
            {
                lineSeries.Points.Clear();
                foreach (var point in newData)
                {
                    lineSeries.Points.Add(new DataPoint(point.X, point.Y));
                }
                ChartModel.InvalidatePlot(true);
            }
        }

        public void UpdateDataMultipleSeries(List<(string Label, List<(double X, double Y)> Points)> seriesData)
        {
            ChartModel.Series.Clear(); // Очищаем старые серии
            ChartModel.Axes.Clear();   // Очищаем старые оси
            ChartModel.Legends.Clear();

            var legend = new Legend()
            {
                LegendBorder = OxyColors.Black,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Vertical,
                LegendItemOrder = LegendItemOrder.Normal,
                LegendItemAlignment = HorizontalAlignment.Left,
                LegendSymbolPlacement = LegendSymbolPlacement.Left,
                LegendMaxWidth = 200,
                LegendMaxHeight = 800,
            };
            ChartModel.Legends.Add(legend);

            // Добавляем оси
            ChartModel.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Bottom, 
                Title = "Значение X",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                TickStyle = TickStyle.Crossing,
                AxislineStyle = LineStyle.Solid,
                AxislineThickness = 2,
                PositionAtZeroCrossing = true,
                Minimum = -2,
            });
            ChartModel.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Title = "Значение Y",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                TickStyle = TickStyle.Crossing,
                AxislineStyle = LineStyle.Solid,
                AxislineThickness = 2,
                PositionAtZeroCrossing = true,
            });

            ChartModel.PlotAreaBorderThickness = new OxyThickness(0);
            ChartModel.PlotMargins = new OxyThickness(10);

            // Цвета для разных серий (можно сделать более гибко)
            var colors = new OxyColor[] { OxyColors.Blue, OxyColors.Red, OxyColors.Green, OxyColors.Orange, OxyColors.Purple };

            for (int i = 0; i < seriesData.Count; i++)
            {
                var seriesInfo = seriesData[i];
                var series = new LineSeries
                {
                    Title = seriesInfo.Label,
                    MarkerType = MarkerType.Square, // Или Circle, если нужны точки
                    Color = colors[i % colors.Length], // Циклически выбираем цвет
                    LabelFormatString = "{1:0.00}",
                    FontSize = 20,
                    InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,                    
                };

                foreach (var point in seriesInfo.Points)
                {
                    series.Points.Add(new DataPoint(point.X, point.Y));
                }

                ChartModel.Series.Add(series);
            }

            ChartModel.InvalidatePlot(true);
        }        
    }
}