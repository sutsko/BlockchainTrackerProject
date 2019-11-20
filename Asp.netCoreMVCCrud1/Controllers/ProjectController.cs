using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.netCoreMVCCrud1.Models;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading;
using ChartJSCore.Helpers;
using ChartJSCore.Models;

namespace Asp.netCoreMVCCrud1.Controllers
{
    public class ProjectController : Controller
    {
        private  ProjectContext _context;
        private IndustryController _ic;
        private ChartController _cc;
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
            _cc = new ChartController();
    }

        [RequireHttps]
        // GET: Project
        public async Task<IActionResult> Index()
        {
            //1. Hente hele listen af alle projekter
           List <Project> listOfProjects = await _context.Projects.ToListAsync();

            listOfProjects = mappedProjects(listOfProjects);

            return View(listOfProjects);
        }

        [RequireHttps]
        // GET: Project
        public async Task<IActionResult> DataAnalysis()
        {
            //1. Hente hele listen af alle projekter
            List<Project> listOfProjects = await _context.Projects.ToListAsync();

            listOfProjects = mappedProjects(listOfProjects);

            return _cc.Index(listOfProjects);
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

            List<Project> listForMapPurpose = new List<Project>();
            listForMapPurpose.Add(project);
            listForMapPurpose = mappedProjects(listForMapPurpose);
            project = listForMapPurpose[0];



            return View(project);
        }

        // GET: Project/AddOrEdit
        //When the asp-action="AddOrEdit" in index.cshtml is called on the client side, this function will be run
        public IActionResult AddOrEdit(int id = 0)
        {
            

            if (id == 0)
            {
                //https://stackoverflow.com/questions/31647259/populate-dropdownlist-in-mvc-5
                var p = ProjectWithLists();

                p.ArticleDate = DateTime.Today;

                //This will return the view "addOrEdit" from the folder Views--> Project --> AddOrEdit
                return View(p);
            }
            else {
            
                Project project_from_id = listAndMapSingleProject(_context.Projects.Find(id));

                //this function will ask the database to find the project with the corresponding id. 
                return View(project_from_id); 
            }
                
        }

        private Project listAndMapSingleProject(Project project_from_id)
        {
            var p = ProjectWithLists();

            project_from_id.IndustryList = p.IndustryList;
            project_from_id.OrganizationList = p.OrganizationList;
            project_from_id.UsecaseList = p.UsecaseList;

            List<Project> listForMapPurpose = new List<Project>();
            listForMapPurpose.Add(project_from_id);
            listForMapPurpose = mappedProjects(listForMapPurpose);
            project_from_id = listForMapPurpose[0];

            return (project_from_id);
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
                                project.Organization = new Organization();
                                project.Organization.OrganizationId = project.OrganizationId;
                                project.Organization.OrganizationName = myOrganization;
                                project.Organization.OrganizationType = 0;

                                _context.Organizations.Update(project.Organization);
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


        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return RedirectToAction(nameof(Index));
                //INsert some alert here, saying there was no file
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction(nameof(Index));
                //INsert some alert here, syaing that the format is wrong. 
            }

            var list = new List<Project>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        long dateNum = long.Parse(worksheet.Cells[row, 4].Value.ToString());
                        DateTime result = DateTime.FromOADate(dateNum);
                       
