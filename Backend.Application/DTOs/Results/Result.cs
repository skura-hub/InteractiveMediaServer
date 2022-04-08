using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.DTOs.Results
{
    public class Result
    {
        public string Message { get; private set; }
        public Result(string message = "")
        {
            this.Message = message;
        }
        
    }

    public class Result  <T> : Result
    {
        public T Data { get; private set; }
        public Result(T data, string message = "") : base(message)
        {
            this.Data = data;
        }
    }
}
