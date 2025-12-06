using ConstructionMaterialsManager.Data;
using ConstructionMaterialsManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionMaterialsManager.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ApplicationDbContext _context;

        public DatabaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetUserByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public List<Material> GetMaterials()
        {
            return _context.Materials.Include(m => m.Supplier).ToList();
        }

        public List<Supplier> GetSuppliers()
        {
            return _context.Suppliers.ToList();
        }

        public List<Project> GetProjects()
        {
            return _context.Projects.ToList();
        }

        public List<Delivery> GetDeliveries()
        {
            return _context.Deliveries.Include(d => d.Material).Include(d => d.Supplier).ToList();
        }

        public List<MaterialMovement> GetMaterialMovements()
        {
            return _context.MaterialMovements.Include(mm => mm.Material).ToList();
        }

        public List<ProjectMaterial> GetProjectMaterials(int projectId)
        {
            return _context.ProjectMaterials
                .Include(pm => pm.Material)
                .Where(pm => pm.ProjectId == projectId)
                .ToList();
        }

        public void AddMaterial(Material material)
        {
            _context.Materials.Add(material);
            _context.SaveChanges();
        }

        public void UpdateMaterial(Material material)
        {
            _context.Materials.Update(material);
            _context.SaveChanges();
        }

        public void DeleteMaterial(int materialId)
        {
            var material = _context.Materials.Find(materialId);
            if (material != null)
            {
                _context.Materials.Remove(material);
                _context.SaveChanges();
            }
        }

        public void AddSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            _context.SaveChanges();
        }

        public void DeleteSupplier(int supplierId)
        {
            var supplier = _context.Suppliers.Find(supplierId);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
        }

        public void AddProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
        }

        public void UpdateProject(Project project)
        {
            _context.Projects.Update(project);
            _context.SaveChanges();
        }

        public void DeleteProject(int projectId)
        {
            var project = _context.Projects.Find(projectId);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
        }

        public void AddDelivery(Delivery delivery)
        {
            _context.Deliveries.Add(delivery);
            _context.SaveChanges();
        }

        public void UpdateDelivery(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            _context.SaveChanges();
        }

        public void DeleteDelivery(int deliveryId)
        {
            var delivery = _context.Deliveries.Find(deliveryId);
            if (delivery != null)
            {
                _context.Deliveries.Remove(delivery);
                _context.SaveChanges();
            }
        }

        public void AddMaterialMovement(MaterialMovement movement)
        {
            _context.MaterialMovements.Add(movement);
            _context.SaveChanges();
        }

        public void AddProjectMaterial(ProjectMaterial projectMaterial)
        {
            _context.ProjectMaterials.Add(projectMaterial);
            _context.SaveChanges();
        }

        public void RemoveProjectMaterial(int projectMaterialId)
        {
            var projectMaterial = _context.ProjectMaterials.Find(projectMaterialId);
            if (projectMaterial != null)
            {
                _context.ProjectMaterials.Remove(projectMaterial);
                _context.SaveChanges();
            }
        }
    }
}
