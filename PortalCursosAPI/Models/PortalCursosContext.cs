using System.Data.Entity;

namespace PortalCursosAPI.Models
{
    public class PortalCursosContext : DbContext
    {
        public PortalCursosContext() : base("PortalCursosLocal")
        {
            //Database.Log = d => System.Diagnostics.Debug.WriteLine(d);
        }

        public DbSet<Curso> Cursos { get; set; }

        public DbSet<Aula> Aulas { get; set; }

    }
}