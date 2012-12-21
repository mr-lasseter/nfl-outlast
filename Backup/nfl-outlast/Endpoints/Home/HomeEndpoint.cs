namespace nfl_outlast.Endpoints.Home
{
    public class HomeEndpoint
    {
        public HomeViewModel Home(HomeRequest request)
        {
            return new HomeViewModel();
        }
    }

    public class HomeViewModel
    {
    }

    public class HomeRequest
    {
    }
}