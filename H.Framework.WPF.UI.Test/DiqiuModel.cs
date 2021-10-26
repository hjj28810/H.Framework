using H.Framework.Core.Mapping;
using H.Framework.Data.ORM;
using H.Framework.Data.ORM.Attributes;
using H.Framework.Data.ORM.Foundations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Framework.WPF.UI.Test
{
    public class BaseDTO : IFoundationViewModel
    {
        public string ID { get; set; }

        public DateTime CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
    }

    public class BaseDBModel : IFoundationModel
    {
        public string ID { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }

    public class CustomerDynamicField : BaseDBModel
    {
        public string DynamicFieldID { get; set; }

        [ForeignKeyID("customer")]
        public string CustomerID { get; set; }

        public string FieldValue { get; set; }
        public string FieldKey { get; set; }

        [MappingIgnore]
        [Foreign("Customer", "CustomerID")]
        public Customer Customer { get; set; }
    }

    public class Order : BaseDBModel
    {
        public string OrderNum { get; set; }
        public int Status { get; set; }

        [Foreign("Customer", "CustomerID")]
        public Customer Customer { get; set; }

        [ForeignKeyID("customer")]
        public int CustomerID { get; set; }

        public string Phone { get; set; }
        public string OpenID { get; set; }
        public string AppID { get; set; }
        public int GoodsType { get; set; }
        public string GoodsID { get; set; }
        public string GoodsName { get; set; }
        public bool IsVipGoods { get; set; }
        public int Amount { get; set; }
        public int DiscountAmount { get; set; }
        public int PaidAmount { get; set; }
        public string GoodsDescription { get; set; }
        public int TradeType { get; set; }
        public string CouponID { get; set; }
        public string TransactionID { get; set; }
        public int ExpiredAt { get; set; }
        public bool IsVerify { get; set; }
        public bool IsSend { get; set; }
        public string MWebUrl { get; set; }

        public string UserID { get; set; }
        public string IntroducerUserID { get; set; }

        [MappingIgnore]
        [Foreign("User", "UserID")]
        public User User { get; set; }

        [MappingIgnore]
        [Foreign("User", "IntroducerUserID")]
        public User IntroducerUser { get; set; }
    }

    public class Customer : BaseDBModel
    {
        public string Phone { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string Sno { get; set; }
        public int Source { get; set; }
        public string PreUserID { get; set; }
        public string PostUserID { get; set; }
        public string Periods { get; set; }
        public string ClassBatch { get; set; }
        public int Level { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public bool IsValid { get; set; }
        public bool IsSubmit { get; set; }
        public long SubmitedAt { get; set; }
        public int LastTrackType { get; set; }
        public string UnionID { get; set; }
        public long LastTrackedAt { get; set; }
        public long LevelExpiryAt { get; set; }
        public DateTime LastPaidTime { get; set; }
        public long PreLastCallAt { get; set; }
        public long PostLastCallAt { get; set; }
        public bool IsAddWeChatWork { get; set; }
        public bool IsInternal { get; set; }
        public bool IsFollowWeChatWork { get; set; }
        public bool IsPositions { get; set; }
        public string IntroducerCustomerID { get; set; }
        public int Points { get; set; }

        [LastIDCondition]
        public string CustomerNum { get; set; }

        [MappingIgnore]
        [DetailList()]
        public List<CallRecord> CallRecords { get; set; }

        [MappingIgnore]
        [Foreign("User", "PreUserID")]
        public User PreUser { get; set; }

        [MappingIgnore]
        [Foreign("User", "PostUserID")]
        public User PostUser { get; set; }

        [MappingIgnore]
        [Foreign("Customer", "IntroducerCustomerID")]
        public Customer IntroducerCustomer { get; set; }

        [MappingIgnore]
        [DetailList()]
        [OnlyQuery]
        public List<Contact> Contacts { get; set; }

        [MappingIgnore]
        [DetailList()]
        [OnlyQuery]
        public List<CustomerDynamicField> CustomerDynamicFields { get; set; }

        [MappingIgnore]
        [DetailList()]
        [OnlyQuery]
        public List<Order> Orders { get; set; }

        [DynamicSQLField("sum(case when FieldValue not in ('未知','') and DynamicFieldID = 36 then true else false end) IsHighRisk")]
        public bool IsHighRisk { get; set; }
    }

    public class Department : BaseDBModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ParentID { get; set; }
    }

    public class Resource : BaseDBModel
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public string Value { get; set; }
        public string Code { get; set; }

        public string ParentID { get; set; }
    }

    public class Role : BaseDBModel
    {
        public string Name { get; set; }
        public string Value { get; set; }

        [DetailList("RoleResource", "RoleID", "ResourceID")]
        public List<Resource> Resources { get; set; }
    }

    public class User : BaseDBModel
    {
        public string Username { get; set; }

        public string Phone { get; set; }

        public string Nickname { get; set; }

        public string EmployeeID { get; set; }

        public string DepartmentID { get; set; }

        public bool IsLeader { get; set; }

        [Foreign("Department", "DepartmentID")]
        public Department Department { get; set; }

        [DetailList("UserRole", "UserID", "RoleID")]
        public List<Role> Roles { get; set; }
    }

    public class DepartmentDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ParentID { get; set; }

        public DepartmentDTO ParentDepartment { get; set; }
    }

    public class RoleDTO : BaseDTO, ICustomMap<Role>
    {
        public string Name { get; set; }
        public string Value { get; set; }

        [MappingIgnore]
        public IEnumerable<ResourceDTO> Resources { get; set; }

        public void MapFrom(Role source)
        {
            Resources = source?.Resources?.MapAllTo(x => new ResourceDTO());
        }
    }

    public class ResourceDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public string Value { get; set; }

        public string Code { get; set; }

        public string ParentID { get; set; }

        [MappingIgnore]
        public ResourceDTO Parent { get; set; }

        [MappingIgnore]
        public IEnumerable<ResourceDTO> Children { get; set; }
    }

    public class CustomerDynamicFieldDTO : BaseDBModel
    {
        public string DynamicFieldID { get; set; }

        public string CustomerID { get; set; }

        public string FieldValue { get; set; }
        public string FieldKey { get; set; }
    }

    public class CustomerDTO : BaseDTO, ICustomMap<Customer>
    {
        public string Phone { get; set; }

        public int Sno { get; set; }
        public int Source { get; set; }

        public string Periods { get; set; }

        public int Level { get; set; }
        public string CustomerNum { get; set; }

        public string Nickname { get; set; }

        public string PreUserID { get; set; }

        public string PostUserID { get; set; }

        [MappingIgnore]
        public IEnumerable<OrderDTO> Orders { get; set; }

        [MappingIgnore]
        public IEnumerable<ContactDTO> Contacts { get; set; }

        [MappingIgnore]
        public IEnumerable<CustomerDynamicFieldDTO> CustomerDynamicFields { get; set; }

        [MappingIgnore]
        public UserDTO PreUser { get; set; }

        [MappingIgnore]
        public UserDTO PostUser { get; set; }

        public bool IsHighRisk { get; set; }

        public void MapFrom(Customer source)
        {
            Orders = source?.Orders?.MapAllTo(x => new OrderDTO());
            Contacts = source?.Contacts?.MapAllTo(x => new ContactDTO());
            CustomerDynamicFields = source?.CustomerDynamicFields?.MapAllTo(x => new CustomerDynamicFieldDTO());
            PreUser = source?.PreUser?.MapTo(x => new UserDTO());
            PostUser = source?.PostUser?.MapTo(x => new UserDTO());
        }
    }

    public class OrderDTO : BaseDTO, ICustomMap<Order>
    {
        public string OrderID { get; set; }
        public int Status { get; set; }
        public string CustomerID { get; set; }
        public string Phone { get; set; }
        public string OpenID { get; set; }
        public string AppID { get; set; }
        public int GoodsType { get; set; }
        public string GoodsID { get; set; }
        public string GoodsName { get; set; }
        public bool IsVipGoods { get; set; }
        public int Amount { get; set; }
        public int DiscountAmount { get; set; }
        public int PaidAmount { get; set; }
        public string GoodsDescription { get; set; }
        public int TradeType { get; set; }
        public string CouponID { get; set; }
        public string TransactionID { get; set; }
        public int ExpiredAt { get; set; }
        public bool IsVerify { get; set; }
        public bool IsSend { get; set; }
        public string UserID { get; set; }
        public string IntroducerUserID { get; set; }
        public string MWebUrl { get; set; }

        [MappingIgnore]
        public CustomerDTO Customer
        {
            get;
            set;
        }

        [MappingIgnore]
        public UserDTO User
        {
            get;
            set;
        }

        [MappingIgnore]
        public UserDTO IntroducerUser
        {
            get;
            set;
        }

        public void MapFrom(Order source)
        {
            Customer = source?.Customer?.MapTo(x => new CustomerDTO());
            User = source?.User?.MapTo(x => new UserDTO());
            IntroducerUser = source?.IntroducerUser?.MapTo(x => new UserDTO());
        }
    }

    public class UserDTO : BaseDTO, ICustomMap<User>
    {
        public string Username { get; set; }

        public string Phone { get; set; }

        public string Nickname { get; set; }

        public string EmployeeID { get; set; }

        public string DepartmentID { get; set; }

        public string Token { get; set; }

        public bool IsLeader { get; set; }

        [MappingIgnore]
        public DepartmentDTO Department
        {
            get;
            set;
        }

        [MappingIgnore]
        public IEnumerable<RoleDTO> Roles
        {
            get;
            set;
        }

        [MappingIgnore]
        public List<ResourceDTO> Resources { get; set; } = new List<ResourceDTO>();

        [MappingIgnore]
        public IEnumerable<ResourceDTO> Menus { get; set; }

        public void MapFrom(User source)
        {
            Department = source?.Department?.MapTo(x => new DepartmentDTO());
            Roles = source?.Roles?.MapAllTo(x => new RoleDTO());
        }
    }

    public class RoleDAL : BaseDAL<Role, Resource>
    {
    }

    public class RoleBLL : BaseBLL<RoleDTO, Role, Resource, RoleDAL>
    {
        public async void Get()
        {
            var a = await GetListAsync((x) => x.ID.Contains("1"), (y) => true, "Resources");
        }
    }

    public class UserDAL : BaseDAL<User, Department, Role>
    {
    }

    public class UserBLL : BaseBLL<UserDTO, User, Department, Role, UserDAL>
    {
        public async void Get()
        {
            var req = new UserDTO { Username = "11135", ID = "8" };
            var query = new WhereQueryable<UserDTO>((x) => x.Nickname == "后端业务");
            var a = await GetListAsync(query, 20, 0, "Roles");
        }
    }

    public class OrderDAL : BaseDAL<Order, Customer, User, User>
    {
    }

    public class OrderBLL : BaseBLL<OrderDTO, Order, Customer, User, User, OrderDAL>
    {
        //DepartmentWithParent
        public async void GetOrdersAsync()
        {
            var date = DateTime.Now;
            var a = await GetListAsync((x, y, z, zz) => x.CreatedTime > date, 1, 0, "Customer,User,IntroducerUser");
        }

        public void AddOrder()
        {
            Add(new OrderDTO { CouponID = "asdasd" });
        }
    }

    public class CustomerDAL : BaseDAL<Customer>
    {
    }

    public class CustomerBLL : BaseBLL<CustomerDTO, Customer, CustomerDAL>
    {
        public void GetAsync()
        {
            var date = DateTime.Now.AddMonths(-1);
            var query = new WhereQueryable<CustomerDTO, User, User, CustomerDynamicField, Order>((x, y, yy, d, o) => true);
            var query0 = new WhereQueryable<CustomerDTO, User, User, CustomerDynamicField, Contact>((x, y, yy, d, w) => true);
            var query1 = new WhereJoinQueryable<CustomerDynamicField>((z) => true);
            var query2 = new WhereQueryable<CustomerDTO, User, User, Contact>((x, y, yy, d) => true);
            //var a = await GetListAsync(x => true, (y, yy, z, zzz) => true, 20, 0, "PreUser,PostUser,Contacts,CustomerDynamicFields", new List<OrderByEntity> { new OrderByEntity { IsAsc = false, KeyWord = "LastPaidTime", IsMainTable = true } });
            //var b = await GetListAsync(query, query1, 20, 0, "PreUser,PostUser", "Contacts,CustomerDynamicFields");
            //query.WhereAnd((x, y, yy) => true);
            //query1.WhereAnd((z, zz) => true);
            //query0.WhereAnd((x, y, yy, z, zzz) => true);
            var include = "PreUser,PostUser";

            //include += ",Contacts,'',Contacts";
            //var nickname = "wang(aaa)";
            //query0 = query0.WhereAnd((x, y, yy, d, w) => d.DynamicFieldID == "3");
            //var bb = GetListAsync(query, query1, 20, 0, "PreUser,PostUser,CustomerDynamicFields,Orders", "CustomerDynamicFields").Result;
            query2 = query2.WhereAnd((x, y, yy, d) => d.Content.Contains("'13321952950'"));
            var bb = GetAsync(query2, query1, include + ",Contacts", "CustomerDynamicFields").Result;
            //var bb = GetListAsync(query, 20, 0, "PreUser,PostUser,CustomerDynamicFields,Orders").Result;
        }

        public async void Get()
        {
            var mainQuery = new WhereQueryable<CustomerDTO, User, User>((x, y, yy) => true);
            var joinQuery = new WhereJoinQueryable<CustomerDynamicField, Contact>((w, d) => true);
            mainQuery = mainQuery.WhereAnd((x, y, yy) => x.ID == "1");

            var data = await GetAsync(mainQuery, joinQuery, "PreUser,PostUser", "Contacts");
        }

        public async void AddAsync()
        {
            await AddAsync(new List<CustomerDTO> { new CustomerDTO { Phone = "13321952950" }, new CustomerDTO { Phone = "13521952950" } });
        }

        public async void UpdateAsync()
        {
            await UpdateAsync(new List<CustomerDTO> { new CustomerDTO { ID = "832", Phone = "13321952950" }, new CustomerDTO { ID = "833", Phone = "13321952951" } });
        }
    }

    public class Customer1DAL : BaseDAL<Customer>
    {
    }

    public class Customer1BLL : BaseBLL<CustomerDTO, Customer, Customer1DAL>
    {
        public void GetAsync()
        {
            var date = DateTime.Now.AddMonths(-1);
            var query = new WhereQueryable<CustomerDTO, User, User>((x, y, yy) => true);
            var query0 = new WhereQueryable<CustomerDTO, User, User, CustomerDynamicField>((x, y, yy, d) => true);
            var query1 = new WhereJoinQueryable<Contact, CustomerDynamicField>((z, zzz) => z.Content.Contains("'13321952950'"));
            //var a = await GetListAsync(x => true, (y, yy, z, zzz) => true, 20, 0, "PreUser,PostUser,Contacts,CustomerDynamicFields", new List<OrderByEntity> { new OrderByEntity { IsAsc = false, KeyWord = "LastPaidTime", IsMainTable = true } });
            //var b = await GetListAsync(query, query1, 20, 0, "PreUser,PostUser", "Contacts,CustomerDynamicFields");
            //query.WhereAnd((x, y, yy) => true);
            //query1.WhereAnd((z, zz) => true);
            //query0.WhereAnd((x, y, yy, z, zzz) => true);
            query0 = query0.WhereAnd((x, y, yy, d) => d.DynamicFieldID == "7" && d.FieldKey == "310300");
            var bb = GetListAsync(query0, 20, 0, "PreUser,PostUser,CustomerDynamicFields").Result;
            var cc = CountAsync(query0, "PreUser,PostUser,CustomerDynamicFields").Result;
        }
    }

    public class CallRecordDTO : BaseDTO, ICustomMap<CallRecord>
    {
        public int Duration { get; set; }
        public string Phone { get; set; }
        public int Type { get; set; }
        public string CustomerID { get; set; }
        public string UserID { get; set; }
        public string Remark { get; set; }
        public string RecordUrl { get; set; }
        public string UserDisplay { get; set; }
        public int DepartmentType { get; set; }
        public string CallID { get; set; }

        [MappingIgnore]
        public CustomerDTO Customer { get; set; }

        [MappingIgnore]
        public UserDTO User { get; set; }

        public void MapFrom(CallRecord source)
        {
            Customer = source?.Customer?.MapTo(x => new CustomerDTO());
            User = source?.User?.MapTo(x => new UserDTO());
        }
    }

    public class Contact : BaseDBModel
    {
        public string Content { get; set; }

        [ForeignKeyID("customer")]
        public string CustomerID { get; set; }

        public string Remark { get; set; }

        [MappingIgnore]
        [Foreign("Customer", "CustomerID")]
        public Customer Customer { get; set; }
    }

    public class CallRecord : BaseDBModel
    {
        public int Duration { get; set; }
        public string Phone { get; set; }
        public int Type { get; set; }
        public string CallID { get; set; }

        [ForeignKeyID("customer")]
        public string CustomerID { get; set; }

        [MappingIgnore]
        [Foreign("Customer", "CustomerID")]
        public Customer Customer { get; set; }

        [MappingIgnore]
        [Foreign("User", "UserID")]
        public User User { get; set; }

        public string UserID { get; set; }
        public string Remark { get; set; }
        public string RecordUrl { get; set; }
        public int DepartmentType { get; set; }
    }

    public class CallRecordDAL : BaseDAL<CallRecord, Customer, User>
    {
    }

    public class CallRecordUpdateReq
    {
        public string ID { get; set; }
        public int? Duration { get; set; }
        public string Phone { get; set; }
        public int? Type { get; set; }
        public string CustomerID { get; set; }
        public string UserID { get; set; }
        public string Remark { get; set; }
        public string RecordUrl { get; set; }
        public string UserDisplay { get; set; }
        public int DepartmentType { get; set; }
        public string CallID { get; set; }
    }

    public class CallRecordBLL : BaseBLL<CallRecordDTO, CallRecord, Customer, User, CallRecordDAL>
    {
        public void GetAsync()
        {
            var req = new CallRecordUpdateReq();
            var a = GetListAsync((x, y, z) => x.CustomerID == req.CustomerID && x.DepartmentType == 2, 100, 0, "Customer,User", new List<OrderByEntity> { new OrderByEntity { IsAsc = false, KeyWord = "CreatedTime", IsMainTable = true } }).Result;
        }

        public async void AddCallRecordAsync(CallRecordDTO req)
        {
            await AddAsync(req);
        }
    }

    public class DynamicFieldDTO : BaseDTO
    {
        public string Name { get; set; }

        public int Type { get; set; }

        public int ViewType { get; set; }

        public string Extension { get; set; }
    }

    public class DynamicFieldBLL : BaseBLL<DynamicFieldDTO, DynamicField, DynamicFieldDAL>
    {
        /// <summary>
        /// 获取动态字段
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DynamicFieldDTO>> GetDynamicFieldsAsync(int type)
        {
            return await GetListAsync(x => x.Type == type);
        }
    }

    public class DynamicField : BaseDBModel
    {
        public string Name { get; set; }

        public int Type { get; set; }

        public int ViewType { get; set; }

        public string Extension { get; set; }
    }

    public class DynamicFieldDAL : BaseDAL<DynamicField>
    {
    }

    public class ContactDTO : BaseDTO
    {
        public string Content { get; set; }
        public string CustomerID { get; set; }
        public string Remark { get; set; }
    }

    public class UserAttachmentDAL : BaseDAL<UserAttachment>
    {
    }

    public class UserAttachment : BaseDBModel
    {
        public string AttachmentUrl { get; set; }
        public int Type { get; set; }
        public string UserID { get; set; }
    }

    public class UserAttachmentDTO : BaseDTO
    {
        public string AttachmentUrl { get; set; }
        public int Type { get; set; }
        public string UserID { get; set; }

        public string UserDisplay { get; set; }
    }

    public class UserAttachmentBLL : BaseBLL<UserAttachmentDTO, UserAttachment, UserAttachmentDAL>
    {
        public async void GetUserAttachmentsAsync()
        {
            var query = new WhereQueryable<UserAttachmentDTO>((x) => true);
            //query = query.WhereAnd(x => x.UserID.Contains(token.SubordinateUserIDs));
            var a = await GetListAsync(query, 20, 0, "", new List<OrderByEntity> { new OrderByEntity { IsAsc = false, KeyWord = "CreatedTime" } });
        }
    }

    public class Notification : BaseDBModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public int Level { get; set; }
        public string Creator { get; set; }
        public string ExtJson { get; set; }
        public int Type { get; set; }

        [DetailList()]
        public List<NotificationMark> NotificationMarks { get; set; }
    }

    public class NotificationMark : BaseDBModel
    {
        public bool IsRead { get; set; }

        [ForeignKeyID("notification")]
        public string NotificationID { get; set; }

        public string UserID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedTime { get; set; }
    }

    public class NotificationBLL : BaseBLL<NotificationDTO, Notification, NotificationMark, NotificationDAL>
    {
        public async void GetUnreadCountAsync(int type)
        {
            var query = new WhereQueryable<NotificationDTO, NotificationMark>((x, y) => y.UserID == "9" && x.IsRead == false & y.IsDeleted == false);
            if (type != 0)
                query = query.WhereAnd((x, y) => x.Type == type);
            var a = await GetAsync(x => true, "NotificationMarks");
        }
    }

    public class NotificationDTO : BaseDTO, ICustomMap<Notification>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public int Level { get; set; }
        public int Type { get; set; }
        public string Creator { get; set; }
        public string ExtJson { get; set; }

        public bool IsRead { get; set; }

        public string MarkID { get; set; }

        public void MapFrom(Notification source)
        {
            var item = source?.NotificationMarks?.FirstOrDefault();
            if (item != null)
            {
                IsRead = item.IsRead;
                MarkID = item.ID;
            }
        }
    }

    public class NotificationDAL : BaseDAL<Notification, NotificationMark>
    {
    }
}