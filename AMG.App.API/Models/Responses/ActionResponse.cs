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
        public string Type
        {
            get
            {
                if (errMessages.Count > 0)
                    return "Detect error by AMIT";
                return null;
            }
        }
        public string Title
        {
            get
            {
                if (errMessages.Count > 0)
                    return "One or more validation errors occurred.";
                return null;
            }
        }
        public int StatusCode
        {
            get
            {
                if (errMessages.Count > 0)
                    return 400;
                return 200;
            }
        }
        public object Data { get; set; }
        public OrderedDictionary Errors
        {
            get
            {
                if (errMessages.Count == 0)
                    return null;
                OrderedDictionary errors = new OrderedDictionary();

                foreach (var errMessage in errMessages)
                {
                    errors.Add(errMessage.Name, errMessage.Errs);
                }
                return errors;
            }
        }
        private List<ErrMessage> errMessages = new List<ErrMessage>();
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
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add(message);
        }
        public void AddRequirementErr(string name)
        {
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add($"{name} required");
        }
        public void AddNotAllowedErr()
        {

            ErrMessage errMessage = GetErrMessage("Permission");
            errMessage.Errs.Add("Denied");
        }
        public void AddNotCreatedErr(string name)
        {
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add($"Not found");
        }
        public void AddExpiredErr(string name)
        {
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add($"Exceeds expiring time");
        }
        public void AddInvalidErr(string name)
        {
            ErrMessage errMessage = GetErrMessage(name);
            errMessage.Errs.Add($"Invalid");
        }
        public void AddExistedErr(string name)
        {
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