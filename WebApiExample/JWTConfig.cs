namespace WebApiExample;

public class JWTConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string JwtKey { get; set; }
}