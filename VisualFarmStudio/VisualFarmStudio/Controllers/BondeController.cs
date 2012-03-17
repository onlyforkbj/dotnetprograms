﻿using System.Web.Mvc;
using VisualFarmStudio.Common.Exceptions;
using VisualFarmStudio.Lib.Facades;
using VisualFarmStudio.Lib.UserSession;
using VisualFarmStudio.Models.Bonde;

namespace VisualFarmStudio.Controllers
{
    public class BondeController : VisualFarmControllerBase
    {
        private readonly IBondeFacade _bondeFacade;
        private readonly IUserManager _userManager;

        public BondeController(IBondeFacade bondeFacade, IUserManager userManager)
        {
            _bondeFacade = bondeFacade;
            _userManager = userManager;
        }

        public ActionResult LogIn()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            return RedirectToAction("Index", "Bondegard");
        }

        public ActionResult Register()
        {
            return View(new RegisterBondeViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterBondeViewModel model)
        {
            try
            {
                _bondeFacade.Add(model.Bonde);
                _userManager.LogIn(model.Bonde);
                return RedirectToAction("Index", "Bondegard");
            }
            catch (UserException ex)
            {
                AddUserMessageFor(ex);
                return View(model);
            }
        }
    }
}