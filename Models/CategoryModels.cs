using System.Collections.Generic;

namespace MedienKultur.Gurps.Models
{
    
    public class Category
    {
        public Category()
        {
            Articles = new Article[0];
        }
        public int Id { get; set; }
        public int[] Parents { get; set; }
        public int[] Children { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Hint { get; set; }
        public IEnumerable<Article> Articles { get; set; } 
    }


    public class CategoryTree
    {
        
    }
}