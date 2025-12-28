using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ApiBooks.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ApiBooks.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutenticacionController : ControllerBase
{
    private readonly string secretKey;

    // Constructor: obtiene la clave secreta desde la configuración (appsettings.json)
    public AutenticacionController(IConfiguration config)
    {
        //secretKey = config.GetSection("settings").GetSection("secretKey").ToString();
        secretKey = config.GetSection("settings").GetSection("secretKey").ToString();        
    }

    [HttpPost]
    [Route("Validate")]
    public IActionResult Validate([FromBody] User request)
    {
        // Validación básica de credenciales (usuario y contraseña fijos para pruebas)
        // Basic credential validation (fixed username and password for testing)        
        if(request.usuario == "admin" && request.password == "123")
        {
            // Convertimos la clave secreta en bytes para firmar el token
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);

            // Creacion de la identidad del usuario y agregar un claim (correo)
            // Creating the user identity and adding a claim (email)
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.usuario));

            // Configuracion del descriptor del token:
            // - Asigancion de los claims
            // - Definicion de tiempo de expiración (5 minutos)
            // - Indicamos el algoritmo de firma y la clave secreta
            var tokenDescriptor = new SecurityTokenDescriptor
            {
              Subject = claims, 
              Expires = DateTime.UtcNow.AddMinutes(5),
              SigningCredentials = new SigningCredentials(
                  new SymmetricSecurityKey(keyBytes),
                  SecurityAlgorithms.HmacSha256Signature)
            };

            // Generar el token usando JwtSecurityTokenHandler
            // Generate the token using JwtSecurityTokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            // Convertimos el token a string para enviarlo al cliente
            string createdToken = tokenHandler.WriteToken(tokenConfig);

            // Devolvemos el token en la respuesta con código 200 OK
            // We return the token in the response with a 200 OK status
            return StatusCode(StatusCodes.Status200OK, new {token = createdToken});
        }
        else
        {
            // Si las credenciales no son válidas, devolvemos 400 BadRequest
            // If the credentials are not valid, we return 400 Bad Request
            return StatusCode(StatusCodes.Status400BadRequest, new {token = "Datos de autenticacion incorrectos"});
        }
    }
}