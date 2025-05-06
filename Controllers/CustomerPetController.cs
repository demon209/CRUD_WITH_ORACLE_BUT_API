using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using MVC.Models;
using System;
using System.Collections.Generic;

public class CustomerPetController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

