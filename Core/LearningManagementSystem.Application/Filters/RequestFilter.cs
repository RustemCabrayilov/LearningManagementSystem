using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystem.Persistence.Filters;

public class RequestFilter
{
 
        //Filtering
        public string? FilterField { get; set; }
        public string? FilterValue { get; set; }


        //Sorting parametrs
        public string? SortField { get; set; }
        public bool IsDescending { get; set; }


        //Paging parametrs
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        [Range(1, int.MaxValue)]
        public int Count { get; set; } = 10;
}