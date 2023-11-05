using System;
using Library.Models;

namespace Library.DataAccess.Interfaces
{
	public interface ILandmarkDataAccess
	{
        IEnumerable<Landmark> GetLandmarks();
        Result UpsertLandmark(string geohash, string name);
    }
}

