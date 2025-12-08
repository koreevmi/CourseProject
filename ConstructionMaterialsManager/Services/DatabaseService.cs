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
            using (var context = new ApplicationDbContext())
            {
                context.Projects.Add(project);
                context.SaveChanges();
            }
        }

        public void UpdateProjectMaterial(ProjectMaterial projectMaterial)
        {
            using (var context = new ApplicationDbContext())
            {
                if (projectMaterial.UsedQuantity > projectMaterial.PlannedQuantity)
                {
                    throw new InvalidOperationException("Использованное количество не может быть больше запланированного.");
                }

                context.Entry(projectMaterial).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public void DeleteProject(int projectId)
        {
            using (var context = new ApplicationDbContext())
            {
                var project = context.Projects.Include(p => p.ProjectMaterials).FirstOrDefault(p => p.ProjectId == projectId);
                if (project != null)
                {
                    // Удаление связанных материалов проекта
                    context.ProjectMaterials.RemoveRange(project.ProjectMaterials);
                    // Удаление проекта
                    context.Projects.Remove(project);
                    context.SaveChanges();
                }
            }
        }


        public void AddDelivery(Delivery delivery)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var materialExists = context.Materials.Any(m => m.MaterialId == delivery.MaterialId);
                        if (!materialExists)
                        {
                            throw new InvalidOperationException($"Материал с ID {delivery.MaterialId} не найден.");
                        }

                        var supplierExists = context.Suppliers.Any(s => s.SupplierId == delivery.SupplierId);
                        if (!supplierExists)
                        {
                            throw new InvalidOperationException($"Поставщик с ID {delivery.SupplierId} не найден.");
                        }

                        if (delivery.Quantity <= 0)
                        {
                            throw new InvalidOperationException("Количество должно быть больше нуля.");
                        }

                        context.Deliveries.Add(delivery);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateDelivery(Delivery delivery)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var materialExists = context.Materials.Any(m => m.MaterialId == delivery.MaterialId);
                        if (!materialExists)
                        {
                            throw new InvalidOperationException($"Материал с ID {delivery.MaterialId} не найден.");
                        }

                        var supplierExists = context.Suppliers.Any(s => s.SupplierId == delivery.SupplierId);
                        if (!supplierExists)
                        {
                            throw new InvalidOperationException($"Поставщик с ID {delivery.SupplierId} не найден.");
                        }

                        if (delivery.Quantity <= 0)
                        {
                            throw new InvalidOperationException("Количество должно быть больше нуля.");
                        }

                        context.Entry(delivery).State = EntityState.Modified;
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
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
            using (var context = new ApplicationDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var materialExists = context.Materials.Any(m => m.MaterialId == movement.MaterialId);
                        if (!materialExists)
                        {
                            throw new InvalidOperationException($"Материал с ID {movement.MaterialId} не найден.");
                        }

                        if (movement.Quantity <= 0)
                        {
                            throw new InvalidOperationException("Количество должно быть больше нуля.");
                        }

                        context.MaterialMovements.Add(movement);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void AddProjectMaterial(ProjectMaterial projectMaterial)
        {
            using (var context = new ApplicationDbContext())
            {
                var projectExists = context.Projects.Any(p => p.ProjectId == projectMaterial.ProjectId);
                if (!projectExists)
                {
                    throw new InvalidOperationException($"Проект с ID {projectMaterial.ProjectId} не найден.");
                }

                var materialExists = context.Materials.Any(m => m.MaterialId == projectMaterial.MaterialId);
                if (!materialExists)
                {
                    throw new InvalidOperationException($"Материал с ID {projectMaterial.MaterialId} не найден.");
                }

                if (projectMaterial.UsedQuantity > projectMaterial.PlannedQuantity)
                {
                    throw new InvalidOperationException("Использованное количество не может быть больше запланированного.");
                }

                context.ProjectMaterials.Add(projectMaterial);
                context.SaveChanges();
            }
        }


        public void RemoveProjectMaterial(int projectMaterialId)
        {
            using (var context = new ApplicationDbContext())
            {
                var projectMaterial = context.ProjectMaterials.Find(projectMaterialId);
                if (projectMaterial != null)
                {
                    context.ProjectMaterials.Remove(projectMaterial);
                    context.SaveChanges();
                }
            }
        }

        public void UpdateProject(Project project)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(project).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
