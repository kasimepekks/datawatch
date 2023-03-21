using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RLDA_VehicleData_Watch.Controllers
{
    [Authorize]
    public class PopUpController : Controller
    {
        public IActionResult LogPopUp()
        {
            return View();
        }
        public IActionResult VehicleImtSetupTablePopUp()
        {

            return View();
        }

        public IActionResult VehicleCalSetupTablePopUp()
        {

            return View();
        }
        public IActionResult T_VehicleMaster_PopUp()
        {

            return View();
        }
        public IActionResult T_VehicleMonitorPara_PopUp()
        {

            return View();
        }
        public IActionResult T_VehicleImportPara_PopUp()
        {

            return View();
        }

        public IActionResult T_VehicleComputeParaPopUp()
        {

            return View();
        }
    }
}
