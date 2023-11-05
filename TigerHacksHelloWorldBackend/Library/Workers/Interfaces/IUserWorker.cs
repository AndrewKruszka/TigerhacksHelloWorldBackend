using System;
using Library.Models;

namespace Library.Workers.Interfaces
{
	public interface IUserWorker
	{
        Result CreateUser(string username, string password);
        Result UpdateUserPassword(string username, string password);
        User Login(string username, string password);
    }
}

