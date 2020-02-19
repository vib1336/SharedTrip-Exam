using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.ViewModels
{
    public class DisplayAllTripViewModel
    {
        public IEnumerable<AllTripViewModel> Trips { get; set; }
    }
}
