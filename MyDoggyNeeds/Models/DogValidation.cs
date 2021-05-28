using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyDoggyNeeds.Models
{
    [MetadataType(typeof(DogMetaData))]
    public partial class Dog
    {

        public class DogMetaData
        {
            [Required]
            [DisplayName("Dog's Name")]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public System.DateTime DOB { get; set; }


            [Required]
            public string Size { get; set; }

            [Required]
            public string Breed { get; set; }


            public string Image { get; set; }
            
            
            [Required]
            public string Description { get; set; }


        }
    }
}