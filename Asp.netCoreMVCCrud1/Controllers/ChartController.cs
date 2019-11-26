using System;
using System.Collections.Generic;
using System.Linq;
using Asp.netCoreMVCCrud1.Models;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Mvc;


namespace Asp.netCoreMVCCrud1.Controllers
{
    
    public class ChartController : Controller
    {

        public ChartController()
        {
           
        }



        [ActionName("Index")]
        public IActionResult Index(List<Project> listOfProjects)
        {
            
            Chart barChartForUsecase = GenerateBarChartForUsecase(listOfProjects);
            Chart barChartForMaturity = GenerateBarChartForMaturity(listOfProjects);
            Chart barChartForYearOnYear = GenerateBarChartForYearOnYear(listOfProjects);
            Chart barChartForIndustry= GenerateBarChartForIndustry(listOfProjects);
            Chart lineChart = GenerateLineChart(listOfProjects);
            Chart lineScatterChart = GenerateLineScatterChart(listOfProjects);
            Chart radarChart = GenerateRadarChart(listOfProjects);
            Chart polarChart = GeneratePolarChart(listOfProjects);
            Chart pieChart = GeneratePieChart(listOfProjects);
            Chart nestedDoughnutChart = GenerateNestedDoughnutChart(listOfProjects);

            ViewData["BarChartForUsecase"] = barChartForUsecase;
            ViewData["BarChartForMaturity"] = barChartForMaturity;
            ViewData["BarChartForYearOnYear"] = barChartForYearOnYear;
            ViewData["BarChartForIndustry"] = barChartForIndustry;
            ViewData["LineChart"] = lineChart;
            ViewData["LineScatterChart"] = lineScatterChart;
            ViewData["RadarChart"] = radarChart;
            ViewData["PolarChart"] = polarChart;
            ViewData["PieChart"] = pieChart;
            ViewData["NestedDoughnutChart"] = nestedDoughnutChart;

            return View("~/Views/Project/DataAnalysis.cshtml");
        }

        //Starting with Usecases
      
