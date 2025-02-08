

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;


namespace ReCaptchaApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors]
public class ReCaptchaAPIController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ReCaptchaAPIController(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    [HttpGet("Captcha")]
    
    public async Task<bool> GetReCaptchaResponse(string userResponse)
    {
        try
        {
             var reCaptchaSecretKey = _configuration["reCaptcha:SecretKey"];

            if (string.IsNullOrEmpty(reCaptchaSecretKey) && string.IsNullOrEmpty(userResponse))
            {
                throw new ArgumentNullException();
            }

            var content = new FormUrlEncodedContent( new Dictionary<string, string>
            {
                {"secret", reCaptchaSecretKey },
                {"response", userResponse }
            });

            var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("not successful call");
            }

            var result = await response.Content.ReadFromJsonAsync<ReCaptchaResponse>(); 
            return result.Success;
       
        } catch (Exception ex)
        {
            throw;
        }
       
    }

    public class ReCaptchaResponse
    {
        public bool Success { get; set; }
        public string[] ErrorCodes { get; set; }
    }
}