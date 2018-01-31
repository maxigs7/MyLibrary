using Microsoft.Owin.Security.OAuth;

namespace Framework.Security
{
    public interface IOAuthAuthorizationServerOptions
    {
        OAuthAuthorizationServerOptions GetOptions();
    }
}
