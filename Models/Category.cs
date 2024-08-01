using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalApp.Models
{
    public class Category
    {
        public string Description { get; set; }

        public string Categories { get; set; }

        public string CategoryName { get; set; }

        public string Amount { get; set; }

        public DateTime Date { get; set; }

        public string TagName { get ; set; }    

        public List<TagNameClass> tagNameList = new List<TagNameClass>();

        public class TagNameClass
        {
            public string TagName { get; set; }
        }

        public List<Image> imagesList = new List<Image>();

        public class Image
        {
           
            public int Id { get; set; }
            public string ImageUrl { get; set; }

            public string ImageName { get; set; }
        }

        public List<DefaultIcon> defaultIconsList = new List<DefaultIcon>(); 

        public class DefaultIcon
        {
            public int Id { get; set; }
            public string ImageUrl { get; set; }

            public string ImageName { get; set; }
        }
    }
}