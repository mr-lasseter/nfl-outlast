using FubuMVC.Core.Continuations;
using nfl_outlast.Domain;
using nfl_outlast.Endpoints.Home;

namespace nfl_outlast.Endpoints.Users
{
    public class UsersEndpoint
    {        
        public NewUserViewModel New(NewUserRequest request)
        {           
            return new NewUserViewModel();
        }

        public FubuContinuation New(NewUserCommand command)
        {
            //do something to save the user
            return FubuContinuation.RedirectTo<HomeRequest>();
        }
    }

    public class NewUserRequest
    {
        
    }

    public class NewUserViewModel
    {
        public User User { get; set; } 
    }
    
    public class NewUserCommand
    {
        public User User { get; set; }
    }

    
}