using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Web;
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
    public class MidClass
    {
        public string mid;
    }
    public class TransferClass
    {
        public int id;
        public string mid;
        public int vid;
    }
    public class MemberClass
    {
        public string mId;

        public string name;

        public string job;

        public decimal baseSalary;
    }

    public class onDutyClass
    {
        public string mId;

        public bool onDuty;
    }

    public class accountClass
    {
        public string account;
    }

    public class MidChange
    {
        public string oldMid;
        public string newMid;
    }
    public class VehicleClass
    {
        public string type;

        public string plateNumber;
    }
    public partial class VehicleStatus
    {
        public int vId;

        public bool onDuty;
    }
    public class LeaveClass
    {
        public string mId;

        public string reason;

        public System.DateTime srcTime;

        public System.DateTime endTime;
    }
    public class ConfirmLeaveClass
    {
        public int lId;

        public string view;

        public string person;

        public bool isDone;

        public string mId;
    }
    public class UpdateLeaveClass
    {
        public int lId;

        public string reason;

        public string mId;

        public System.DateTime srcTime;

        public System.DateTime endTime;
    }
    public class lIdClass
    {
        public int lId;

        public string mId;
    }

    public class jobClass
    {
        public string mId;

        public string job;
    }

    public class lIdClass1
    {
        public int lId;
    }
    public class MoneyClass
    {
        public string mId;

        public decimal subsidy;

        public decimal fine;

        public string reason;

        public string person;
    }
    public class MoneyClassPlus
    {
        public int sId;

        public decimal subsidy;

        public decimal fine;

        public string reason;

        public string person;
    }
    public class ErrorClass
    {
        public int id;

        public string introduction;

        public string status;
    }
    public class CommisionClass
    {
        public decimal pickUpValue;
        public decimal transferValue;
        public decimal deliveryValue;
        public string person;
    }

}