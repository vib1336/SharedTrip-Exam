using SharedTrip.Models;
using SharedTrip.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SharedTrip.Services
{
    public class TripsService : ITripsService
    {
        private ApplicationDbContext db;

        public TripsService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void Create(string startPoint, string endPoint, DateTime departureTime, 
            string imagePath, int seats, string description)
        {
            Trip trip = new Trip
            {
                StartPoint = startPoint,
                EndPoint = endPoint,
                DepartureTime = departureTime,
                ImagePath = imagePath,
                Seats = seats,
                Description = description
            };

            this.db.Trips.Add(trip);
            this.db.SaveChanges();
        }

        public IEnumerable<AllTripViewModel> GetAll()
            => this.db
            .Trips
            .Select(t => new AllTripViewModel
            {
                Id = t.Id,
                StartPoint = t.StartPoint,
                EndPoint = t.EndPoint,
                DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm:ss"),
                Seats = t.Seats
            })
            .ToList();

        public DetailsTripViewModel GetTrip(string tripId, string currentUserId)
        {
            bool isSameUser = false;

            string userIdToCheck = this.db
                .UserTrips
                .Where(ut => ut.TripId == tripId)
                .Select(ut => ut.UserId)
                .FirstOrDefault();

            isSameUser = userIdToCheck == currentUserId ? true : false;

            DetailsTripViewModel detailsTrip = this.db
              .Trips
              .Where(t => t.Id == tripId)
              .Select(t => new DetailsTripViewModel
              {
                  Id = t.Id,
                  StartPoint = t.StartPoint,
                  EndPoint = t.EndPoint,
                  DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm:ss"),
                  Seats = t.Seats,
                  ImagePath = t.ImagePath,
                  Description = t.Description,
                  IsSameUser = isSameUser
              })
              .FirstOrDefault();

            return detailsTrip; 
        }

        public void JoinTrip(string tripId, string userId)
        {
            Trip trip = this.db
                .Trips
                .FirstOrDefault(t => t.Id == tripId);

            trip.Seats--;

            UserTrip userTrip = new UserTrip
            {
                TripId = tripId,
                UserId = userId
            };

            this.db.UserTrips.Add(userTrip);
            this.db.SaveChanges();
        }

    }
}
