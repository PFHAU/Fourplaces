using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fourplaces.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int? ImageId { get; set; }

        public Image Image { get; set; }
    }
}
