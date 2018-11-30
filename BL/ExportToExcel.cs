using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using Excel = Microsoft.Office.Interop.Excel;


namespace BL
{
    public static class ExportToExcel
    {
        public static void ToExcel(this IEnumerable<Trainee> allTrainee, Func<Trainee,bool> predicate=null)
        {
            if(predicate!=null)
                allTrainee = allTrainee.Where(predicate);

            var excelApp = new Excel.Application {Visible = false};
            // Make the object visible.

            // Create a new, empty workbook and add it to the collection returned 
            // by property Workbooks. The new workbook becomes the active workbook.
            // Add has an optional parameter for specifying a praticular template. 
            // Because no argument is sent in this example, Add creates a new workbook. 
            excelApp.Workbooks.Add();

            // This example uses a single workSheet. The explicit type casting is
            // removed in a later procedure.
            Excel._Worksheet workSheet = (Excel.Worksheet) excelApp.ActiveSheet;

//excelApp.Visible = true;
            excelApp.Windows.Application.ActiveWindow.DisplayGridlines = false;

            workSheet.Name = "Trainees";
            // Establish column headings in cells A1 and B1.
            workSheet.Cells[1, "A"] = "ID Number";
            workSheet.Cells[1, "A"].EntireColumn.NumberFormat = "@";
            workSheet.Cells[1, "C"] = "First Name";
            workSheet.Cells[1, "D"] = "Last Name";
            workSheet.Cells[1, "E"] = "Address";
            workSheet.Cells[1, "F"] = "Phone Number";
            workSheet.Cells[1, "F"].EntireColumn.NumberFormat = "@";
            workSheet.Cells[1, "G"] = "Gender";
            workSheet.Cells[1, "H"] = "Tester ID";
            workSheet.Cells[1, "H"].EntireColumn.NumberFormat = "@";
            workSheet.Cells[1, "I"] = "School Name";
            workSheet.Cells[1, "J"] = "License Type";
            workSheet.Cells[1, "K"] = "Gear Type";
            workSheet.Cells[1, "L"] = "Birth date";

            
            for (char index = 'A'; index < 'M'; index++)
            {
                workSheet.Cells[1, index.ToString()].Font.Bold = true;
                workSheet.Cells[1, index.ToString()].Interior.Color =
                    System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            }
  
  

        

  
            var row = 1;
            foreach (var trainee in allTrainee)
            {
                string licenseType = "";
                foreach (var license in trainee.LicenceTypeLearning)
                {
                    licenseType += license.ToString()+" ";
                }
                row++;
                workSheet.Cells[row, "A"] = trainee.ID;
                workSheet.Cells[row, "B"] = trainee.FirstName;
                workSheet.Cells[row, "C"] = trainee.LastName;
                workSheet.Cells[row, "D"] = trainee.Email;
                workSheet.Cells[row, "E"] = trainee.Address.ToString();
                workSheet.Cells[row, "F"] = trainee.PhoneNumber;
                workSheet.Cells[row, "G"] = trainee.Gender.ToString();
                workSheet.Cells[row, "H"] = (trainee.TesterName!=null)? trainee.TesterName.ID.ToString() : "";
                workSheet.Cells[row, "I"] = trainee.SchoolName;
                workSheet.Cells[row, "J"] = licenseType;
                workSheet.Cells[row, "K"] = trainee.GearType.ToString();
                workSheet.Cells[row, "L"] = trainee.BirthDate.ToString("d");
            }
            workSheet.Range["A1", "L"+row].Borders.Weight = Excel.XlBorderWeight.xlThin;

            for (int index =1; index < 13; index++)
            {
                workSheet.Columns[index].AutoFit();
               
            }
  
            excelApp.Visible = true;
            //excelApp.ActiveWorkbook.SaveAs(@"C:\Users\Elisja\Google Drive\Lev\שנה ב\sss דדד\מבנה נתונים ותכניות ב\תרגילי ריצה\Home Work 3\DATA");
            //excelApp.Workbooks.Close();
            //excelApp.Quit();
            // excelApp.Save(@"C:\Users\Elisja\Google Drive\Lev\שנה ב\sss דדד\מבנה נתונים ותכניות ב\תרגילי ריצה\Home Work 3");

        }
    }
}
