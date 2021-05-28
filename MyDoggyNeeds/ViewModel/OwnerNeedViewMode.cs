using MyDoggyNeeds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyDoggyNeeds.ViewModel
{
    public class OwnerViewModel
    {
        public Owner Owner { get; set; }
        public IEnumerable<SelectListItem> AllOwnerNeeds { get; set; }
        private List<int> _selectedOwnerNeeds;
        public OwnerViewModel()
        {
            AllOwnerNeeds = new List<SelectListItem>();
        }
        public List<int> SelectedOwnerNeeds
        {
            get
            {
                if (_selectedOwnerNeeds == null)
                {
                    _selectedOwnerNeeds = Owner.Needs.Select(m => m.Id).ToList();
                }
                return _selectedOwnerNeeds;
            }
            set
            {
                _selectedOwnerNeeds = value;
            }
        }

    }
}