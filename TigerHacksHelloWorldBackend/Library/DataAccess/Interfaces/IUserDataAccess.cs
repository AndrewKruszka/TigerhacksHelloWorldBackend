using System;
using Library.Models;

namespace Library.DataAccess.Interfaces
{
	public interface IUserDataAccess
	{
        IEnumerable<FriendEntry> GetFriends(Int64 userId);
        Result AddFriend(Int64 userId, Int64 friendUserId, Int64 interactionId);
        User GetUser(string username);
        Result UpsertUser(string username, string passhash);
        IEnumerable<User> GetUsersInHash(string gridhash);
        Result UpdateUserLocation(Int64 userId, string geohash);
    }
}

