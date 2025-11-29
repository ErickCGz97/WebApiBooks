namespace ApiBooks.Controllers;

using System.Data;
using System.Data.SqlClient;
using ApiBooks.Models;

using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly string connecSQL;

    public BookController(IConfiguration config)
    {
        connecSQL = config.GetConnectionString("cadenaSQL");
    }

    [HttpGet]
    [Route("Get")]
    public IActionResult Get()
    {
        List<Books> list = new List<Books>();
        try
        {
            using (var connec = new SqlConnection(connecSQL))
            {
                connec.Open();
                var cmd = new SqlCommand("sp_get_books", connec);
                cmd.CommandType = CommandType.StoredProcedure;

                using(var rd = cmd.ExecuteReader())
                {
                    while(rd.Read())
                    {
                        list.Add(new Books()
                        {
                           IdBook = Convert.ToInt32(rd["IDBook"]),
                           ISBN = rd["ISBN"].ToString(),
                           Title = rd["Title"].ToString(),
                           Author = rd["Author"].ToString(),
                           Pages = Convert.ToInt32(rd["Pages"])
                        });
                    }
                }
            }
            return StatusCode(StatusCodes.Status200OK, new {message = "Proper functioning", response = list});
        }
        catch(Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new {message = error.Message, response = list});            
        }   
    }

    [HttpGet]
    [Route("Get by ID/{idBook:int}")]
    public IActionResult GetId(int idBook)
    {
        List<Books> list = new List<Books>();
        Books book = new Books();
        try
        {
            using (var connec = new SqlConnection(connecSQL))
            {
                connec.Open();
                var cmd = new SqlCommand("sp_get_books", connec);
                cmd.CommandType = CommandType.StoredProcedure;

                using(var rd = cmd.ExecuteReader())
                {
                    while(rd.Read())
                    {
                        list.Add(new Books()
                        {
                           IdBook = Convert.ToInt32(rd["IDBook"]),
                           ISBN = rd["ISBN"].ToString(),
                           Title = rd["Title"].ToString(),
                           Author = rd["Author"].ToString(),
                           Pages = Convert.ToInt32(rd["Pages"])                        
                        });
                    }
                }
            }
            book = list.Where(item => item.IdBook == idBook).FirstOrDefault();

            return StatusCode(StatusCodes.Status200OK, new {message = "Proper functioning", response = book});
        }
        catch (Exception error)
        {            
            return StatusCode(StatusCodes.Status500InternalServerError, new {message = error.Message, response = book});            
        }
    }
}