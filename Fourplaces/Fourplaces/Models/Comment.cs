using System;
using System.Collections.Generic;
using System.Text;

namespace Fourplaces.Models
{
    public class Comment
    {

		public Comment()
		{

		}

		public DateTime Date { get; set; }


		public User Author { get; set; }


		public string Text { get; set; }
	}
}
