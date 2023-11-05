using System;
namespace Library.Models
{
	public class Interaction
	{
		public Int64 Id { get; set; }
		public DateTime InteractedOn { get; set; }
		public Int64 PrimaryUserId { get; set; }
		public Int64 InteractedWithUserId { get; set; }
		public string InteractedAtHash { get; set; }
	}
}

