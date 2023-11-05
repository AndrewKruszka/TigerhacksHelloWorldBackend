using System;
using Library.Models;

namespace Library.Workers.Interfaces
{
	public interface IInteractionWorker
	{
        Interaction GetInteraction(double latitude, double longitude, Int64 userId);
    }
}

