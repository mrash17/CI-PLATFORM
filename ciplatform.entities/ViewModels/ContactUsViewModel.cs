using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ciplatform.entities.ViewModels
{
    public class ContactUsViewModel
    {
        [Required(ErrorMessage = "Subject is Required")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Message is Required")]
        public string Message { get; set; } = null!;
    }
}
