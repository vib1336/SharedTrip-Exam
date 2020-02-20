namespace SharedTrip.Services
{
    using SharedTrip.ViewModels.Trips;
    using System;
    using System.Collections.Generic;

    public interface ITripsService
    {
        void Create(string startPoint, string endPoint, DateTime departureTime, string imagePath, int seats, string description);

        IEnumerable<AllTripViewModel> GetAll();

        DetailsTripViewModel GetTrip(string tripId, string currentUserId);

        void JoinTrip(string tripId, string userId);
    }
}
