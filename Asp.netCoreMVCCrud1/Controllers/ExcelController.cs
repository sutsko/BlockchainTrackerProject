using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Asp.netCoreMVCCrud1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Asp.netCoreMVCCrud1.Controllers
{
    public class ExcelController : Controller
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ExcelController (IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        /// <summary>
        /// /Home/FileReport
        /// </summary>
        public VirtualFileResult FileReport()
        {
            var fileDownloadName = "report.xlsx";
            var reportsFolder = "reports";

            using (var package = createExcelPackage())
            {
                package.SaveAs(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName)));
            }
            return File($"~/{reportsFolder}/{fileDownloadName}", XlsxContentType, fileDownloadName);
        }

        /// <summary>
        /// /Home/FileReport
        /// </summary>
        public VirtualFileResult FileReport(List<Project> projects)
        {
            var fileDownloadName = "report.xlsx";
            var reportsFolder = "reports";

            using (var package = createExcelPackage(projects))
            {
                package.SaveAs(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName)));
            }
            return File($"~/{reportsFolder}/{fileDownloadName}", XlsxContentType, fileDownloadName);
        }


        private ExcelPackage createExcelPackage()
        {
            var package = new ExcelPackage();
            //This could maybe be made specific for the user who downloads it.
            package.Workbook.Properties.Title = "Salary Report";
            package.Workbook.Properties.Author = "Vahid N.";
            package.Workbook.Properties.Subject = "Salary Report";
            package.Workbook.Properties.Keywords = "Salary";


            var worksheet = package.Workbook.Worksheets.Add("Employee");

            //First add the headers
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Gender";
            worksheet.Cells[1, 4].Value = "Salary (in $)";

            //Add values

            var numberformat = "#,##0";
            var dataCellStyleName = "TableNumber";
            var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            numStyle.Style.Numberformat.Format = numberformat;

            worksheet.Cells[2, 1].Value = 1000;
            worksheet.Cells[2, 2].Value = "Jon";
            worksheet.Cells[2, 3].Value = "M";
            worksheet.Cells[2, 4].Value = 5000;
            worksheet.Cells[2, 4].Style.Numberformat.Format = numberformat;

            worksheet.Cells[3, 1].Value = 1001;
            worksheet.Cells[3, 2].Value = "Graham";
            worksheet.Cells[3, 3].Value = "M";
            worksheet.Cells[3, 4].Value = 10000;
            worksheet.Cells[3, 4].Style.Numberformat.Format = numberformat;

            worksheet.Cells[4, 1].Value = 1002;
            worksheet.Cells[4, 2].Value = "Jenny";
            worksheet.Cells[4, 3].Value = "F";
            worksheet.Cells[4, 4].Value = 5000;
            worksheet.Cells[4, 4].Style.Numberformat.Format = numberformat;

            // Add to table / Add summary row
            var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: 4, toColumn: 4), "Data");
            tbl.ShowHeader = true;
            tbl.TableStyle = TableStyles.Dark9;
            tbl.ShowTotal = true;
            tbl.Columns[3].DataCellStyleName = dataCellStyleName;
            tbl.Columns[3].TotalsRowFunction = RowFunctions.Sum;
            worksheet.Cells[5, 4].Style.Numberformat.Format = numberformat;

            // AutoFitColumns
            worksheet.Cells[1, 1, 4, 4].AutoFitColumns();


            return package;
        }

        private ExcelPackage createExcelPackage(List<Project> projects )
        {
            var package = new ExcelPackage();
            //This could maybe be made specific for the user who downloads it.
            package.Workbook.Properties.Title = "Project Report";
            package.Workbook.Properties.Author = "Deloitte NexGen DK";
            package.Workbook.Properties.Subject = "Blockchain Projects Report";
            package.Workbook.Properties.Keywords = "Blockchain tracker";


            var worksheet = package.Workbook.Worksheets.Add("ListOfProjects");

            //First add the headers
            worksheet.Cells[1, 1].Value = "Article Headline";
            worksheet.Cells[1, 2].Value = "Article URL";
            worksheet.Cells[1, 3].Value = "Article Description";
            worksheet.Cells[1, 4].Value = "Article Date";
            worksheet.Cells[1, 5].Value = "Organization"; 
            worksheet.Cells[1, 6].Value = "Country";
            worksheet.Cells[1, 7].Value = "Industry";
            worksheet.Cells[1, 8].Value = "Use Case";
            worksheet.Cells[1, 9].Value = "Maturity";
            worksheet.Cells[1, 10].Value = "Technical Vendor";

            //Add values

            var numberformat = "#,##0";
            var dataCellStyleName = "TableNumber";
            var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            numStyle.Style.Numberformat.Format = numberformat;

            for (int i = 0; i < projects.Count; i++)
            {
                //i+2 since the first row is for headers and index starts at 0
                worksheet.Cells[i+2, 1].Value = projects[i].ArticleHeadline;
                worksheet.Cells[i+2, 2].Value = projects[i].ArticleUrl;
                worksheet.Cells[i+2, 3].Value = projects[i].ArticleDescription;
                worksheet.Cells[i+2, 4].Value = projects[i].ArticleDate;
                worksheet.Cells[i+2, 4].Style.Numberformat.Format = "dd-mm-yyyy";
                worksheet.Cells[i+2, 5].Value = projects[i].Organization.OrganizationName;
                worksheet.Cells[i+2, 6].Value = projects[i].Country;
                worksheet.Cells[i+2, 7].Value = projects[i].Industry.IndustryName;
                worksheet.Cells[i+2, 8].Value = projects[i].Usecase.UsecaseName;
                worksheet.Cells[i+2, 9].Value = projects[i].Maturity;
                worksheet.Cells[i+2, 10].Value = projects[i].TechnicalVendor;
            }

            // Add to table / Add summary row
            var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: projects.Count+2, toColumn: 10), "Data");
            tbl.ShowHeader = true;
            tbl.TableStyle = TableStyles.Dark9;
            //tbl.ShowTotal = true;
            //tbl.Columns[3].DataCellStyleName = dataCellStyleName;
            //tbl.Columns[3].TotalsRowFunction = RowFunctions.Sum;
            

            // AutoFitColumns
            worksheet.Cells[1, 1, projects.Count+2, 10].AutoFitColumns();


            return package;
        }


    }
}