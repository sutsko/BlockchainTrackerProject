using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asp.netCoreMVCCrud1.Models;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml;
using System.IO;
using System.Data;
using OfficeOpenXml.Table;
using Microsoft.AspNetCore.Hosting;

namespace Asp.netCoreMVCCrud1.Controllers
{
    public class ProjectController : Controller
    {
        private  ProjectContext _context;
        public IndustryController _ic;
        private SectorController _sc;
        private OrganizationController _oc;
        private UsecaseController _uc;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ProjectController(ProjectContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _oc = new OrganizationController(_context);
            _ic = new IndustryController(_context);
            _uc = new UsecaseController(_context);
            _hostingEnvironment = hostingEnvironment;
            
    }

        // GET: Project
        public async Task<IActionResult> Index()
        {

            //1. Hente hele listen af alle projekter
           List <Project> listOfProjects = await _context.Projects.ToListAsync();

            //2. Hente hele listen af usecases, organisationer og industrier  
            List<Organization> listOfOrganizations = _oc.GetOrgList();
            List<Usecase> listOfUsecases = _uc.GetUsecList();
            List<Industry> listOfIndustries = _ic.GetInduList();

            //3. Lave mappings over dem - altså fra punkt 2 og deres id 
            Dictionary<int, string> organizationMap = new Dictionary<int, string>();
            Dictionary<int, string> usecaseMap = new Dictionary<int, string>();
            Dictionary<int, string> industryMap = new Dictionary<int, string>();

            foreach (Organization o in listOfOrganizations)
            {
                organizationMap.Add(o.OrganizationId, o.OrganizationName);
            }

            foreach (Usecase uc in listOfUsecases)
            {
                usecaseMap.Add(uc.UsecaseId, uc.UsecaseName);
            }

            foreach (Industry i in listOfIndustries)
            {
                industryMap.Add(i.IndustryId, i.IndustryName);
            }

            //4. På baggrund af nøglen, der skal være ID'et, tilføjes for each til projekterne.   
            foreach(Project p in listOfProjects)
            {
                p.Organization.OrganizationName = organizationMap[p.OrganizationId];
                p.Industry.IndustryName = industryMap[p.IndustryId];
                p.Usecase.UsecaseName = usecaseMap[p.UseCaseId];
            }

            return View(listOfProjects);
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Project/AddOrEdit
        //When the asp-action="AddOrEdit" in index.cshtml is called on the client side, this function will be run
        public IActionResult AddOrEdit(int id = 0)
        {
            var p = ProjectWithLists();

            if (id == 0)
            {
                //https://stackoverflow.com/questions/31647259/populate-dropdownlist-in-mvc-5
                p.ArticleDate = DateTime.Today;


                //This will return the view "addOrEdit" from the folder Views--> Project --> AddOrEdit
                return View(p);
            }
            else {
            
                Project project_from_id = _context.Projects.Find(id);
                project_from_id.IndustryList = p.IndustryList;
                project_from_id.OrganizationList = p.OrganizationList;
                project_from_id.UsecaseList = p.UsecaseList;

            //this function will ask the database to find the project with the corresponding id. 
            return View(project_from_id); 
            }
                
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //The bind function will bind the corresponding values from the UI to the corresponding element that we have defined in model. In this case Project. 
        //All of those parameters have a corresponding control inside the AddOrEdit.cshtml. Just not projectID since it is autoincremented
        public async Task<IActionResult> AddOrEdit([Bind("ProjectId,ArticleHeadline,ArticleUrl,ArticleDescription,ArticleDate,Confidentiality,OrganizationId,Country,IndustryId,UseCaseId,Maturity,TechnicalVendor")] Project project, string command, string myOrganization)
        {

        
            try
            {
                switch (command)
                {
                    case "action:reload":
                        /* This was never implemented. Kept as a reminder of a way to do this.  */
                        break;

                    case "action:submit":
                        if (ModelState.IsValid)
                        {
                            if (project.ProjectId == 0) {
                                //Check if the organization is a new one. 
                                if (project.OrganizationId==0)
                                {
                                    //Adding the organization to the database, and then seeting the id of that organization to the 
                                    Organization preliminaryOrg = new Organization();
                                    preliminaryOrg.OrganizationName = myOrganization;
                                    preliminaryOrg.OrganizationType = 1;
                                    await _oc.Create(preliminaryOrg);

                                    project.OrganizationId = preliminaryOrg.OrganizationId;
                                }

                                _context.Add(project); //This is an insert operation to the database. 
                            }
                            else { 
                                _context.Update(project); //This is therefore an update operation to the database, since the projectID is already existing. 
                            }
                            
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                                //So this checks whether the data given and binded to the Project-class instance is in order with what we have specified in the Model folder. 
                            
                        }
                        break;
                }

            }catch (RetryLimitExceededException) {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("dex", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            var p = ProjectWithLists();
            
            project.OrganizationList = p.OrganizationList;

            return View(project);
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ArticleHeadline,ArticleUrl,ArticleDescription,ArticleDate,Confidentiality,OrganizationId,Country,IndustryId,UseCaseId,Maturity,TechnicalVendor")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            //This line will just send you back to the main screen when you delete something
            return RedirectToAction(nameof(Index));
        }

        /*
        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        */

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }


        public Project ProjectWithLists()
        {
            Project p = new Project();
            p.OrganizationList = new OrganizationController(_context).GetOrgList();
            p.IndustryList = new IndustryController(_context).GetInduList();
            p.UsecaseList = new UsecaseController(_context).GetUsecList();

            return p;
        }

        //This will effectively let the user download an excel spreadsheet. 
        public IActionResult FileReport()
        {
            ExcelController _ec = new ExcelController(_hostingEnvironment);
            VirtualFileResult file = _ec.FileReport();
            return file;
        }




    }
}
