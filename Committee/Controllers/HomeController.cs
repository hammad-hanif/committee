using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Committee.Models;
using Committee.Interfaces;
using System.Text;

namespace Committee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAzureTableStorage<CommitteeMembers> _azureTableStorage;

        public HomeController(ILogger<HomeController> logger, IAzureTableStorage<CommitteeMembers> azureTableStorage)
        {
            _logger = logger;
            _azureTableStorage = azureTableStorage;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Committee(string id)
        {
            var viewModel = new CommitteeViewModel();
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    viewModel.ErrorMessage = "Please enter the code.";
                }
                else if (id.Length != 5)
                {
                    viewModel.ErrorMessage = "Please enter the valid code.";
                }
                else
                {
                    var committeeMembers = await _azureTableStorage.GetList();

                    var member = committeeMembers.FirstOrDefault(x => x.Code == id);
                    if (member == null)
                    {
                        viewModel.ErrorMessage = "Code is invalid. No Record Found.";
                    }
                    else
                    {
                        if (member.Number == 0)
                        {
                            var random = new Random(100000);
                            var nextNumber = random.Next(1, 11);
                            while (committeeMembers.Any(x => x.Number == nextNumber))
                            {
                                nextNumber = random.Next(1, 11);
                            }


                            member.Number = nextNumber;
                            await _azureTableStorage.Update(member);
                        }

                        viewModel.CommitteeMembers = committeeMembers.Where(x => x.Number != 0).ToList();
                        viewModel.Number = member.Number;
                    }
                }
            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;

                //var innerException = ex.InnerException;
                //if (innerException != null)
                //{
                //    var sb = new StringBuilder("<br /><ul>");
                //    while (ex.InnerException != null)
                //    {
                //        sb.Append($"<li>{innerException.Message}</li>");
                //        innerException = innerException.InnerException;
                //    }
                //    sb.Append("</ul>");
                //    viewModel.ErrorMessage = sb.ToString();
                //}
            }


            //check if code exist
            return PartialView("Committee", viewModel);
        }
    }
}
