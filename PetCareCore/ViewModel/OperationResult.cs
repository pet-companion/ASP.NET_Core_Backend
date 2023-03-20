using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.ViewModel
{
    public class OperationResult
    {
        public bool Status { get; set; }
    }
    public class OperationResult<T>: OperationResult
    {
        public T Data { get; set; }
    }
}
