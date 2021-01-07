using H.Framework.Core.Mapping;
using H.Framework.Data.ORM;
using H.Framework.Data.ORM.Attributes;
using H.Framework.Data.ORM.Foundations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Framework.WPF.UITest
{
    public class BaseDTO : IFoundationViewModel
    {
        public string ID { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }

    public class BaseDBModel : IFoundationModel
    {
        public string ID { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }

    public class Order : BaseDBModel
    {
        public string OrderID { get; set; }
        public int Status { get; set; }

        [Foreign("Customer", "CustomerID")]
        public Customer Customer { get; set; }

        [Foreign("User", "UserID")]
        public User User { get; set; }

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
    }

    public class Customer : BaseDBModel
    {
        public string Phone { get; set; }
        public string Nickname { get; set; }
        public int Sno { get; set; }
        public string Avatar { get; set; }
        public int Source { get; set; }
        public string AppID { get; set; }
        public int Ver { get; set; }
        public int RelationType { get; set; }
        public string Channel { get; set; }
        public string ServiceRelation { get; set; }
        public string UserID { get; set; }
        public string EmployeeID { get; set; }
        public string Periods { get; set; }
        public string ClassBatch { get; set; }
        public string OpenID { get; set; }
        public string UnionID { get; set; }
        public string DeviceID { get; set; }
        public int Level { get; set; }
        public string CustomerNum { get; set; }

        [DetailList()]
        public IEnumerable<Order> Orders { get; set; }

        [Foreign("User", "UserID")]
        public User User { get; set; }
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

    public class CustomerDTO : BaseDTO, ICustomMap<Customer>
    {
        public string Phone { get; set; }
        public string Nickname { get; set; }
        public int Sno { get; set; }
        public string Avatar { get; set; }
        public int Source { get; set; }
        public string AppID { get; set; }
        public int Ver { get; set; }
        public int RelationType { get; set; }
        public string Channel { get; set; }
        public string ServiceRelation { get; set; }
        public string UserID { get; set; }
        public string EmployeeID { get; set; }
        public string Periods { get; set; }
        public string ClassBatch { get; set; }
        public string OpenID { get; set; }
        public string UnionID { get; set; }
        public string DeviceID { get; set; }
        public int Level { get; set; }
        public string CustomerNum { get; set; }

        [MappingIgnore]
        public IEnumerable<OrderDTO> Orders { get; set; }

        [MappingIgnore]
        public UserDTO User
        {
            get;
            set;
        }

        public void MapFrom(Customer source)
        {
            Orders = source?.Orders?.MapAllTo(x => new OrderDTO());
            User = source?.User?.MapTo(x => new UserDTO());
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

        public void MapFrom(Order source)
        {
            Customer = source?.Customer?.MapTo(x => new CustomerDTO());
            User = source?.User?.MapTo(x => new UserDTO());
        }
    }

    public class UserDTO : BaseDTO, ICustomMap<User>
    {
        public string Username { get; set; }

        public string Phone { get; set; }

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

    public class UserDAL : BaseDAL<User, Department, Role>
    {
    }

    public class UserBLL : BaseBLL<UserDTO, User, Department, Role, UserDAL>
    {
        public async void Get()
        {
            var req = new UserDTO { Username = "11135", ID = "8" };
            var query = new WhereQueryable<UserDTO, Department, Role>((x, y, z) => x.ID == req.ID && y.Name == "asd");
            var a = await GetAsync(query, "Department,Roles");
        }
    }

    public class OrderDAL : BaseDAL<Order, Customer, User>
    {
    }

    public class OrderBLL : BaseBLL<OrderDTO, Order, Customer, User, OrderDAL>
    {
        //DepartmentWithParent
        public async void GetOrdersAsync()
        {
            var a = await GetListAsync((x, y, z) => 1 == 1, 1, 0, "Customer,User");
        }

        public void AddOrder()
        {
            Add(new OrderDTO { CouponID = "asdasd" });
        }
    }

    public class CustomerDAL : BaseDAL<Customer, User, Order>
    {
    }

    public class CustomerBLL : BaseBLL<CustomerDTO, Customer, User, Order, CustomerDAL>
    {
        public async void GetAsync()
        {
            var query = new WhereQueryable<CustomerDTO, User, Order>((x, y, z) => true);
            var a = await GetAsync((x, y, z) => x.UserID.Contains("70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,9,10,11,23,86") && y.DepartmentID.Contains("6"), "User");
        }

        public async void AddAsync()
        {
            await AddAsync(new List<CustomerDTO> { new CustomerDTO { Nickname = "asd'eee" }, new CustomerDTO { Nickname = "哈哈'eee" } });
        }

        public async void UpdateAsync()
        {
            await UpdateAsync(new List<CustomerDTO> { new CustomerDTO { Nickname = "asd'尔尔", ID = "832", UpdatedTime = DateTime.Now.AddDays(1) }, new CustomerDTO { Nickname = "哈哈'吧吧v", ID = "833" } });
        }
    }
}