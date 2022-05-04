using System.ComponentModel.DataAnnotations;

namespace MVC_Sort_Test.Models
{
    public class SortEntry
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }

        public string? OrigonalCSV { get; set; }
        public string? SortedCSV { get; set; }
        public int SortOrder { get; set; }

        [DataType(DataType.Time)]
        public double SortTime { get; set; }


        
    }
}
