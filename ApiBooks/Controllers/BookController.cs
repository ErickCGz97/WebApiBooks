namespace ApiBooks.Controllers;

using System.Data;
using System.Data.SqlClient;
using ApiBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

[EnableCors("cors")]
[ApiController]
[Authorize]
[Route("api/[controller]")]
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

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
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
            return StatusCode(StatusCodes.Status200OK, new { message = "Proper functioning", response = list });
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message, response = list });
        }
    }

    [HttpGet]
    [Route("GetById/{idBook:int}")]
    public IActionResult GetId(int idBook)
    {
        Books book = null;
        try
        {
            using (var connec = new SqlConnection(connecSQL))
            {
                connec.Open();
                using (var cmd = new SqlCommand("sp_get_book_by_id", connec))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDBook", idBook);

                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            book = new Books()
                            {
                                IdBook = Convert.ToInt32(rd["IDBook"]),
                                ISBN = rd["ISBN"].ToString(),
                                Title = rd["Title"].ToString(),
                                Author = rd["Author"].ToString(),
                                Pages = Convert.ToInt32(rd["Pages"])
                            };
                        }
                    }
                }
            }
            if (book == null)
                return NotFound(new { message = "Book not found" });

            return Ok(new { message = "Proper functioning", response = book });
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message });
        }
    }

    [HttpPost]
    [Route("Post")]
    public IActionResult Post([FromBody] Books bookObject)
    {
        try
        {
            using (var connec = new SqlConnection(connecSQL))
            {
                connec.Open();
                var cmd = new SqlCommand("sp_post_books", connec);
                cmd.Parameters.AddWithValue("ISBN", bookObject.ISBN);
                cmd.Parameters.AddWithValue("Title", bookObject.Title);
                cmd.Parameters.AddWithValue("Author", bookObject.Author);
                cmd.Parameters.AddWithValue("Pages", bookObject.Pages);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            return StatusCode(StatusCodes.Status200OK, new { message = "Proper functioning" });
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message });
        }
    }

    [HttpPut]
    [Route("Update")]
    public IActionResult Update([FromBody] Books bookObject)
    {
        try
        {
            using (var connec = new SqlConnection(connecSQL))
            {
                connec.Open();
                var cmd = new SqlCommand("sp_update_books", connec);
                cmd.Parameters.AddWithValue("IDBook", bookObject.IdBook == 0 ? DBNull.Value : bookObject.IdBook);
                cmd.Parameters.AddWithValue("ISBN", bookObject.ISBN is null ? DBNull.Value : bookObject.ISBN);
                cmd.Parameters.AddWithValue("Title", bookObject.Title is null ? DBNull.Value : bookObject.Title);
                cmd.Parameters.AddWithValue("Author", bookObject.Author is null ? DBNull.Value : bookObject.Author);
                cmd.Parameters.AddWithValue("Pages", bookObject.Pages == 0 ? DBNull.Value : bookObject.Pages);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            return StatusCode(StatusCodes.Status200OK, new { message = "Update process functioning" });
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message });
        }
    }

    [HttpDelete]
    [Route("Delete/{idBook:int}")]
    public IActionResult Delete(int idBook)
    {
        try
        {
            using (var connec = new SqlConnection(connecSQL))
            {
                connec.Open();
                var cmd = new SqlCommand("sp_delete_books", connec);
                cmd.Parameters.AddWithValue("IDBook", idBook);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            return StatusCode(StatusCodes.Status200OK, new { message = "Delete process functioning" });
        }
        catch (Exception error)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error.Message });
        }
    }
}