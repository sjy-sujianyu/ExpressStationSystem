using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace ExpressStationSystem
{
    public class UploadFilter : IOperationFilter
    {

        /// <summary>
        ///     文件上传
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (!string.IsNullOrWhiteSpace(operation.summary) && operation.summary.Contains("upload"))
            {
                operation.consumes.Add("application/form-data");
                if (operation.parameters == null)
                    operation.parameters = new List<Parameter>();
                operation.parameters.Add(new Parameter
                {
                    name = "file",
                    @in = "formData",
                    required = true,
                    type = "file"
                });
            }
        }
    }
    public class PackageClass
    {
        public int id;

        public decimal weight;

        public decimal price;

        public int sendId;

        public int receiverId;

        public string Remarks;

        public string account;
    }
    public class AddressBookClass
    {
        public int aId;

        public string phone;

        public string name;

        public string province;

        public string city;

        public string street;

        public string account;
    }
    public class LoginClass
    {
        public string account;
        public string password;
    }
    public class Address
    {
        private string province;
        private string city;
        private string street;

        public string Province { get => province; set => province = value; }
        public string City { get => city; set => city = value; }
        public string Street { get => street; set => street = value; }

        public Address()
        {
            province = "A省";
            city = "A市";
            street = "A街道";
        }
        public Address(string province, string city, string street)
        {
            Province = province;
            City = city;
            Street = street;
        }
        
    }
    public class PickUpClass
    {
        public int id;
        public string mId;
    }
    public class PickUpClassPlus
    {
        public int id;
        public string mId;
        public DateTime time;
    }

    public class IdClass
    {
        public int id;
    }
    public class DeliveryClass
    {
        private int id;
        private string mid;

        public int Id { get => id; set => id = value; }
        public string Mid { get => mid; set => mid = value; }
    }
    public class MemberClass
    {
        public string mId;

        public string name;

        public string job;

        public bool isDelete;
    }
}