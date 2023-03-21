using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace RLDA_VehicleData_Watch.Utility.Filter
{
    public class CustomerExceptionFilterAtrribute:ExceptionFilterAttribute
    {
        private readonly ILogger<CustomerExceptionFilterAtrribute> _logger;

        public CustomerExceptionFilterAtrribute(ILogger<CustomerExceptionFilterAtrribute> logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                Exception ex = context.Exception;
                //错误所在的控制器方法名称
                var actionName = context.ActionDescriptor.DisplayName;
                var controllerName = context.HttpContext.Request.RouteValues["controller"];
                _logger.LogError("控制器为："+controllerName+ "，方法为："+ actionName+ "，出现了错误：" + ex.Message);
                //context.HttpContext.Response.Redirect("/Home/Error");
                context.ExceptionHandled = true;
            }
           
        }
    }
}
