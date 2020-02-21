namespace SharedTrip.Controllers
{
    using SharedTrip.Services;
    using SIS.HTTP;
    using SIS.MvcFramework;
    using SharedTrip.ViewModels.Trips;
    using static SharedTrip.Data.DataValidation;
    using System;
    using System.Globalization;

    public class TripsController : Controller
    {
        private ITripsService tripsService;

        public TripsController(ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            DisplayAllTripViewModel tripViewModel = new DisplayAllTripViewModel
            {
                Trips = this.tripsService.GetAll()
            };

            return this.View(tripViewModel);
        }

        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(CreateTripModel model)
        {
            int seats = 0;
            bool isSeatsParsed = int.TryParse(model.Seats, out seats);

            if (!isSeatsParsed)
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrWhiteSpace(model.StartPoint))
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrWhiteSpace(model.EndPoint))
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrWhiteSpace(model.DepartureTime))
            {
                return this.Redirect("/Trips/Add");
            }

            if (seats < SeatsMinSize || seats > SeatsMaxSize)
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Length > DescriptionMaxSize)
            {
                return this.Redirect("/Trips/Add");
            }

            DateTime dateResult;
            bool isDateParsed = DateTime.TryParseExact(model.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);

            if (isDateParsed)
            {
                this.tripsService.Create(model.StartPoint, model.EndPoint, dateResult, model.ImagePath, seats, model.Description);

                return this.Redirect("/Trips/All");
            }
            else
            {
                return this.Redirect("/Trips/Add");
            }
        }

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            string currentUserId = this.User;

            DetailsTripViewModel detailsViewModel = this.tripsService.GetTrip(tripId, currentUserId);
            
            return this.View(detailsViewModel);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            string userId = this.User;

            this.tripsService.JoinTrip(tripId, userId);

            return this.Redirect("/Trips/All");
        }
    }
}
