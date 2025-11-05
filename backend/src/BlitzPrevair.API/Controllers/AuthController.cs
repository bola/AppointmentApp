using BlitzPrevair.API.DTOs;
using BlitzPrevair.API.DTOs.Auth;
using BlitzPrevair.API.Services;
using BlitzPrevair.Core.Entities;
using BlitzPrevair.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlitzPrevair.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _passwordService;

    public AuthController(
        ApplicationDbContext context,
        ITokenService tokenService,
        IPasswordService passwordService)
    {
        _context = context;
        _tokenService = tokenService;
        _passwordService = passwordService;
    }

    /// <summary>
    /// Register a new customer
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        // Validate model
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if username already exists
        if (await _context.Customers.AnyAsync(c => c.Username.ToLower() == request.Username.ToLower()))
        {
            return BadRequest(new { message = "Username already exists" });
        }

        // Check if email already exists
        if (await _context.Customers.AnyAsync(c => c.Email.ToLower() == request.Email.ToLower()))
        {
            return BadRequest(new { message = "Email already exists" });
        }

        // Hash password
        var passwordHash = _passwordService.HashPassword(request.Password);

        // Create customer
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            EmailVerified = false,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Generate JWT token
        var token = _tokenService.GenerateToken(customer);

        var response = new AuthResponse
        {
            Token = token,
            ExpiresAt = _tokenService.GetTokenExpiration(),
            Customer = new CustomerDto
            {
                Id = customer.Id,
                Username = customer.Username,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                DateOfBirth = customer.DateOfBirth,
                Gender = customer.Gender,
                EmailVerified = customer.EmailVerified
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Login with username/email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        // Validate model
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Find customer by username or email
        var customer = await _context.Customers
            .Where(c => c.Username.ToLower() == request.UsernameOrEmail.ToLower() ||
                       c.Email.ToLower() == request.UsernameOrEmail.ToLower())
            .FirstOrDefaultAsync();

        if (customer == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // Check if account is active
        if (!customer.IsActive || customer.IsDeleted)
        {
            return Unauthorized(new { message = "Account is inactive" });
        }

        // Verify password
        if (!_passwordService.VerifyPassword(request.Password, customer.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // Generate JWT token
        var token = _tokenService.GenerateToken(customer);

        var response = new AuthResponse
        {
            Token = token,
            ExpiresAt = _tokenService.GetTokenExpiration(),
            Customer = new CustomerDto
            {
                Id = customer.Id,
                Username = customer.Username,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                DateOfBirth = customer.DateOfBirth,
                Gender = customer.Gender,
                EmailVerified = customer.EmailVerified
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Get current logged-in customer profile
    /// </summary>
    [HttpGet("me")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<ActionResult<CustomerDto>> GetCurrentCustomer()
    {
        // Get customer ID from JWT claims
        var customerIdClaim = User.FindFirst("customerId")?.Value;
        if (customerIdClaim == null || !Guid.TryParse(customerIdClaim, out var customerId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var customer = await _context.Customers
            .Where(c => c.Id == customerId && !c.IsDeleted)
            .FirstOrDefaultAsync();

        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        var customerDto = new CustomerDto
        {
            Id = customer.Id,
            Username = customer.Username,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Phone = customer.Phone,
            DateOfBirth = customer.DateOfBirth,
            Gender = customer.Gender,
            EmailVerified = customer.EmailVerified
        };

        return Ok(customerDto);
    }
}