                        list.Add(new Project
                        {
                            ArticleHeadline = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            ArticleUrl = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            ArticleDescription = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            ArticleDate = result,
                            Organization = new Organization(worksheet.Cells[row, 5].Value.ToString().Trim()),
                            Country = worksheet.Cells[row, 6].Value.ToString().Trim(),
                            Industry = new Industry(worksheet.Cells[row, 7].Value.ToString().Trim()),
                            Usecase = new Usecase(worksheet.Cells[row, 8].Value.ToString().Trim()),
                            Maturity = worksheet.Cells[row, 9].Value.ToString().Trim(),
                            TechnicalVendor = worksheet.Cells[row, 10].Value.ToString().Trim(),
                        });
                    }

                }
            }

            List<Project> revisedProjectList = await mappedProjectsNameIsKey(list);
            if (revisedProjectList == null)
            {
                return RedirectToAction(nameof(Index));
                //make some error statement to ui . LATER
            } else
            {
                foreach (Project p in revisedProjectList)
                {
                    p.Industry = null;
                    p.Organization = null;
                    p.Usecase = null;
                        _context.Add(p);
                    
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> FileReportAsync()
        {
            //1. Hente hele listen af alle projekter og map de tilhørende organization, industry og usecase
            List<Project> listOfProjects = await _context.Projects.ToListAsync();
            listOfProjects = mappedProjects(listOfProjects);

            //Send the list to the excelcontroller to create the package
            ExcelController _ec = new ExcelController(_hostingEnvironment);
            FileContentResult file = _ec.FileReport(listOfProjects); //Remove listofprojects to make it different
            
            return file;
        }

        public List<Project> mappedProjects(List<Project> listOfProjects)
        {
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
            foreach (Project p in listOfProjects)
            {
                p.Organization.OrganizationName = organizationMap[p.OrganizationId];
                p.Industry.IndustryName = industryMap[p.IndustryId];
                p.Usecase.UsecaseName = usecaseMap[p.UseCaseId];
            }

            return listOfProjects;
        }

        public async Task<List<Project>> mappedProjectsNameIsKey(List<Project> listOfProjects)
        {
            //1. Hente hele listen af usecases, organisationer og industrier  
            List<Organization> listOfOrganizations = _oc.GetOrgList();
            List<Usecase> listOfUsecases = _uc.GetUsecList();
            List<Industry> listOfIndustries = _ic.GetInduList();

            //3. Lave mappings over dem. Navnet på en org, ind, eller use skal være map-key, hvor id'et er valu.  
            Dictionary<string, int> organizationMap = new Dictionary<string, int>();
            Dictionary<string, int> usecaseMap = new Dictionary<string, int>();
            Dictionary<string, int> industryMap = new Dictionary<string, int>();

            foreach (Organization o in listOfOrganizations)
            {
                if (!organizationMap.TryGetValue(o.OrganizationName, out int indResult))
                {
                    organizationMap.Add(o.OrganizationName, o.OrganizationId);
                }

            }

            foreach (Usecase uc in listOfUsecases)
            {

                if (!usecaseMap.TryGetValue(uc.UsecaseName, out int indResult))
                {
                    usecaseMap.Add(uc.UsecaseName, uc.UsecaseId);
                }

            }

            foreach (Industry i in listOfIndustries)
            {

                if (!industryMap.TryGetValue(i.IndustryName, out int indResult))
                {
                    industryMap.Add(i.IndustryName, i.IndustryId);
                }
            }

            //4. På baggrund af nøglen, der skal være navnet, tilføjes for each til projekterne.   
            foreach (Project p in listOfProjects)
            {
                if (organizationMap.TryGetValue(p.Organization.OrganizationName, out int orgResult))
                {
                    p.OrganizationId = orgResult;
                }
                else
                {
                    //Adding the organization to the database, and then seeting the id of that organization to the 
                    Organization preliminaryOrg = new Organization();
                    preliminaryOrg.OrganizationName = p.Organization.OrganizationName;
                    preliminaryOrg.OrganizationType = 1;
                    await _oc.Create(preliminaryOrg);

                    p.OrganizationId = preliminaryOrg.OrganizationId;
                }

                if (industryMap.TryGetValue(p.Industry.IndustryName, out int indResult))
                {
                    p.IndustryId = indResult;
                }
                else
                {
                    Console.WriteLine("Could not find the specified key. Please verify that the industry name is an existing one. It must be completely accurate");
                    return null;
                }

                if (usecaseMap.TryGetValue(p.Usecase.UsecaseName, out int useResult))
                {
                    p.UseCaseId = useResult;
                }
                else
                {
                    Console.WriteLine("Could not find the specified key. Please verify that the usecase name is an existing one. It must be completely accurate");
                    return null;
                }
            }

            return listOfProjects;
        }



    }
}
