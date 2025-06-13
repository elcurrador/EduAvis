
using EduAvis.Service;
using NLog;

namespace di.proyecto.clase._2024.MVVM.Base
{
    public class MVBaseCRUD<T> : MVBase
        where T : class
    {
        public GenericService<T> service { get; set; }
        private static Logger log = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Realiza una inserción en la base de datos y captura la excepción
        /// </summary>
        /// <param name="entity">Objeto a guardar</param>
        /// <returns></returns>
        public async Task<bool> Add(T entity)
        {

            return await service.AddAsync(entity);

        }
        /// <summary>
        /// Realiza una actualización de una tupla de la base de datos
        /// </summary>
        /// <param name="entity">Objeto que se actualiza</param>
        /// <returns></returns>
        public async Task<bool> Update(T entity)
        {

            return await service.UpdateAsync(entity);
        }
        /// <summary>
        /// Borra una fila de la tabla correspondiente
        /// </summary>
        /// <param name="entity">Objeto que se borra</param>
        /// <returns></returns>
        public async Task<bool> Delete(T entity)
        {       
             return  await service.DeleteAsync(entity);
       
        }
    }
}
