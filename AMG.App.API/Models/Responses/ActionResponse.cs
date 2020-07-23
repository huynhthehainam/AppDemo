using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace AMG.App.API.Models.Responses
{
    public class ErrMessage
    {
        public string Name { get; set; }
        public List<string> Errs { get; set; } = new List<string>();
    }
    public class ActionResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int StatusCode { get; set; } = 200;
        public object Data { get; set; }
        public OrderedDictionary Errors { get; set; }
        private List<ErrMessage> errMessages = new List<ErrMessage>();
        private void EnsureErrorsNotNull()
        {
            if (Errors == null)
            {
                Errors = new OrderedDictionary();
                Type = "Detect error by AMIT";
                Title = "One or more validation errors occurred.";
            }
        }
        private ErrMessage GetErrMessage(string name)
        {
            ErrMessage errMessage = errMessages.FirstOrDefault(dd => dd.Name == name);
            if (errMessage == null)
            {
                errMessage = new ErrMessage() { Name = name };
                errMessages.Add(errMessage);
            }
            return errMessage;
        }
        public void AddMessageErr(string name, string message)
        {
            StatusCode = 400;
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add(message);
        }
        public void AddRequirementErr(string name)
        {
            StatusCode = 400;
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add($"{name} required");
        }
        public void AddNotAllowedErr()
        {
            StatusCode = 400;
            ErrMessage errMessage = GetErrMessage("Permission");
            errMessage.Errs.Add("Denied");
        }
        public void AddNotCreatedErr(string name)
        {
            StatusCode = 400;
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add($"Not found");
        }
        public void AddExpiredErr(string name)
        {
            StatusCode = 400;
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add($"Exceeds expiring time");
        }
        public void AddInvalidErr(string name)
        {
            StatusCode = 400;
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add($"Invalid");
        }
        public void AddExistedErr(string name)
        {
            StatusCode = 400;
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add($"Already exists");
        }
        public IActionResult ToIActionResult()
        {
            if (this.StatusCode == 200)
            {
                return new OkObjectResult(this);
            }
            else
            {
                return new BadRequestObjectResult(this);
            }
        }
    }
}