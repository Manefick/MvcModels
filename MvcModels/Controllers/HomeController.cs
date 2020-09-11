using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcModels.Models;

namespace MvcModels.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;
        public HomeController(IRepository repo)
        {
            repository = repo;
        }
        //проверяет выражение перед ?? если оно равно ноне возвращает значение после ?? 
        //public ViewResult Index(int id) => View(repository[id] ?? repository.People.FirstOrDefault());
        public IActionResult Index([FromQuery]int? id)//получения даных из строки запроса
        {
            Person person;
            if(id.HasValue&&(person= repository[id.Value]) != null)
            {
                return View(person);
            }
            else
            {
                return NotFound();
            }
        }
        public ViewResult Create() => View(new Person());
        [HttpPost]
        public ViewResult Create(Person model) => View("Index", model);
        //используем конструкцию/префикс для привязки даных(свойств) к другой модели, в списке через запятую добавляються свойства.
        //которые нужно включить в процес привязки модели
        public ViewResult DisplaySummary([Bind(Prefix =nameof(Person.HomeAddress))] AddressSummary summary) => View(summary);
        public ViewResult Names(IList<string> names) => View(names ?? new List<string>());
        public ViewResult Address(IList<AddressSummary> adresses) => View(adresses ?? new List<AddressSummary>());
        //Использование заголовка в качестве источника даных привязки
        public string Header([FromHeader(Name ="Accept-Language")]string accept) => $" Header: {accept}";
        public ViewResult SummaryHeader(HeaderModel header) => View(header);
        //использования тела запроса как источника даных для привязки модели
        public ViewResult Body() => View();
        public Person Body([FromBody] Person person) => person;
    }
}
