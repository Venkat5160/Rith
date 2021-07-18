using SDAE.Data.Model.Excel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SDAE.Common
{
    public static class Helper
    {

        public static string SaveImage(this string imgBase64, string ImgName, string imgStorePath)
        {
            if (string.IsNullOrEmpty(imgBase64))
                return string.Empty;

            bool isBase64 = IsBase64String(imgBase64);
            if (isBase64)
            {
                if (!Directory.Exists(imgStorePath))
                {
                    Directory.CreateDirectory(imgStorePath);
                }
                string imageName = ImgName + ".jpg";
                string imgPath = Path.Combine(imgStorePath, imageName);
                byte[] imageBytes = Convert.FromBase64String(imgBase64);
                File.WriteAllBytes(imgPath, imageBytes);
                return imageName;
            }
            else
                return Path.GetFileName(imgBase64);
        }

        public static bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }
    }

    public static class ExcelHelper
    {
        public static byte[] GenerateExcel(List<AgentsCallDto> lstModel, string sheetName)
        {
            var workBook = new XLWorkbook();
            DataTable dataTable = new DataTable();
            dataTable.TableName = sheetName;
            List<ExcelAttribute> lst = new List<ExcelAttribute>();
            if (lstModel != null && Enumerable.Count(lstModel) > 0)
            {
                dataTable.Columns.Add("S. No");
                dataTable.Columns.Add("Agent");
                dataTable.Columns.Add("Date & Time");
                dataTable.Columns.Add("Country");
                dataTable.Columns.Add("Language");
                dataTable.Columns.Add("Brand");
                dataTable.Columns.Add("Retailer");
                dataTable.Columns.Add("Product");
                dataTable.Columns.Add("Rating");
                dataTable.Columns.Add("Notes");
                dataTable.Columns.Add("Duration");
                dataTable.Columns.Add("Call Status");
                //lst = GetColumnsBasedOnObject(lstModel, lst);
                //foreach (var column in lst)
                //{
                //    if (column.DataType == ExcelDataType.Number)
                //        dataTable.Columns.Add(column.Description, typeof(System.Int32));
                //    else
                //        dataTable.Columns.Add(column.Description);
                //}
                int i = 1;
                foreach (var item in lstModel)
                {
                    DataRow dr = dataTable.NewRow();
                    dr[0] = i;
                    dr[1] = item.Agent;
                    dr[2] = item.DateTimeValue;
                    dr[3] = item.Country;
                    dr[4] = item.Language;
                    dr[5] = item.Brand;
                    dr[6] = item.Retailer;
                    dr[7] = item.Product;
                    dr[8] = item.Rating;
                    dr[9] = item.Notes;
                    dr[10] = item.CalDuration;
                    dr[11] = item.CallStatus;
                    //foreach (var dataRow in lst)
                    //{
                    //    PropertyInfo propertyInfo = ((object)item).GetType().GetProperty(dataRow.PropertyName);
                    //    var rowValue = propertyInfo.GetValue(item);
                    //    dr[dataRow.Description] = rowValue != null ? rowValue : "";
                    //}
                    dataTable.Rows.Add(dr);
                    i++;
                }
            }
            else
            {
                dataTable.Columns.Add("No Records");
                DataRow dr = dataTable.NewRow();
                dr["No Records"] = "No Records";
                dataTable.Rows.Add(dr);
            }
            workBook.AddWorksheet(dataTable);
            var workbookBytes = new byte[0];
            using (var ms = new MemoryStream())
            {
                workBook.SaveAs(ms);
                workbookBytes = ms.ToArray();
            }
            return workbookBytes;
        }

        public static byte[] GenerateAgentExcel(List<AgentDto> lstModel, string sheetName)
        {
            var workBook = new XLWorkbook();
            DataTable dataTable = new DataTable { TableName = sheetName };

            if (lstModel != null && Enumerable.Count(lstModel) > 0)
            {
                dataTable.Columns.Add("S. No");
                dataTable.Columns.Add("Agent");
                dataTable.Columns.Add("Country");
                dataTable.Columns.Add("Time Zone");
                dataTable.Columns.Add("Mobile");
                dataTable.Columns.Add("Email");
                dataTable.Columns.Add("Retailer");
                int i = 1;
                foreach (var item in lstModel)
                {
                    DataRow dr = dataTable.NewRow();
                    dr[0] = i;
                    dr[1] = item.FirstName + " " + item.LastName;
                    dr[2] = item.Country;
                    dr[3] = item.TimeZoneName;
                    dr[4] = item.Mobile;
                    dr[5] = item.Email;
                    dr[6] = item.Retailer;
                    dataTable.Rows.Add(dr);
                    i++;
                }
            }
            else
            {
                dataTable.Columns.Add("No Records");
                DataRow dr = dataTable.NewRow();
                dr["No Records"] = "No Records";
                dataTable.Rows.Add(dr);
            }
            workBook.AddWorksheet(dataTable);
            var workbookBytes = new byte[0];
            using (var ms = new MemoryStream())
            {
                workBook.SaveAs(ms);
                workbookBytes = ms.ToArray();
            }
            return workbookBytes;
        }

        private static List<ExcelAttribute> GetColumnsBasedOnObject(dynamic lstModel, List<ExcelAttribute> lst)
        {
            var singleObject = lstModel[0];
            Type t = ((object)singleObject).GetType();
            var properties = t.GetProperties();
            foreach (var item in properties)
            {
                var propertyInfo = GetPropertyAttribute(item);
                if (propertyInfo != null)
                    lst.Add(propertyInfo);
            }
            lst = lst.OrderBy(x => x.OrderId).ToList();
            return lst;
        }

        private static ExcelAttribute GetPropertyAttribute(PropertyInfo propertyInfo)
        {
            ExcelAttribute excelAttribute = (ExcelAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(ExcelAttribute));
            if (excelAttribute != null)
            {
                excelAttribute.PropertyName = propertyInfo.Name;
                return excelAttribute;
            }
            else
                return null;
        }
    }
}
