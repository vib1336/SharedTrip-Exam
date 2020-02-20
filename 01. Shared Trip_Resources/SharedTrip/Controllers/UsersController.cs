namespace SharedTrip.Controllers
{
    using SharedTrip.Services;
    using SharedTrip.ViewModels.Users;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class UsersController : Controller
    {
        private IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterUserInputModel model)
        {
            if (model.Username.Length < 5 || model.Username.Length > 20 
                || this.usersService.UsernameExists(model.Username))
            {
                return this.Redirect("/Users/Register"); 
            }

            if (string.IsNullOrWhiteSpace(model.Email) || this.usersService.EmailExists(model.Email))
            {
                return this.Redirect("/Users/Register"); 
            }

            if (model.Password.Length < 6 || model.Password.Length > 20)
            {
                return this.Redirect("/Users/Register");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            
            this.usersService.Register(model.Username, model.Email, model.Password);

            return this.Redirect("/Users/Login"); 
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginInputModel model)
        {
            var userId = this.usersService.GetUserId(model.Username, model.Password);

            if (userId != null)
            {
                this.SignIn(userId);
                return this.Redirect("/");
            }

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
