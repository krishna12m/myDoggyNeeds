using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyDoggyNeeds.Models
{
    [MetadataType(typeof(OwnerMetaData))]
    public partial class Owner
    {
       
        public class OwnerMetaData
        {
            [Required]
            [DisplayName("First Name")]

            public string Fname { get; set; }

            [Required]
            [DisplayName("Last Name")]
            public string Lname { get; set; }

            [Required]
            [DisplayName("DOB")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public System.DateTime Dob { get; set; }

            
            [DisplayName("Line 1")]
            public string Line1 { get; set; }


            [Required]
            [DisplayName("City")]
            public string City { get; set; }

            [Required]
            [DisplayName("PostCode")]
            public string Postcode { get; set; }

            [Required]
            public string Email { get; set; }

            [Required]
            public string Phone { get; set; }
          

        }
    }
}