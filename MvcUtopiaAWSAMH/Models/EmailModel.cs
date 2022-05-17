using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcUtopiaAWSAMH.Models
{
    public class EmailModel
    {
        public string Email { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
    }
}
