using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static H.Framework.WPF.UITest.EFCoreTest;

namespace H.Framework.WPF.UITest
{
    public class EFCoreTest
    {
        public class User
        {
            public int ID { set; get; }

            public string Username { set; get; }

            public List<UserAttachment> Attachments { set; get; }
        }

        public class UserAttachment
        {
            public int ID { set; get; }
            public string AttachmentUrl { set; get; }
            public int UserID { set; get; }
        }
    }

    public class DBDiqiuContext : DbContext
    {
        public DbSet<EFCoreTest.User> user { set; get; }

        public DbSet<UserAttachment> userattachment { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySQL(@"Server=192.168.99.108;Database=diqiu_crm;User ID=root;Password=Dasong@;Port=3306;TreatTinyAsBoolean=false;SslMode=none;Allow User Variables=True;charset=utf8");
    }

    public class UserDB
    {
        private readonly DBDiqiuContext _db;

        public UserDB()
        {
            _db = new DBDiqiuContext();
        }

        //public List<EFCoreTest.User> GetUsers()
        //{
        //    return _db.user.Join(_db.userattachment, a => a.ID, b => b.UserID, (a, b) => new EFCoreTest.User { ID = a.ID, Username = a.Username }).ToList();
        //}
    }
}