using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TR22Demo.EntityFramework;

namespace TR22Demo.Controllers
{
    [RoutePrefix("api/AppVersionMetadatas")]
    public class AppVersionMetadatasController : ApiController
    {
        private AppVersionDataModel db = new AppVersionDataModel();

        // GET: api/AppVersionMetadatas
        public IQueryable<AppVersionMetadata> GetAppVersionMetadatas()
        {
            return db.AppVersionMetadatas;
        }

        [Route("GetLatest")]
        public async Task<AppVersionMetadata> GetLatest()
        {
            return
                await db.AppVersionMetadatas
                    .OrderByDescending(x => x.PublishedTime)
                    .FirstAsync();
        }

        // GET: api/AppVersionMetadatas/5
        [ResponseType(typeof(AppVersionMetadata))]
        public async Task<IHttpActionResult> GetAppVersionMetadata(int id)
        {
            AppVersionMetadata appVersionMetadata = await db.AppVersionMetadatas.FindAsync(id);
            if (appVersionMetadata == null)
            {
                return NotFound();
            }

            return Ok(appVersionMetadata);
        }

        // PUT: api/AppVersionMetadatas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAppVersionMetadata(int id, AppVersionMetadata appVersionMetadata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appVersionMetadata.Id)
            {
                return BadRequest();
            }

            db.Entry(appVersionMetadata).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppVersionMetadataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AppVersionMetadatas
        [ResponseType(typeof(AppVersionMetadata))]
        public async Task<IHttpActionResult> PostAppVersionMetadata(AppVersionMetadata appVersionMetadata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AppVersionMetadatas.Add(appVersionMetadata);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = appVersionMetadata.Id }, appVersionMetadata);
        }

        // DELETE: api/AppVersionMetadatas/5
        [ResponseType(typeof(AppVersionMetadata))]
        public async Task<IHttpActionResult> DeleteAppVersionMetadata(int id)
        {
            AppVersionMetadata appVersionMetadata = await db.AppVersionMetadatas.FindAsync(id);
            if (appVersionMetadata == null)
            {
                return NotFound();
            }

            db.AppVersionMetadatas.Remove(appVersionMetadata);
            await db.SaveChangesAsync();

            return Ok(appVersionMetadata);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppVersionMetadataExists(int id)
        {
            return db.AppVersionMetadatas.Count(e => e.Id == id) > 0;
        }
    }
}