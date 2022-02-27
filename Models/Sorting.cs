using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MilestoneProject.Models
{
    public class Sorting
    {

        public int Id { get; set; }
        public string Title { get; set; }

        private SelectList _selectSorting { get; set; }

        public SelectList SortingSelectList
        {
            get
            {
                if (_selectSorting != null)
                    return _selectSorting;
                return new SelectList(GetSorting(), "Id", "Title");
            }
            set { _selectSorting = value; }
        }
        private List<Sorting> GetSorting()
        {
            var ddl = new List<Sorting>();
            ddl.Add(new Sorting() { Id = 1, Title = "Company Name" });
            ddl.Add(new Sorting() { Id = 2, Title = "Conctact Name" });
            ddl.Add(new Sorting() { Id = 3, Title = "Years In Business and Company Name" });

            return ddl;
        }
    }
}