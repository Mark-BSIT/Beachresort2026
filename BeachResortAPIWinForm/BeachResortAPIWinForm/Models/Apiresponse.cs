using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachResortAPIWinForm.Models
{
    public class Apiresponse<T>
    {
        public List<T> data { get; set; }
    }
}
