namespace SharedTrip.App.Controllers
{
    using SharedTrip.Services;
    using SharedTrip.ViewModels;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            return this.View();
        }

    }
}