using EduAvis.Backend.Model;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Linq.Expressions;

namespace EduAvis.Service
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly BdeduavisContext _context;
        private readonly DbSet<T> _dbSet;
        internal static Logger logger = LogManager.GetCurrentClassLogger();

        public GenericService(BdeduavisContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<bool> AddAsync(T entidad)
        {
            bool resultado = true;
            try
            {
                await _dbSet.AddAsync(entidad);
                await _context.SaveChangesAsync();
                logger.Info("Objeto " + entidad.GetType() + " insertado correctamente.");
            }
            catch (Exception ex)
            {
                // SOLUCIÓN 1: Limpiar el estado del contexto después de un error
                await HandleDatabaseError("Error al insertar la entidad.", ex, entidad);
                resultado = false;
            }
            return resultado;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            bool resultado = true;
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                logger.Info("Entidad actualizada con éxito.");
            }
            catch (Exception ex)
            {
                // SOLUCIÓN 1: Limpiar el estado del contexto después de un error
                await HandleDatabaseError("Error al actualizar la entidad.", ex, entity);
                resultado = false;
            }
            return resultado;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            bool resultado = true;
            try
            {
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                    logger.Info("Entidad borrada con éxito.");
                }
                else
                {
                    logger.Warn("Entidad no encontrada.");
                    resultado = false;
                }
            }
            catch (Exception ex)
            {
                await HandleDatabaseError("Error al borrar la entidad", ex, entity);
                resultado = false;
            }
            return resultado;
        }

        // MÉTODO PARA MANEJAR ERRORES Y LIMPIAR EL ESTADO
        private async Task HandleDatabaseError(string mensaje, Exception ex, T entity = null)
        {
            ErrorLog(mensaje, ex);

            // SOLUCIÓN 1A: Detach de la entidad específica que causó problemas
            if (entity != null)
            {
                var entry = _context.Entry(entity);
                if (entry != null)
                {
                    entry.State = EntityState.Detached;
                }
            }

            // SOLUCIÓN 1B: Limpiar todos los cambios pendientes
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        // MÉTODO ALTERNATIVO: Usar transacciones explícitas
        public async Task<bool> AddAsyncWithTransaction(T entidad)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _dbSet.AddAsync(entidad);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                logger.Info("Objeto " + entidad.GetType() + " insertado correctamente con transacción.");
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ErrorLog("Error al insertar la entidad con transacción.", ex);
                return false;
            }
        }

        public async Task<bool> UpdateAsyncWithTransaction(T entity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                logger.Info("Entidad actualizada con éxito con transacción.");
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ErrorLog("Error al actualizar la entidad con transacción.", ex);
                return false;
            }
        }
    
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet.ToListAsync();
                logger.Info("Entidades obtenidas correctamente.");
                return entities;
            }
            catch (Exception ex)
            {
                ErrorLog("Error al leer las entidades.", ex);
                return Enumerable.Empty<T>();
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    logger.Info("Entidad encontrada.");
                }
                else
                {
                    logger.Warn("Entidad no encontrada.");
                }
                return entity;
            }
            catch (Exception ex)
            {
                ErrorLog("Error al buscar la entidad.", ex);
                return null;
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entities = await _dbSet.Where(predicate).ToListAsync();
                logger.Info("Entidades filtradas con éxito.");
                return entities;
            }
            catch (Exception ex)
            {
                ErrorLog("Error al filtrar las entidades", ex);
                return Enumerable.Empty<T>();
            }
        }

        private void ErrorLog(string mensaje, Exception ex)
        {
            logger.Error(mensaje + "\n" + ex.Message);
            logger.Error(ex.InnerException);
            logger.Error(ex.StackTrace);
        }

        // MÉTODO PARA LIMPIAR MANUALMENTE EL CONTEXTO
        public void ClearContext()
        {
            _context.ChangeTracker.Clear();
        }
    }
}