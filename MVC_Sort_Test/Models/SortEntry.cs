using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MVC_Sort_Test.Models
{
    public class SortEntry
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }
        
        [Required]
        [RegularExpression(@"^-?\d+,-?\d+(,-?\d+)*$", ErrorMessage = "Must be two numbers or more and match format '123,45,6,78,90'")]
        public string? OriginalCSV { get; set; }
        
         
        public string? SortedCSV { get; private set; }    
        
        [Required]
        public int SortOrder { get; set; }
      
        [DataType(DataType.Time)]
        public double SortTime { get; private set; }

        public void Sort()
        {
            this.DateAdded = DateTime.Now;
            var watch = new Stopwatch();
            //Parse ints and skip int if not a int.
            //regex catchs all formatting issuses except for number being <||> max/min for a int.
            //LINQ query filers out the unusable numbers
            int[] ints = OriginalCSV.Split(',')
                    .Where(m => int.TryParse(m, out _))
                    .Select(m => int.Parse(m))
                    .ToArray();
          
            //Do the sort depending on the direction selected
            if (this.SortOrder == 1)
            {
                watch.Restart();
                // Sort with LINQ
                /* var sorted = ints.OrderBy(num => num).ToArray(); */
                // Sort with array.sort()
                Array.Sort(ints);
                
                watch.Stop();

                this.SortedCSV = string.Join<int>(",", ints);
                this.SortTime = watch.Elapsed.TotalMilliseconds;
            }
            else
            {
                watch.Restart();
                // Sort with LINQ
                /* var sorted = ints.OrderByDescending(num => num).ToArray(); */
                // Sort with array.sort()
                Array.Sort(ints);
                Array.Reverse(ints);
                watch.Stop();
                this.SortedCSV = string.Join<int>(",", ints);
                this.SortTime = watch.Elapsed.TotalMilliseconds;
            }          
        }        

        public void GenerateRandomOriginalCSV()
        {
            //Build originalCSV
            var randIntCount = Extensions.ThreadSafeRandom.Next(2, 1000);
            int[] OGList = new int[randIntCount];
            for (int j = 0; j < randIntCount; j++)
            {
                OGList[j] = Extensions.ThreadSafeRandom.Next(-10000, 10000);
            }
            this.OriginalCSV = string.Join<int>(",", OGList);
            this.SortOrder = (Extensions.ThreadSafeRandom.Next(0, 2) == 1) ? 1 : -1;           
        }

    }
}
