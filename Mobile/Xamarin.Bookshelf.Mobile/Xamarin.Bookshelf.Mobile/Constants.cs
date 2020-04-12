namespace Xamarin.Bookshelf.Mobile
{
    public class Constants
    {
        public const string AUTHENTICATION_URL = "https://xamarin-bookshelf.azurewebsites.net/.auth/login/google?post_login_redirect_uri=tracinha://&session_mode=token";
            public const string DEEP_LINK_SCHEMA = "tracinha://";

        public const string ACCESS_TOKEN = "access_token";
        public const string REFRESH_TOKEN = "refresh_token";
        public const string EXPIRES_IN = "expires_in";
    }
}
