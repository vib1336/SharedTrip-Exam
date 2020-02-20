namespace SharedTrip.ViewModels.Trips
{
    using System.Collections.Generic;

    public class DisplayAllTripViewModel
    {
        public IEnumerable<AllTripViewModel> Trips { get; set; }
    }
}
