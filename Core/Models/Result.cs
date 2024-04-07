using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Common.Enums;

namespace Core.Models
{
    public class Result
    {
        public ResponseStatus Status { get; set; }
        public string? Message { get; set; }
        public string? ValidationErrors { get; set; }

        public Result(ResponseStatus status, string errorMessage)
        {
            Status = status;
            Message = errorMessage;
        }

        public Result(string errorMessage, string validationErrors)
        {
            Status = ResponseStatus.fail;
            Message = errorMessage;
            ValidationErrors = validationErrors;
        }

        public Result(string errorMessage)
        {
            Status = ResponseStatus.error;
            Message = errorMessage;
        }

        public Result()
        {
            Status = ResponseStatus.success;
        }
    }

    public class Result<T> : Result where T : class
    {
        public T? Data { get; set; }

        public Result() : base()
        {
        }

        public Result(T data) : base()
        {
            Data = data;
        }

        public Result(T data, ResponseStatus status)
        {
            Data = data;
            Status = status;
        }

        public Result(T data, string messages) : base()
        {
            Data = data;
            Message = messages;
        }

        public Result(T data, string messages, ResponseStatus status) : base()
        {
            Data = data;
            Message = messages;
            Status = status;
        }

        public Result(string messages, ResponseStatus status) : base()
        {
            Message = messages;
            Status = status;
        }
    }
}
