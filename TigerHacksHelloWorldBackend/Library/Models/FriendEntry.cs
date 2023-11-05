using System;
namespace Library.Models
{
	public class FriendEntry
	{
		public Int64 PrimaryUserId { get; set; }
		public Int64 FriendUserId { get; set; }
		public Int64 InteractionId { get; set; }
	}
}

