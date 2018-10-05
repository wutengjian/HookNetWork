using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
public class DapperBase
{
    public static string ConnString = "Server=.;Database=Store;uid=sa;pwd=123456;";
    static void Main(string[] args)
    {
        using (var conn = new SqlConnection(ConnString))
        {
            conn.Open();
            //==================================简单查询=======================================
            //var bookList= conn.Query<Book>("select * from Book where id>=@id", new { id = 1});

            //foreach(var book in bookList)
            //{
            //    Console.WriteLine(book.Name);

            //}
            //===================================添加数据============================================
            //Book book = new Book();
            //book.Name = "C#";
            //string query = "INSERT INTO Book(Name)VALUES(@name)";
            ////对对象进行操作
            //conn.Execute(query, book);
            ////直接赋值操作
            //conn.Execute(query, new { name = "C#" });
            //===================================更新数据============================================
            //Book book = new Book();
            //book.Name = "ASP编程入门";
            //book.Id = 2;
            //string query = "UPDATE Book SET  Name=@name WHERE id =@id";
            //conn.Execute(query, book);
            //===================================删除数据============================================
            //string query = "DELETE FROM Book WHERE id = @id";
            //conn.Execute(query, book);
            //conn.Execute(query, new { id = id });
            //==================================查询=======================================
            //string query = "SELECT * FROM Book";
            ////无参数查询，返回列表，带参数查询和之前的参数赋值法相同。
            //conn.Query<Book>(query).ToList();

            ////返回单条信息
            //string query = "SELECT * FROM Book WHERE id = @id";
            //book = conn.Query<Book>(query, new { id = id }).SingleOrDefault();
            //==================================1对多复杂询=======================================
            //查询图书时，同时查找对应的书评，并存在List中。实现1--n的查询操作
            //string query = "SELECT * FROM Book b LEFT JOIN BookReview br ON br.BookId = b.Id WHERE b.id = @id";
            ////Query<TFirst, TSecond, TReturn>
            //Book lookup = null;
            ////Query<TFirst, TSecond, TReturn>
            //var b = conn.Query<Book, BookReview, Book>(query,
            //    (book, bookReview) =>
            //    {
            //        //扫描第一条记录，判断非空和非重复
            //        if (lookup == null || lookup.Id != book.Id)
            //            lookup = book;
            //        //书对应的书评非空，加入当前书的书评List中，最后把重复的书去掉。
            //        if (bookReview != null)
            //            lookup.Reviews.Add(bookReview);
            //        return lookup; //输入参数Book, BookReview,返回 Book
            //    }, new { id = 1 }).ToList();
            //Console.WriteLine("");
            //conn.Close();
            //==================================多对1复杂查询=======================================
            //string query = "SELECT BR.Id,BR.BookId,BR.Content,B.Id,B.Name FROM BookReview BR INNER JOIN Book B ON BR.BookId=B.Id WHERE BR.Id= @id";
            ////第一个对象BookReview，第二个对象Book，返回对象 与查询语句表顺序对应
            //BookReview br = conn.Query<BookReview, Book, BookReview>(query,
            //   (bookReview, book) => //与上面的对象对应
            //   {
            //       bookReview.AssoicationWithBook = book;
            //       return bookReview;
            //   }, new { id = 1 },null,true,"Id").SingleOrDefault(); //第2个对象也就是Book对象（与查询语句对应），从Id拆分，所以查询语句必须写上B.Id，否则报错
            conn.Close();
        }
    }
}