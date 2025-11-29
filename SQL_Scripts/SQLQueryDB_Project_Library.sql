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