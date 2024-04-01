using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Result
    {
        public string? Message { get; set; }
        public string? ValidationErrors { get; set; }


        public Result(string errorMessage, string validationErrors)
        {
            Message = errorMessage;
            ValidationErrors = validationErrors;
        }
    }
}
