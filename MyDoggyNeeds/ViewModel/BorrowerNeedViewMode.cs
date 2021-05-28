using MyDoggyNeeds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyDoggyNeeds.ViewModel
{
    public class BorrowerViewModel
    {

        public Borrower Borrower { get; set; }
        public IEnumerable<SelectListItem> AllBorrowerNeeds { get; set; }
        private List<int> _selectedBorrowerNeeds;

        //for create method
        public BorrowerViewModel()
        {
            AllBorrowerNeeds = new List<SelectListItem>();
        }

        //for edit method
        public List<int> SelectedBorrowerNeeds
        {
            get
            {
                if (_selectedBorrowerNeeds == null)
                {
                    _selectedBorrowerNeeds = Borrower.Needs.Select(m => m.Id).ToList();
                }
                return _selectedBorrowerNeeds;
            }
            set
            {
                _selectedBorrowerNeeds = value;
            }
        }

    }
}