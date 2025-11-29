--1) Create and use Database
create database DB_Project_Library
use DB_Project_Library

--2) Create tables
create table BOOK
(
IDBook int identity primary key,
ISBN varchar(20) not null,
Title varchar(100) not null,
Author varchar(100) not null,
Pages int not null
);

Select *from BOOK

--3) Stored procedures

--Mostrar libros/Get books
create proc sp_get_books
as
begin
	select
	IDBook, ISBN, Title, Author, Pages from BOOK
end

-- Filtrar libro unico // Show just the requested book
CREATE PROCEDURE sp_get_book_by_id
    @IDBook INT
AS
BEGIN
    SELECT * FROM BOOK WHERE IdBook = @IDBook;
END

--Agregar libro/Post books
create proc sp_post_books(
@ISBN varchar(20),
@Title varchar(100),
@Author varchar(100),
@Pages int
)as
begin
	insert into BOOK(ISBN, Title, Author, Pages)
	values (@ISBN, @Title, @Author, @Pages)
end

--Editar books/Update books
create proc sp_update_books(
@IDBook int,
@ISBN varchar(20) null,
@Title varchar(100) null,
@Author varchar(100) null,
@Pages int null
)as
begin
	update BOOK set
	ISBN = isnull(@ISBN, ISBN),
	Title = isnull(@Title, Title),
	Author = isnull(@Author, Author),
	Pages = isnull(@Pages, Pages)
	where IDBook = @IDBook
end

--Eliminar libros/Delete books
create proc sp_delete_books(
@IDBook int
)as
begin
	delete from BOOK where IDBook = @IDBook
end

/*
1	978-0-5468-9549-0	G.R.R Martin	Game of Thrones	574
2	978-6-0588-6172-5	G.R.R Martin	A Clash of Kings	761
3	978-5-6151-6604-4	G.R.R Martin	A Storm of Swords	973
4	978-5-3698-0454-4	G.R.R Martin	A Dance with Dragons	1056
*/

execute sp_get_books