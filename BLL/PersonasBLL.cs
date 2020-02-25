using RegistroConDetalle.DAL;
using RegistroConDetalle.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;

namespace RegistroConDetalle.BLL
{
    public class PersonasBLL
    {
        public static bool Guardar(Personas persona)
        {
            bool paso = false;
            Contexto db = new Contexto();
            try
            {
                if (db.Personas.Add(persona) != null)
                    paso = db.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }

            return paso;
            
        }

        public static bool Modificar(Personas persona)
        {
            bool paso = false;
            Contexto db = new Contexto();
            
            try
            {
                var Anterior = db.Personas.Find(persona.PersonaId);
                foreach(var item in Anterior.Telefonos)
                {
                    if (!persona.Telefonos.Exists(d => d.Id == item.Id))
                        db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
                db.Entry(persona).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                paso = (db.SaveChanges() > 0);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }

            return paso;
        }

        public static bool Eliminar(int id)
        {
            bool paso = false;
            Contexto db = new Contexto();

            try
            {
                var eliminar = db.Personas.Find(id);
                db.Entry(eliminar).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                paso = (db.SaveChanges() > 0);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }

            return paso;
        }

        public static Personas Buscar(int id)
        {
            Contexto db = new Contexto();
            Personas persona = new Personas();
            try
            {
                persona = db.Personas.Find(id);
                persona.Telefonos.Count();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }

            return persona;
        }

        public static List<Personas> GetList(Expression<Func<Personas, bool>> persona)
        {
            List<Personas> Lista = new List<Personas>();
            Contexto db = new Contexto();

            try
            {
                Lista = db.Personas.Where(persona).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }

            return Lista;
        }
    }
}