        //Starting with Industries. Probably should not implement this yet, since there are so f. many industries. Should add if it amount of mentionnings in listofprojects == 0 they should be removed from graph
        /*private static Chart GenerateBarChartForIndustry(List<Project> listOfProjects)
        {

            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;
            Data data = new Data();

            Dictionary<int, string> industryMap = new Dictionary<int, string>();
            Dictionary<string, int> counterMap = new Dictionary<string, int>();
            List<double> countList = new List<double>();
            List<string> industryNames = new List<string>();

            //Setting the different names for the bars
            foreach (Industry i in listOfProjects[0].IndustryList)
            {
                industryNames.Add(i.IndustryName);
                industryMap.Add(i.IndustryId, i.IndustryName);
                counterMap.Add(i.IndustryName, 0);
            }
            data.Labels = industryNames;

            foreach (Project p in listOfProjects)
            {
                if (industryMap.TryGetValue(p.IndustryId, out string indResult))
                {
                    counterMap[indResult]++;
                }
            }

            foreach (var industryName in data.Labels)
            {
                if (counterMap.TryGetValue(industryName, out int indResult))
                {
                    countList.Add(indResult);
                }
            }

            //Maybe this foreach and the one above could be merged?
            List<ChartColor> listOfBackgroundColors = new List<ChartColor>();
            List<ChartColor> listOfBorderColors = new List<ChartColor>();
            Random r = new Random();
            foreach (var industryName in data.Labels)
            {
                listOfBackgroundColors.Add(ChartColor.FromRgba((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255), 0.2));
                listOfBorderColors.Add(ChartColor.FromRgb(listOfBackgroundColors.Last().Red, listOfBackgroundColors.Last().Green, listOfBackgroundColors.Last().Blue));
            }


            BarDataset dataset = new BarDataset()
            {
                Label = "# of Use cases",
                Data = countList,
                BackgroundColor = listOfBackgroundColors,
                BorderColor = listOfBorderColors,
                BorderWidth = new List<int>() { 1 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            var options = new Options
            {
                Scales = new Scales()
            };

            var scales = new Scales
            {
                YAxes = new List<Scale>
                {
                    new CartesianScale
                    {
                        Ticks = new CartesianLinearTick
                        {
                            BeginAtZero = true
                        }
                    }
                },
                XAxes = new List<Scale>
                {
                    new BarScale
                    {
                        BarPercentage = 0.5,
                        BarThickness = 6,
                        MaxBarThickness = 8,
                        MinBarLength = 2,
                        GridLines = new GridLine()
                        {
                            OffsetGridLines = true
                        }
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            chart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };

            return chart;
        }
       */
        private static Chart GenerateBarChartForIndustry(List<Project> listOfProjects)
        {

            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;
            Data data = new Data();

            List<List<Project>> result = listOfProjects
           .GroupBy(p => p.IndustryId)
              .Select(g => g.ToList())
                   .ToList();

            List<string> datalabels = new List<string>();
           
            Dictionary<int, string> industryMap2 = new Dictionary<int, string>();
            Dictionary<string, double> counterMap = new Dictionary<string, double>();

            //Setting the different names for the bars
            foreach (Industry i in listOfProjects[0].IndustryList)
            {
                industryMap2.Add(i.IndustryId, i.IndustryName);
            }

            foreach (var projectListByUsecase in result)
            {
                datalabels.Add(industryMap2[projectListByUsecase[0].IndustryId]);

                counterMap.Add(industryMap2[projectListByUsecase[0].IndustryId], (double)projectListByUsecase.Count);
            }

            List<double> countList = new List<double>();
            data.Labels = datalabels;


            foreach (var industry in data.Labels)
            {
                if (counterMap.TryGetValue(industry, out double indResult))
                {
                    countList.Add(indResult);
                }
            }


            //Maybe this foreach and the one above could be merged?
            List<ChartColor> listOfBackgroundColors = new List<ChartColor>();
            List<ChartColor> listOfBorderColors = new List<ChartColor>();
            Random r = new Random();
            foreach (var usecaseName in data.Labels)
            {
                listOfBackgroundColors.Add(ChartColor.FromRgba((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255), 0.2));
                listOfBorderColors.Add(ChartColor.FromRgb(listOfBackgroundColors.Last().Red, listOfBackgroundColors.Last().Green, listOfBackgroundColors.Last().Blue));
            }


            BarDataset dataset = new BarDataset()
            {
                Label = "# of Industry usages",
                Data = countList,
                BackgroundColor = listOfBackgroundColors,
                BorderColor = listOfBorderColors,
                BorderWidth = new List<int>() { 1 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;


            var options = new Options
            {
                Scales = new Scales()
            };

            var scales = new Scales
            {
                YAxes = new List<Scale>
                {
                    new CartesianScale
                    {
                        Ticks = new CartesianLinearTick
                        {
                            BeginAtZero = true
                        }
                    }
                },
                XAxes = new List<Scale>
                {
                    new BarScale
                    {
                        BarPercentage = 0.5,
                        BarThickness = 6,
                        MaxBarThickness = 8,
                        MinBarLength = 2,
                        GridLines = new GridLine()
                        {
                            OffsetGridLines = true
                        }
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            chart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };

            return chart;
        }

        private static Chart GenerateBarChartForUsecase(List<Project> listOfProjects)
        {

            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;
            Data data = new Data();

            List<List<Project>> result = listOfProjects
           .GroupBy(p => p.UseCaseId)
              .Select(g => g.ToList())
                   .ToList();

            List<string> datalabels = new List<string>();
            Dictionary<int, List<Project>> usecaseMap = new Dictionary<int, List<Project>>();
            Dictionary<int, string> usecaseMap2 = new Dictionary<int, string>();
            Dictionary<string, double> counterMap = new Dictionary<string, double>();

            //Setting the different names for the bars
            foreach (Usecase uc in listOfProjects[0].UsecaseList)
            {
                usecaseMap2.Add(uc.UsecaseId, uc.UsecaseName);
            }

            foreach (var projectListByUsecase in result)
            {
                datalabels.Add(usecaseMap2[projectListByUsecase[0].UseCaseId]); 

                counterMap.Add(usecaseMap2[projectListByUsecase[0].UseCaseId], (double)projectListByUsecase.Count);
            }

            List<double> countList = new List<double>();
            data.Labels = datalabels;


            foreach (var usecase in data.Labels)
            {
                if (counterMap.TryGetValue(usecase, out double indResult))
                {
                    countList.Add(indResult);
                }
            }


            //Maybe this foreach and the one above could be merged?
            List<ChartColor> listOfBackgroundColors = new List<ChartColor>();
            List<ChartColor> listOfBorderColors = new List<ChartColor>();
            Random r = new Random();
            foreach (var usecaseName in data.Labels)
            {
                listOfBackgroundColors.Add(ChartColor.FromRgba((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255), 0.2));
                listOfBorderColors.Add(ChartColor.FromRgb(listOfBackgroundColors.Last().Red, listOfBackgroundColors.Last().Green, listOfBackgroundColors.Last().Blue));
            }


            BarDataset dataset = new BarDataset()
            {
                Label = "# of Use cases",
                Data = countList,
                BackgroundColor = listOfBackgroundColors,
                BorderColor = listOfBorderColors,
                BorderWidth = new List<int>() { 1 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            var options = new Options
            {
                Scales = new Scales()
            };

            var scales = new Scales
            {
                YAxes = new List<Scale>
                {
                    new CartesianScale
                    {
                        Ticks = new CartesianLinearTick
                        {
                            BeginAtZero = true
                        }
                    }
                },
                XAxes = new List<Scale>
                {
                    new BarScale
                    {
                        BarPercentage = 0.5,
                        BarThickness = 6,
                        MaxBarThickness = 8,
                        MinBarLength = 2,
                        GridLines = new GridLine()
                        {
                            OffsetGridLines = true
                        }
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            chart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };

            return chart;
        }

        private static Chart GenerateBarChartForMaturity(List<Project> listOfProjects)
        {

            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;
            Data data = new Data();

            List<List<Project>> result = listOfProjects
             .GroupBy(p => p.Maturity)
                .Select(g => g.ToList())
                     .ToList();

            List<string> datalabels = new List<string>();
            Dictionary<string, List<Project>> maturityMap = new Dictionary<string, List<Project>>();
            Dictionary<string, double> counterMap = new Dictionary<string, double>();

            foreach (var projectListByMaturity in result)
            {
                datalabels.Add(projectListByMaturity[0].Maturity);
                maturityMap.Add(projectListByMaturity[0].Maturity, projectListByMaturity);
                counterMap.Add(projectListByMaturity[0].Maturity, (double)projectListByMaturity.Count);
            }

            List<double> countList = new List<double>();
            data.Labels = datalabels;


            foreach (var maturity in data.Labels)
            {
                if (counterMap.TryGetValue(maturity, out double indResult))
                {
                    countList.Add(indResult);
                }
            }

            //Maybe this foreach and the one above could be merged? Since they both iterate in data.labels
            List<ChartColor> listOfBackgroundColors = new List<ChartColor>();
            List<ChartColor> listOfBorderColors = new List<ChartColor>();
            Random r = new Random();
            foreach (var maturityName in data.Labels)
            {
                listOfBackgroundColors.Add(ChartColor.FromRgba((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255), 0.2));
                listOfBorderColors.Add(ChartColor.FromRgb(listOfBackgroundColors.Last().Red, listOfBackgroundColors.Last().Green, listOfBackgroundColors.Last().Blue));
            }


            BarDataset dataset = new BarDataset()
            {
                Label = "# of Maturity",
                Data = countList,
                BackgroundColor = listOfBackgroundColors,
                BorderColor = listOfBorderColors,
                BorderWidth = new List<int>() { 1 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            var options = new Options
            {
                Scales = new Scales()
            };

            var scales = new Scales
            {
                YAxes = new List<Scale>
                {
                    new CartesianScale
                    {
                        Ticks = new CartesianLinearTick
                        {
                            BeginAtZero = true
                        }
                    }
                },
                XAxes = new List<Scale>
                {
                    new BarScale
                    {
                        BarPercentage = 0.5,
                        BarThickness = 6,
                        MaxBarThickness = 8,
                        MinBarLength = 2,
                        GridLines = new GridLine()
                        {
                            OffsetGridLines = true
                        }
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            chart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };

            return chart;
        }

        private static Chart GenerateBarChartForYearOnYear(List<Project> listOfProjects)
        {

            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;
            Data data = new Data();

            List<List<Project>> result = listOfProjects
             .GroupBy(p => p.ArticleDate.Year)
                .Select(g => g.ToList())
                     .ToList();


            List<string> datalabels = new List<string>();
            Dictionary<string, List<Project>> yearMap = new Dictionary<string, List<Project>>();
            Dictionary<string, double> counterMap = new Dictionary<string, double>();

            foreach (var projectListByYear in result)
            {
                datalabels.Add(projectListByYear[0].ArticleDate.Year.ToString());
                yearMap.Add(projectListByYear[0].ArticleDate.Year.ToString(), projectListByYear);
                counterMap.Add(projectListByYear[0].ArticleDate.Year.ToString(), (double) projectListByYear.Count);
            }

        
            
            
            List<double> countList = new List<double>();
            data.Labels = datalabels;


            foreach (var year in data.Labels)
            {
                if (counterMap.TryGetValue(year, out double indResult))
                {
                    countList.Add(indResult);
                }
            }

            //Maybe this foreach and the one above could be merged?
            List<ChartColor> listOfBackgroundColors = new List<ChartColor>();
            List<ChartColor> listOfBorderColors = new List<ChartColor>();
            Random r = new Random();
            foreach (var year in data.Labels)
            {
                listOfBackgroundColors.Add(ChartColor.FromRgba((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255), 0.2));
                listOfBorderColors.Add(ChartColor.FromRgb(listOfBackgroundColors.Last().Red, listOfBackgroundColors.Last().Green, listOfBackgroundColors.Last().Blue));
            }


            BarDataset dataset = new BarDataset()
            {
                Label = "# of projects per year",
                Data = countList,
                BackgroundColor = listOfBackgroundColors,
                BorderColor = listOfBorderColors,
                BorderWidth = new List<int>() { 1 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            var options = new Options
            {
                Scales = new Scales()
            };

            var scales = new Scales
            {
                YAxes = new List<Scale>
                {
                    new CartesianScale
                    {
                        Ticks = new CartesianLinearTick
                        {
                            BeginAtZero = true
                        }
                    }
                },
                XAxes = new List<Scale>
                {
                    new BarScale
                    {
                        BarPercentage = 0.5,
                        BarThickness = 6,
                        MaxBarThickness = 8,
                        MinBarLength = 2,
                        GridLines = new GridLine()
                        {
                            OffsetGridLines = true
                        }
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            chart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };

            return chart;
        }

        private static Chart GenerateLineChart(List<Project> listOfProjects)
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Line;

            Data data = new Data();
            data.Labels = new List<string>() { "January", "February", "March", "April", "May", "June", "July" };

            LineDataset dataset = new LineDataset()
            {
                Label = "My First dataset",
                Data = new List<double>() { 65, 59, 80, 81, 56, 55, 40 },
                Fill = "false",
                LineTension = 0.1,
                BackgroundColor = ChartColor.FromRgba(75, 192, 192, 0.4),
                BorderColor = ChartColor.FromRgba(75, 192, 192, 1),
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<ChartColor>() { ChartColor.FromRgba(75, 192, 192, 1) },
                PointBackgroundColor = new List<ChartColor>() { ChartColor.FromHexString("#fff") },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor>() { ChartColor.FromRgba(75, 192, 192, 1) },
                PointHoverBorderColor = new List<ChartColor>() { ChartColor.FromRgba(220, 220, 220, 1) },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            Options options = new Options()
            {
                Scales = new Scales()
            };

            Scales scales = new Scales()
            {
                YAxes = new List<Scale>()
                {
                    new CartesianScale()
                }
            };

            CartesianScale yAxes = new CartesianScale()
            {
                Ticks = new Tick()
            };

            Tick tick = new Tick()
            {
                Callback = "function(value, index, values) {return '$' + value;}"
            };

            yAxes.Ticks = tick;
            scales.YAxes = new List<Scale>() { yAxes };
            options.Scales = scales;
            chart.Options = options;

            chart.Data = data;

            return chart;
        }

        private static Chart GenerateLineScatterChart(List<Project> listOfProjects)
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Line;

            Data data = new Data();

            LineScatterDataset dataset = new LineScatterDataset()
            {
                Label = "Scatter Dataset",
                Data = new List<LineScatterData>()
            };

            LineScatterData scatterData1 = new LineScatterData();
            LineScatterData scatterData2 = new LineScatterData();
            LineScatterData scatterData3 = new LineScatterData();

            scatterData1.X = "-10";
            scatterData1.Y = "0";
            dataset.Data.Add(scatterData1);

            scatterData2.X = "0";
            scatterData2.Y = "10";
            dataset.Data.Add(scatterData2);

            scatterData3.X = "10";
            scatterData3.Y = "5";
            dataset.Data.Add(scatterData3);

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            Options options = new Options()
            {
                Scales = new Scales()
            };

            Scales scales = new Scales()
            {
                XAxes = new List<Scale>()
                {
                    new CartesianScale()
                    {
                        Type = "linear",
                        Position = "bottom",
                        Ticks = new CartesianLinearTick()
                        {
                            BeginAtZero = true
                        }
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            return chart;
        }

        private static Chart GenerateRadarChart(List<Project> listOfProjects)
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Radar;

            Data data = new Data();
            data.Labels = new List<string>() { "Eating", "Drinking", "Sleeping", "Designing", "Coding", "Cycling", "Running" };

            RadarDataset dataset1 = new RadarDataset()
            {
                Label = "My First dataset",
                BackgroundColor = ChartColor.FromRgba(179, 181, 198, 0.2),
                BorderColor = ChartColor.FromRgba(179, 181, 198, 1),
                PointBackgroundColor = new List<ChartColor>() { ChartColor.FromRgba(179, 181, 198, 1) },
                PointBorderColor = new List<ChartColor>() { ChartColor.FromHexString("#fff") },
                PointHoverBackgroundColor = new List<ChartColor>() { ChartColor.FromHexString("#fff") },
                PointHoverBorderColor = new List<ChartColor>() { ChartColor.FromRgba(179, 181, 198, 1) },
                Data = new List<double>() { 65, 59, 80, 81, 56, 55, 40 }
            };

            RadarDataset dataset2 = new RadarDataset()
            {
                Label = "My Second dataset",
                BackgroundColor = ChartColor.FromRgba(255, 99, 132, 0.2),
                BorderColor = ChartColor.FromRgba(255, 99, 132, 1),
                PointBackgroundColor = new List<ChartColor>() { ChartColor.FromRgba(255, 99, 132, 1) },
                PointBorderColor = new List<ChartColor>() { ChartColor.FromHexString("#fff") },
                PointHoverBackgroundColor = new List<ChartColor>() { ChartColor.FromHexString("#fff") },
                PointHoverBorderColor = new List<ChartColor>() { ChartColor.FromRgba(255, 99, 132, 1) },
                Data = new List<double>() { 28, 48, 40, 19, 96, 27, 100 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset1);
            data.Datasets.Add(dataset2);

            chart.Data = data;

            return chart;
        }

        private static Chart GeneratePolarChart(List<Project> listOfProjects)
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.PolarArea;

            Data data = new Data();
            data.Labels = new List<string>() { "Red", "Green", "Yellow", "Grey", "Blue" };

            PolarDataset dataset = new PolarDataset()
            {
                Label = "My dataset",
                BackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#FF6384"),
                    ChartColor.FromHexString("#4BC0C0"),
                    ChartColor.FromHexString("#FFCE56"),
                    ChartColor.FromHexString("#E7E9ED"),
                    ChartColor.FromHexString("#36A2EB")
                },
                Data = new List<double>() { 11, 16, 7, 3, 14 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            return chart;
        }

        private static Chart GeneratePieChart(List<Project> listOfProjects)
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Pie;

            Data data = new Data();
            data.Labels = new List<string>() { "Red", "Blue", "Yellow" };

            PieDataset dataset = new PieDataset()
            {
                Label = "My dataset",
                BackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#FF6384"),
                    ChartColor.FromHexString("#36A2EB"),
                    ChartColor.FromHexString("#FFCE56")
                },
                HoverBackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#FF6384"),
                    ChartColor.FromHexString("#36A2EB"),
                    ChartColor.FromHexString("#FFCE56")
                },
                Data = new List<double>() { 300, 50, 100 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            return chart;
        }

        private static Chart GenerateNestedDoughnutChart(List<Project> listOfProjects)
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Doughnut;

            Data data = new Data();
            data.Labels = new List<string>() {
                "resource-group-1",
                "resource-group-2",
                "Data Services - Basic Database Days",
                "Data Services - Basic Database Days",
                "Azure App Service - Basic Small App Service Hours",
                "resource-group-2 - Other"
            };

            PieDataset outerDataset = new PieDataset()
            {
                BackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#3366CC"),
                    ChartColor.FromHexString("#DC3912"),
                    ChartColor.FromHexString("#FF9900"),
                    ChartColor.FromHexString("#109618"),
                    ChartColor.FromHexString("#990099"),
                    ChartColor.FromHexString("#3B3EAC")
                },
                HoverBackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#3366CC"),
                    ChartColor.FromHexString("#DC3912"),
                    ChartColor.FromHexString("#FF9900"),
                    ChartColor.FromHexString("#109618"),
                    ChartColor.FromHexString("#990099"),
                    ChartColor.FromHexString("#3B3EAC")
                },
                Data = new List<double>() {
                    0.0,
                    0.0,
                    8.31,
                    10.43,
                    84.69,
                    0.84
                }
            };

            PieDataset innerDataset = new PieDataset()
            {
                BackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#3366CC"),
                    ChartColor.FromHexString("#DC3912"),
                    ChartColor.FromHexString("#FF9900"),
                    ChartColor.FromHexString("#109618"),
                    ChartColor.FromHexString("#990099"),
                    ChartColor.FromHexString("#3B3EAC")
                },
                HoverBackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#3366CC"),
                    ChartColor.FromHexString("#DC3912"),
                    ChartColor.FromHexString("#FF9900"),
                    ChartColor.FromHexString("#109618"),
                    ChartColor.FromHexString("#990099"),
                    ChartColor.FromHexString("#3B3EAC")
                },
                Data = new List<double>() {
                    8.31,
                    95.96
                }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(outerDataset);
            data.Datasets.Add(innerDataset);

            chart.Data = data;

            return chart;
        }

        /* private static Chart GenerateBarChartForMaturity(List<Project> listOfProjects)
        {

            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;
            Data data = new Data();

            Dictionary<string, string> maturityMap = new Dictionary<string, string>();
            Dictionary<string, int> counterMap = new Dictionary<string, int>();
            List<double> countList = new List<double>();
            data.Labels = new List<string> {"Under Development",  "Test", "Operational", "Stalled"};

            //Setting the different names for the bars
            foreach (string maturity in data.Labels )
            {
                maturityMap.Add(maturity, maturity);
                counterMap.Add(maturity, 0);
            }
             

            foreach (Project p in listOfProjects)
            {
                if (maturityMap.TryGetValue(p.Maturity, out string indResult))
                {
                    counterMap[indResult]++;
                }
            }

            foreach (var maturityName in data.Labels)
            {
                if (counterMap.TryGetValue(maturityName, out int indResult))
                {
                    countList.Add(indResult);
                }
            }

            //Maybe this foreach and the one above could be merged?
            List<ChartColor> listOfBackgroundColors = new List<ChartColor>();
            List<ChartColor> listOfBorderColors = new List<ChartColor>();
            Random r = new Random();
            foreach (var maturityName in data.Labels)
            {
                listOfBackgroundColors.Add(ChartColor.FromRgba((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255), 0.2));
                listOfBorderColors.Add(ChartColor.FromRgb(listOfBackgroundColors.Last().Red, listOfBackgroundColors.Last().Green, listOfBackgroundColors.Last().Blue));
            }


            BarDataset dataset = new BarDataset()
            {
                Label = "# of Maturity",
                Data = countList,
                BackgroundColor = listOfBackgroundColors,
                BorderColor = listOfBorderColors,
                BorderWidth = new List<int>() { 1 }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            var options = new Options
            {
                Scales = new Scales()
            };

            var scales = new Scales
            {
                YAxes = new List<Scale>
                {
                    new CartesianScale
                    {
                        Ticks = new CartesianLinearTick
                        {
                            BeginAtZero = true
                        }
                    }
                },
                XAxes = new List<Scale>
                {
                    new BarScale
                    {
                        BarPercentage = 0.5,
                        BarThickness = 6,
                        MaxBarThickness = 8,
                        MinBarLength = 2,
                        GridLines = new GridLine()
                        {
                            OffsetGridLines = true
                        }
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            chart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };

            return chart;
        }
    */
        /*private static Chart GenerateBarChartForUsecases(List<Project> listOfProjects)
          {

              Chart chart = new Chart();
              chart.Type = Enums.ChartType.Bar;
              Data data = new Data();

              Dictionary<int, string> usecaseMap = new Dictionary<int, string>();
              Dictionary<string, int> counterMap = new Dictionary<string, int>();
              List<double> countList = new List<double>();
              List<string> usecaseNames = new List<string>();

              //Setting the different names for the bars
              foreach (Usecase uc in listOfProjects[0].UsecaseList)
              {
                  usecaseNames.Add(uc.UsecaseName);
                  usecaseMap.Add(uc.UsecaseId, uc.UsecaseName);
                  counterMap.Add(uc.UsecaseName, 0);
              }
              data.Labels = usecaseNames;

              foreach (Project p in listOfProjects)
              {
                  if (usecaseMap.TryGetValue(p.UseCaseId, out string indResult))
                  {
                      counterMap[indResult]++;
                  }
              }

              foreach (var usecaseName in data.Labels)
              {
                  if (counterMap.TryGetValue(usecaseName, out int indResult))
                  {
                      countList.Add(indResult);
                  }
              }

              //Maybe this foreach and the one above could be merged?
              List<ChartColor> listOfBackgroundColors = new List<ChartColor>();
              List <ChartColor> listOfBorderColors = new List<ChartColor>();
              Random r = new Random();
              foreach (var usecaseName in data.Labels)
              {
                  listOfBackgroundColors.Add(ChartColor.FromRgba((byte) r.Next(1, 255), (byte) r.Next(1, 255), (byte) r.Next(1, 255), 0.2));
                  listOfBorderColors.Add(ChartColor.FromRgb(listOfBackgroundColors.Last().Red, listOfBackgroundColors.Last().Green, listOfBackgroundColors.Last().Blue));
              }


              BarDataset dataset = new BarDataset()
              {
                  Label = "# of Use cases",
                  Data = countList,
                  BackgroundColor = listOfBackgroundColors,
                  BorderColor = listOfBorderColors,
                  BorderWidth = new List<int>() { 1 }
              };

              data.Datasets = new List<Dataset>();
              data.Datasets.Add(dataset);

              chart.Data = data;

              var options = new Options
              {
                  Scales = new Scales()
              };

              var scales = new Scales
              {
                  YAxes = new List<Scale>
                  {
                      new CartesianScale
                      {
                          Ticks = new CartesianLinearTick
                          {
                              BeginAtZero = true
                          }
                      }
                  },
                  XAxes = new List<Scale>
                  {
                      new BarScale
                      {
                          BarPercentage = 0.5,
                          BarThickness = 6,
                          MaxBarThickness = 8,
                          MinBarLength = 2,
                          GridLines = new GridLine()
                          {
                              OffsetGridLines = true
                          }
                      }
                  }
              };

              options.Scales = scales;

              chart.Options = options;

              chart.Options.Layout = new Layout
              {
                  Padding = new Padding
                  {
                      PaddingObject = new PaddingObject
                      {
                          Left = 10,
                          Right = 12
                      }
                  }
              };

              return chart;
          }
          */
    }
}