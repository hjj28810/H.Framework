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

        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
    }

    public class BaseDBModel : IFoundationModel
    {
        public string ID { get; set; }

        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
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
    { }
}