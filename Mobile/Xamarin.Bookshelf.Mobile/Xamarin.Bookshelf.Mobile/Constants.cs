namespace Xamarin.Bookshelf.Mobile
{
    public class Constants
    {
        public const string AUTHENTICATION_BASE_URL = "https://tracinha-functions.azurewebsites.net/.auth/login/";
        public const string AUTHENTICATION_URL_GOOGLE = AUTHENTICATION_BASE_URL + "google?post_login_redirect_uri=tracinha://&session_mode=token&access_type=offline";
        public const string AUTHENTICATION_URL_APPLE = AUTHENTICATION_BASE_URL + "Apple";
            public const string DEEP_LINK_SCHEMA = "tracinha://";

        public const string AUTHENTICATION_TOKEN = "authentication_token";
        public const string ACCESS_TOKEN = "access_token";
        public const string ID_TOKEN = "id_token";
        public const string EXPIRES_IN = "expires_in";
        public const string PROVIDER_NAME = "provider_name";
        public const string USER_ID = "user_id";
    }
}
