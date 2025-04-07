using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CourseProject.Models;

namespace CourseProject.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
