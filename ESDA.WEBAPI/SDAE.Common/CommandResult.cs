using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SDAE.Common
{
    public class CommandResult
    {
        public bool Successful { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public RoomTokenDto RoomToken { get; set; }
        public AgtCallHis AgtCallHis { get; set; }
        public List<DropdownList> LstDDL { get; set; }
        public static CommandResult Success()
        {
            return new CommandResult { Successful = true };
        }

        public static CommandResult Success(dynamic objectId)
        {
            return new CommandResult { Successful = true, ObjectId = objectId };
        }

        public dynamic ObjectId { get; set; }

        public static CommandResult Error(Exception exception)
        {
            return new CommandResult { Successful = false, Exception = exception };
        }

        private readonly IList<ValidationErrorResult> _messages = new List<ValidationErrorResult>();

        public IEnumerable<ValidationErrorResult> Errors { get; }
        public dynamic Result { get; set; }

        public CommandResult() => Errors = new ReadOnlyCollection<ValidationErrorResult>(_messages);

        public CommandResult(object result) : this() => Result = result;

        public CommandResult AddError(string propName, string message)
        {
            _messages.Add(new ValidationErrorResult() { PropName = propName, ErrorMessage = message });
            return this;
        }
    }
    public class ValidationErrorResult
    {
        public string PropName { get; set; }
        public string ErrorMessage { get; set; }
    }

    #region Blazor

    public class RoomTokenDto
    {
        public string RoomName { get; set; }
        public string RoomSid { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string RetailerLogo { get; set; }
        public string ProductLogo { get; set; }
        public string BrandLogo { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }

    }

    public class DropdownList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AgentId { get; set; }
    }

    public class AgentsCallDto
    {
        public int AgentId { get; set; }
        public string Agent { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Product { get; set; }
        public string Country { get; set; }
        public string Brand { get; set; }
        public string Retailer { get; set; }
        public string Language { get; set; }
        //public int Duration { get; set; }
        public string CalDuration { get; set; }
        public int? Rating { get; set; }
        public string Notes { get; set; }
        public string DateTimeValue { get; set; }
        public int CallStatusTypeId { get; set; }
        public long CustomerCallId { get; set; }
        public string CallStatus { get; set; }
    }

    public class AgtCallHis
    {
        public AgtCallHis()
        {
            AgtCallHisList = new List<AgentsCallDto>();
        }
        public int TotalRecords { get; set; }
        public List<AgentsCallDto> AgtCallHisList { get; set; }
    }

    public class UserDetailsModel
    {
        public int AgentId { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int? CountryId { get; set; }
        public string FirstName { get; set; }
        public string UserId { get; set; }
    }

    #endregion
}
