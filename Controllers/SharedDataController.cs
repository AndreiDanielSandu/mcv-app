using MilestoneProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MilestoneProject.Controllers
{
    public class SharedDataController : Controller
    {
        // GET: Shared/ReadData
        public List<Comma> commaList = new List<Comma>();
        public List<Hash> hashList = new List<Hash>();
        public List<Hyphen> hyphenList = new List<Hyphen>();
        public List<Shared> SharedViewModel = new List<Shared>();

        public List<Shared> GetData()
        {
            string[] lineTxt = System.IO.File.ReadAllLines(Server.MapPath("~/App_Data/code_exercise_inputs/comma.txt"));
            string[][] elemTxt = new string[lineTxt.Length][];


            for (int i = 0; i < lineTxt.Length; i++)
            {
                elemTxt[i] = lineTxt[i].Split(',').ToArray();

                int k = 0;

                commaList.Add(new Comma
                {
                    CompanyName = elemTxt[i][k++],
                    ConctactName = elemTxt[i][k++],
                    ContactPhoneNo = elemTxt[i][k++],
                    YearsInBusiness = elemTxt[i][k++],
                    ContactEmail = elemTxt[i][k++],
                });
            }

            lineTxt = System.IO.File.ReadAllLines(Server.MapPath("~/App_Data/code_exercise_inputs/hash.txt"));
            elemTxt = new string[lineTxt.Length][];

            for (int i = 0; i < lineTxt.Length; i++)
            {
                elemTxt[i] = lineTxt[i].Split('#').ToArray();

                int k = 0;

                hashList.Add(new Hash
                {
                    CompanyName = elemTxt[i][k++],
                    YearFounded = elemTxt[i][k++],
                    ConctactName = elemTxt[i][k++],
                    ContactPhoneNo = elemTxt[i][k++]
                });
            }

            lineTxt = System.IO.File.ReadAllLines(Server.MapPath("~/App_Data/code_exercise_inputs/hyphen.txt"));
            elemTxt = new string[lineTxt.Length][];

            for (int i = 0; i < lineTxt.Length; i++)
            {
                elemTxt[i] = lineTxt[i].Split('-').ToArray();

                int k = 0;

                hyphenList.Add(new Hyphen
                {
                    CompanyName = elemTxt[i][k++],
                    YearFounded = elemTxt[i][k++],
                    ContactPhoneNo = elemTxt[i][k++],
                    ContactEmail = elemTxt[i][k++],
                    ConctactFirstName = elemTxt[i][k++],
                    ConctactLastName = elemTxt[i][k++]
                });
            }

            SharedViewModel.Add(new Shared
            {
                CommaViewModel = commaList,
                HashViewModel = hashList,
                HyphenViewModel = hyphenList,
            });

            return SharedViewModel;
        }
        public ActionResult ReadData()
        {
            //SharedViewModel = GetData();
            //ViewBag.Data = SharedViewModel.FirstOrDefault();
            ViewBag.Data = SortData(null);

            ViewBag.SortingSelectList = new SelectList(GetSorting(), "Id", "Title");

            var model = new Sorting();
            return View(model);

            //return View();
        }

        public List<Sorting> GetSorting()
        {
            var ddl = new List<Sorting>();
            ddl.Add(new Sorting() { Id = 1, Title = "Company Name" });
            ddl.Add(new Sorting() { Id = 2, Title = "Conctact Name" });
            ddl.Add(new Sorting() { Id = 3, Title = "Years In Business and Company Name" });

            return ddl;
        }

        public List<Comma> SortData(int? sortingId)
        {
            List<Comma> sortData = new List<Comma>();
            SharedViewModel = GetData();

            foreach (var comma in SharedViewModel.FirstOrDefault().CommaViewModel)
            {
                sortData.Add(new Comma
                {
                    CompanyName = comma.CompanyName,
                    YearsInBusiness = comma.YearsInBusiness,
                    ConctactName = comma.ConctactName,
                    ContactPhoneNo = comma.ContactPhoneNo,
                    ContactEmail = comma.ContactEmail
                });

            }
            foreach (var hash in SharedViewModel.FirstOrDefault().HashViewModel)
            {
                sortData.Add(new Comma
                {
                    CompanyName = hash.CompanyName,
                    YearsInBusiness = null,
                    ConctactName = hash.ConctactName,
                    ContactPhoneNo = hash.ContactPhoneNo,
                    ContactEmail = null
                });
            }
            foreach (var hyphen in SharedViewModel.FirstOrDefault().HyphenViewModel)
            {
                sortData.Add(new Comma
                {
                    CompanyName = hyphen.CompanyName,
                    YearsInBusiness = null,
                    ConctactName = hyphen.ConctactFirstName + " " + hyphen.ConctactLastName,
                    ContactPhoneNo = hyphen.ContactPhoneNo,
                    ContactEmail = hyphen.ContactEmail
                });
            }

            switch (sortingId)
            {
                case 1:
                    sortData = sortData.OrderBy(x => x.CompanyName).ToList();
                    break;
                case 2:
                    sortData = sortData.OrderBy(x => x.ConctactName).ToList();
                    break;
                case 3:
                    sortData = sortData.OrderBy(x => x.YearsInBusiness).ThenBy(x => x.CompanyName).ToList();
                    break;
                default:

                    break;
            }
            return sortData;
        }

        [HttpPost]
        public ActionResult ReadData(Sorting sorting)
        {
            ViewBag.Data = SortData(sorting.Id);
            ViewBag.SortingSelectList = new SelectList(GetSorting(), "Id", "Title");
            var model = new Sorting();
            return View(model);
        }

        [HttpPost]
        public FileResult Export(Sorting sortingId)
        {
            //SharedViewModel = GetData();
            List<Comma> sortedList = SortData(sortingId.Id);

            //List<object> exportData = new List<object>();

            //foreach (var comma in SharedViewModel.FirstOrDefault().CommaViewModel)
            //{
            //    exportData.Add(new string[5] { comma.CompanyName, comma.YearsInBusiness, comma.ConctactName, comma.ContactPhoneNo, comma.ContactEmail });

            //}
            //foreach (var hash in SharedViewModel.FirstOrDefault().HashViewModel)
            //{
            //    exportData.Add(new string[5] { hash.CompanyName, string.Empty, hash.ConctactName, hash.ContactPhoneNo, string.Empty });

            //}
            //foreach (var hyphen in SharedViewModel.FirstOrDefault().HyphenViewModel)
            //{
            //    exportData.Add(new string[5] { hyphen.CompanyName, string.Empty, hyphen.ConctactFirstName + " " + hyphen.ConctactLastName, hyphen.ContactPhoneNo, hyphen.ContactEmail });

            //}

            //Insert the Column Names.
            
            //exportData.Insert(0, new string[5] { "Company Name", "Years in Business", " Contact Name", "Contact Phone Number", "Contact Email" });

            StringBuilder sb = new StringBuilder();

            string[] head = new string[] { "Company Name", "Years in Business", " Contact Name", "Contact Phone Number", "Contact Email" };

            foreach(string s in head)
            {
                sb.Append(s + ",");
            }
           
            sb.Append("\r\n");

            for (int i = 0; i < sortedList.Count; i++)
            {
                string[] export = new string[5];
                int k = 0;
                export[k++] = sortedList[i].CompanyName ?? String.Empty;
                export[k++] = sortedList[i].YearsInBusiness ?? String.Empty;
                export[k++] = sortedList[i].ConctactName ?? String.Empty;
                export[k++] = sortedList[i].ContactPhoneNo ?? String.Empty;
                export[k++] = sortedList[i].ContactEmail ?? String.Empty;

                for (int j = 0; j < export.Length; j++)
                {
                    export[j] = export[j].Replace(",", "");
                    //Append data with separator.
                    sb.Append(export[j] + ',');
                }

                //Append new line character.
                sb.Append("\r\n");

            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Export.csv");
        }
    }
}

//{"companyName", "numebr", "plm"} -> ["companyName", "number", "plm"]