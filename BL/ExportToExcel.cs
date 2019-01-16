using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BE.MainObjects;
using Microsoft.Office.Interop.Excel;

namespace BL
{
    /// <summary>
    ///     Export the data to excel
    /// </summary>
    public static class ExportToExcel
    {
        /// <summary>
        ///     Export all Trainees to excel file
        /// </summary>
        /// <param name="allTrainee">All trainee</param>
        /// <param name="predicate">Witch trainee</param>
        public static void ToExcel(this IEnumerable<Trainee> allTrainee, Func<Trainee, bool> predicate = null)
        {
            //get all the trainees that are like the predicate
            if (predicate != null)
                allTrainee = allTrainee.Where(predicate);

            //Make a new Excel object
            var excelApp = new Application {Visible = false};

            //Create a new WorkBook
            excelApp.Workbooks.Add();

            //Create a work sheet
            _Worksheet workSheet = (Worksheet) excelApp.ActiveSheet;

            //remove the grid lines
            excelApp.Windows.Application.ActiveWindow.DisplayGridlines = false;

            //Change the sheet name
            workSheet.Name = "Trainees";

            // Establish column headings 
            workSheet.Cells[1, "A"] = "Id Number";
            workSheet.Cells[1, "A"].EntireColumn.NumberFormat = "@"; //Make it strings
            workSheet.Cells[1, "B"] = "First Name";
            workSheet.Cells[1, "C"] = "Last Name";
            workSheet.Cells[1, "D"] = "Email";
            workSheet.Cells[1, "E"] = "Address";
            workSheet.Cells[1, "F"] = "Phone Number";
            workSheet.Cells[1, "F"].EntireColumn.NumberFormat = "@"; //Make it strings
            workSheet.Cells[1, "G"] = "Gender";
            workSheet.Cells[1, "H"] = "Teacher";
            workSheet.Cells[1, "H"].EntireColumn.NumberFormat = "@"; //Make it strings
            workSheet.Cells[1, "I"] = "School Name";
            workSheet.Cells[1, "J"] = "License Type";
            workSheet.Cells[1, "K"] = "Gear Type";
            workSheet.Cells[1, "L"] = "Birth date";

            //Make them all bold and gray
            for (var index = 'A'; index < 'M'; index++)
            {
                workSheet.Cells[1, index.ToString()].Font.Bold = true;
                workSheet.Cells[1, index.ToString()].Interior.Color =
                    ColorTranslator.ToOle(Color.LightGray);
            }

            //Fill the excel up  
            var row = 1;
            foreach (var trainee in allTrainee)
            {
                //Get all the license types
                var licenseType = "";
                foreach (var license in trainee.LicenseTypeLearning) licenseType += license.License + " ";

                row++;

                //Add all the fields
                workSheet.Cells[row, "A"] = trainee.Id;
                workSheet.Cells[row, "B"] = trainee.FirstName;
                workSheet.Cells[row, "C"] = trainee.LastName;
                workSheet.Cells[row, "D"] = trainee.EmailAddress;
                workSheet.Cells[row, "E"] = trainee.Address != null ? trainee.Address.ToString() : "";
                workSheet.Cells[row, "F"] = trainee.PhoneNumber;
                workSheet.Cells[row, "G"] = trainee.Gender.ToString();
                workSheet.Cells[row, "H"] = trainee.TeacherName ?? "";
                workSheet.Cells[row, "I"] = trainee.SchoolName;
                workSheet.Cells[row, "J"] = licenseType;
                //             workSheet.Cells[row, "K"] = trainee.GearType.ToString();
                workSheet.Cells[row, "L"] = trainee.BirthDate != null ? trainee.BirthDate.ToString("d") : "";
            }

            //Add borders to the Table
            workSheet.Range["A1", "L" + row].Borders.Weight = XlBorderWeight.xlThin;

            //fit the columns
            for (var index = 1; index < 13; index++) workSheet.Columns[index].AutoFit();

            //make visible
            excelApp.Visible = true;
        }

