using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using MVC.Models;
using System;
using System.Collections.Generic;

public class HomeController : Controller
{
    private readonly string _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING");


    public IActionResult Intro()
    {
        return View();
    }
}

