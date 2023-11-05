using System;
namespace Library.Models
{
	public class User
	{
		public Int64 UserId { get; set; }
		public string Name { get; set; }
		public string PassHash { get; set; }
	}
}