        /// <summary>
        ///     Export all Testers to excel file
        /// </summary>
        /// <param name="allTester">All testers</param>
        /// <param name="predicate">Witch testers</param>
        public static void ToExcel(this IEnumerable<Tester> allTester, Func<Tester, bool> predicate = null)
        {
            //get all the trainees that are like the predicate
            if (predicate != null)
                allTester = allTester.Where(predicate);

            //Make a new Excel object
            var excelApp = new Application {Visible = false};

            //Create a new WorkBook
            excelApp.Workbooks.Add();

            //Create a work sheet
            _Worksheet workSheet = (Worksheet) excelApp.ActiveSheet;

            //remove the grid lines
            excelApp.Windows.Application.ActiveWindow.DisplayGridlines = false;

            //Change the sheet name
            workSheet.Name = "Testers";

            // Establish column headings 
            workSheet.Cells[1, "A"] = "Id Number";
            workSheet.Cells[1, "A"].EntireColumn.NumberFormat = "@"; //Make it strings
            workSheet.Cells[1, "B"] = "First Name";
            workSheet.Cells[1, "C"] = "Last Name";
            workSheet.Cells[1, "D"] = "Email";
            workSheet.Cells[1, "E"] = "Address";
            workSheet.Cells[1, "F"] = "Phone Number";
            workSheet.Cells[1, "F"].EntireColumn.NumberFormat = "@"; //Make it strings
            workSheet.Cells[1, "G"] = "Gender";
            workSheet.Cells[1, "H"] = "Experience";
            workSheet.Cells[1, "I"] = "Max Exams in week";
            workSheet.Cells[1, "J"] = "License Type";
            workSheet.Cells[1, "K"] = "Max distance";
            workSheet.Cells[1, "L"] = "Birth date";

            //Make them all bold and gray
            for (var index = 'A'; index < 'M'; index++)
            {
                workSheet.Cells[1, index.ToString()].Font.Bold = true;
                workSheet.Cells[1, index.ToString()].Interior.Color =
                    ColorTranslator.ToOle(Color.LightGray);
            }

            //Fill the excel up  
            var row = 1;
            foreach (var tester in allTester)
            {
                //Get all the license types
                var licenseType = "";
                foreach (var license in tester.LicenseTypeTeaching) licenseType += license + " ";

                row++;

                //Add all the fields
                workSheet.Cells[row, "A"] = tester.Id;
                workSheet.Cells[row, "B"] = tester.FirstName;
                workSheet.Cells[row, "C"] = tester.LastName;
                workSheet.Cells[row, "D"] = tester.EmailAddress;
                workSheet.Cells[row, "E"] = tester.Address != null ? tester.Address.ToString() : "";
                workSheet.Cells[row, "F"] = tester.PhoneNumber;
                workSheet.Cells[row, "G"] = tester.Gender.ToString();
                workSheet.Cells[row, "H"] = tester.Experience;
                workSheet.Cells[row, "I"] = tester.MaxWeekExams;
                workSheet.Cells[row, "J"] = licenseType;
                workSheet.Cells[row, "K"] = tester.MaxDistance;
                workSheet.Cells[row, "L"] = tester.BirthDate != null ? tester.BirthDate.ToString("d") : "";
            }

            //Add borders to the Table
            workSheet.Range["A1", "L" + row].Borders.Weight = XlBorderWeight.xlThin;

            //fit the columns
            for (var index = 1; index < 13; index++) workSheet.Columns[index].AutoFit();

            //make visible
            excelApp.Visible = true;
        }

        /// <summary>
        ///     Export all Tests to excel file
        /// </summary>
        /// <param name="allTest">All tests</param>
        /// <param name="predicate">witch tests</param>
        public static void ToExcel(this IEnumerable<Test> allTest, Func<Test, bool> predicate = null)
        {
            //get all the trainees that are like the predicate
            if (predicate != null)
                allTest = allTest.Where(predicate);

            //Make a new Excel object
            var excelApp = new Application {Visible = false};

            //Create a new WorkBook
            excelApp.Workbooks.Add();

            //Create a work sheet
            _Worksheet workSheet = (Worksheet) excelApp.ActiveSheet;

            //remove the grid lines
            excelApp.Windows.Application.ActiveWindow.DisplayGridlines = false;

            //Change the sheet name
            workSheet.Name = "Tests";

            // Establish column headings 
            workSheet.Cells[1, "A"] = "Test Id";
            workSheet.Cells[1, "A"].EntireColumn.NumberFormat = "@"; //Make it strings
            workSheet.Cells[1, "B"] = "Trainee Id";
            workSheet.Cells[1, "B"].EntireColumn.NumberFormat = "@"; //Make it strings
            workSheet.Cells[1, "C"] = "Tester Id";
            workSheet.Cells[1, "C"].EntireColumn.NumberFormat = "@"; //Make it strings
            workSheet.Cells[1, "D"] = "Date";
            workSheet.Cells[1, "E"] = "Actual Date";
            workSheet.Cells[1, "F"] = "Address";
            workSheet.Cells[1, "G"] = "Passed";
            workSheet.Cells[1, "H"] = "Route";
            workSheet.Cells[1, "I"] = "Num of Criteria";
            workSheet.Cells[1, "J"] = "License Type";
            workSheet.Cells[1, "K"] = "Comment";

            //Make them all bold and gray
            for (var index = 'A'; index < 'M'; index++)
            {
                workSheet.Cells[1, index.ToString()].Font.Bold = true;
                workSheet.Cells[1, index.ToString()].Interior.Color =
                    ColorTranslator.ToOle(Color.LightGray);
            }

            //Fill the excel up  
            var row = 1;
            foreach (var test in allTest)
            {
                row++;

                //Add all the fields
                workSheet.Cells[row, "A"] = test.Id;
                workSheet.Cells[row, "B"] = test.TraineeId;
                workSheet.Cells[row, "C"] = test.TesterId;
                workSheet.Cells[row, "D"] = test.TestTime != null ? test.TestTime.ToString("d") : "";
                workSheet.Cells[row, "E"] =
                    test.ActualTestTime != DateTime.MinValue ? test.ActualTestTime.ToString("d") : "";
                workSheet.Cells[row, "F"] =
                    test.AddressOfBeginningTest != null ? test.AddressOfBeginningTest.ToString() : "";
                workSheet.Cells[row, "G"] = test.Passed == true ? "Passed" : "Not passed";
                var Cell = workSheet.Cells[row, "H"];
                workSheet.Cells[row, "I"] = test.Criteria.Count;
                workSheet.Cells[row, "J"] = test.LicenseType.ToString();
                workSheet.Cells[row, "K"] = test.Comment;

                if (test.RouteUrl != null)
                    workSheet.Hyperlinks.Add(Cell, test.RouteUrl.AbsoluteUri, Type.Missing, "Route", "Route");
            }

            //Add borders to the Table
            workSheet.Range["A1", "L" + row].Borders.Weight = XlBorderWeight.xlThin;

            //fit the columns
            for (var index = 1; index < 13; index++) workSheet.Columns[index].AutoFit();

            //make visible
            excelApp.Visible = true;
        }
    }
}