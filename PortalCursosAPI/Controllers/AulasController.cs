using PortalCursosAPI.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace PortalCursosAPI.Controllers
{
    public class AulasController : ApiController
    {
        private PortalCursosContext db = new PortalCursosContext();

        public IHttpActionResult GetAulas(int idCurso)
        {
            if (idCurso <= 0)
                return BadRequest("O id deve ser maior do que zero.");

            Curso curso = db.Cursos.Find(idCurso);

            // 404 - Curso não encontrado
            if (curso == null)
                return NotFound();

            return Ok(curso.Aulas.OrderBy(a => a.Ordem).ToList());
        }

        public IHttpActionResult GetAula(int idCurso, int ordemAula)
        {
            if (idCurso <= 0)
                return BadRequest("O id deve ser maior do que zero.");
            if (ordemAula <= 0)
                return BadRequest("A ordem deve ser maior do que zero.");

            Curso curso = db.Cursos.Find(idCurso);

            // 404 - Curso não encontrado
            if (curso == null)
                return NotFound();

            Aula aula = curso.Aulas.FirstOrDefault(a => a.Ordem == ordemAula);

            // 404 - Aula não encontrada
            if (aula == null)
                return NotFound();

            return Ok(aula);
        }

        public IHttpActionResult DeleteAula(int idCurso, int ordemAula)
        {
            if (idCurso <= 0)
                return BadRequest("O id deve ser maior do que zero.");
            if (ordemAula <= 0)
                return BadRequest("A ordem deve ser maior do que zero.");

            Curso curso = db.Cursos.Find(idCurso);

            // 404 - Curso não encontrado
            if (curso == null)
                return NotFound();

            Aula aula = curso.Aulas.FirstOrDefault(a => a.Ordem == ordemAula);

            // 404 - Aula não encontrada
            if (aula == null)
                return NotFound();

            db.Entry(aula).State = EntityState.Deleted;

            // atualiza ordem da lista ao remover uma aula
            curso.Aulas.Where(a => a.Ordem > ordemAula).ToList().ForEach(a => a.Ordem--);

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult PostAula(int idCurso, Aula aula)
        {
            if (idCurso <= 0)
                return BadRequest("O id deve ser maior do que zero.");

            Curso curso = db.Cursos.Find(idCurso);

            // 404 - Curso não encontrado
            if (curso == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (curso.Aulas.Count() > 0)
            {    // Calcula a ordem da próxima aula a ser cadastrada
                int proximaAula = curso.Aulas.Max(a => a.Ordem) + 1;

                if (aula.Ordem > proximaAula)
                    aula.Ordem = proximaAula;
                else if (aula.Ordem < proximaAula)
                    curso.Aulas.Where(a => a.Ordem >= aula.Ordem).ToList().ForEach(a => a.Ordem++);
            }
            else
            {
                aula.Ordem = 1;
            }
            aula.IdCurso = idCurso;

            db.Aulas.Add(aula);
            db.SaveChanges();

            return CreatedAtRoute("Aulas", new { idCurso = curso.Id, ordemAula = aula.Ordem }, aula);
        }

        public IHttpActionResult PutAula(int idCurso, int ordemAula, Aula aula)
        {
            if (idCurso <= 0)
                return BadRequest("O id deve ser maior do que zero.");
            if (ordemAula <= 0)
                return BadRequest("A ordem deve ser maior do que zero.");

            Curso curso = db.Cursos.Find(idCurso);

            // 404 - Curso não encontrado
            if (curso == null)
                return NotFound();

            Aula aulaAtual = curso.Aulas.FirstOrDefault(a => a.Ordem == ordemAula);

            // 404 - Aula não encontrada
            if (aulaAtual == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (aula.Ordem > ordemAula)
            {
                int ultimaAula = curso.Aulas.Max(a => a.Ordem);

                if (aula.Ordem > ultimaAula)
                    aula.Ordem = ultimaAula;

                curso.Aulas.Where(a => a.Ordem > ordemAula && a.Ordem <= aula.Ordem).ToList().ForEach(a => a.Ordem--);
            }
            else if (aula.Ordem < ordemAula)
            {
                curso.Aulas.Where(a => a.Ordem >= aula.Ordem && a.Ordem < ordemAula).ToList().ForEach(a => a.Ordem++);
            }

            aulaAtual.Titulo = aula.Titulo;
            aulaAtual.Ordem = aula.Ordem;

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
