using System;
using Library.DataAccess.Interfaces;
using Library.Models;
using Library.Workers.Interfaces;

namespace Library.Workers
{
	public class UserWorker : IUserWorker
	{
		private readonly IUserDataAccess dataAccess;

		public UserWorker(IUserDataAccess userDataAccess)
		{
			this.dataAccess = userDataAccess;
		}

		public Result CreateUser(string username, string password)
		{
			var user = dataAccess.GetUser(username);
			if (user != null)
				return new Result() { IsSuccessful = false, ErrorMessage = "User already exists" };

			return dataAccess.UpsertUser(username, password);
		}

		public Result UpdateUserPassword(string username, string password)
		{
            var user = dataAccess.GetUser(username);
            if (user == null)
                return new Result() { IsSuccessful = false, ErrorMessage = "User does not exist" };

            return dataAccess.UpsertUser(username, password);
        }

		public User Login(string username, string password)
		{
            var user = dataAccess.GetUser(username);
			if (user == null)
				return null;

			if (user.PassHash.Equals(password))
				return user;

			return null;
        }
    }
}

