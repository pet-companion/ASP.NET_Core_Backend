using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.ViewModel
{
    public class APIResponse
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }

        public APIResponse(string status, int statusCode, string message, string exceptionMessage)
        {
            Status = status;
            StatusCode = statusCode;
            Message = message;
            ExceptionMessage = exceptionMessage;
        }

        public APIResponse(string status, int statusCode, string message)
        {
            Status = status;
            StatusCode = statusCode;
            Message = message;
        }
        public APIResponse()
        {
        }
    }
    public class APIResponse<T> : APIResponse
    {
        public T Data { get; set; }
        public int TotalRecords { get; set; }

        public APIResponse(string status, int statusCode, string message, int totalRecords, T data)
        {
            Status = status;
            StatusCode = statusCode;
            Message = message;
            TotalRecords = totalRecords;
            Data = data;
        }

        public APIResponse(string status, int statusCode, string message, string exceptionMessage, T data)
        {
            Status = status;
            StatusCode = statusCode;
            Message = message;
            ExceptionMessage = exceptionMessage;
            Data = data;
        }

        public APIResponse(string status, int statusCode, string message, T data)
        {
            Status = status;
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }
}
