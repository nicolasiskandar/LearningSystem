using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LearningSystemDbContext _context;

        public CategoryRepository(LearningSystemDbContext context)
        {
            _context = context;
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public ICollection<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category? GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public Category? GetCategoryByName(string name)
        {
            return _context.Categories.FirstOrDefault(c => c.Name == name);
        }

        public void RemoveCategory(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
    }
}
