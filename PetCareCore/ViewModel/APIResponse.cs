using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.ViewModel
{
    public class APIResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<string> errorlist { get; set; }
        public Exception Exception { get; set; }

        public APIResponse(bool status, string message, List<string> errorlist, Exception exception)
        {
            this.status = status;
            this.message = message;
            this.errorlist = errorlist;
            Exception = exception;
        }
        public APIResponse(bool status, string message, List<string> errorlist)
        {
            this.status = status;
            this.message = message;
            this.errorlist = errorlist;
        }
        public APIResponse(bool status, string message)
        {
            this.status = status;
            this.message = message;
            this.errorlist = errorlist;
        }
        public APIResponse()
        {
        }
    }
    public class APIResponse<T> : APIResponse
    {
        public T Data { get; set; }
        public int TotalRecords { get; set; }

        public APIResponse(bool status, string message, List<string> errorlist, Exception exception, T data, int totalRecords)
        {
            this.status = status;
            this.message = message;
            this.errorlist = errorlist;
            this.Exception = exception;
            Data = data;
            TotalRecords = totalRecords;
        }
        public APIResponse(bool status, string message, List<string> errorlist, T data, int totalRecords)
        {
            this.status = status;
            this.message = message;
            this.errorlist = errorlist;
            Data = data;
            TotalRecords = totalRecords;
        }
        public APIResponse(bool status, string message, T data, int totalRecords)
        {
            this.status = status;
            this.message = message;
            this.errorlist = errorlist;
            Data = data;
            TotalRecords = totalRecords;
        }
        public APIResponse(bool status, string message, T data)
        {
            this.status = status;
            this.message = message;
            this.errorlist = errorlist;
            Data = data;
        }
    }
}
