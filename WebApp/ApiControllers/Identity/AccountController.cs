using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using App.DAL.EF;
using App.Domain.Identity;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;

namespace WebApp.ApiControllers.Identity;

[ApiController]
[Route("/api/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    
    public AccountController(
        UserManager<AppUser> userManager, 
        ILogger<AccountController> logger, 
        SignInManager<AppUser> signInManager, 
        IConfiguration configuration, 
        AppDbContext context)
    {
        _userManager = userManager;
        _logger = logger;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<JwtResponse>> Login([FromBody] LoginInfo loginInfo)
    {
        // verify user
        var appUser = await _userManager.FindByEmailAsync(loginInfo.Email);
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginInfo.Email);
            // TODO: random delay
            return NotFound("User/Password Problem");
        }
        
        // verify password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginInfo.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password {} for email {} was wrong", loginInfo.Password, loginInfo.Email);
            // TODO: random delay
            return NotFound("User/Password problem");
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("WebApi login failed, claimsPrincipal null");
            return NotFound("User/Password problem");
        }

        // TODO: think some more! Do we allow multiple sessions? or delete all tokens?
        // TODO: Create access token for each session, then can manage session (log out), and each session has its own refresh token (no clash when refreshing)
        var deletedRows = await _context.RefreshTokens
            .Where(t => t.AppUserId == appUser.Id && t.ExpirationDt < DateTime.UtcNow).ExecuteDeleteAsync();
        
        _logger.LogWarning("Deleted {} refresh tokens", deletedRows);

        var refreshToken = new AppRefreshToken()
        {
            AppUserId = appUser.Id
        };
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>("JWT:key")!,
            _configuration.GetValue<string>("JWT:issuer")!,
            _configuration.GetValue<string>("JWT:audience")!,
            60*60*24);

        var response = new JwtResponse
        {
            Jwt = jwt,
            RefreshToken = refreshToken.RefreshToken
        };
        
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<JwtResponse>> RefreshTokenData(
        [FromBody] TokenRefreshInfo tokenRefreshInfo
        )
    {
        // extract jwt
        JwtSecurityToken? jwtSecTok;
        try
        {
            jwtSecTok = new JwtSecurityTokenHandler().ReadJwtToken(tokenRefreshInfo.Jwt);
            if (jwtSecTok == null)
            {
                return BadRequest("No token");
            }
        }
        catch (Exception)
        {
            return BadRequest("No token");
        }
        
        // validate jwt, ignore expiration date
        var isValidJwt = IdentityHelpers.ValidateJwt(
            tokenRefreshInfo.Jwt,
            _configuration.GetValue<string>("JWT:key")!,
            _configuration.GetValue<string>("JWT:issuer")!,
            _configuration.GetValue<string>("JWT:audience")!
            );

        if (!isValidJwt) return BadRequest("Invalid Token");
        
        // TODO!
        // extract userid or username from jwt
        
        
        // validate refresh token
        // get user
        // generate jwt
        // mark refresh token in db as expired, generate new values
        // return data
        
        return Ok();
    }
}