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
    public class AppVersionData
    {
        public string AppVersion { get; set; }
        public int NumberOfDevicesOnVersion { get; set; }
    }
    [RoutePrefix("api/IpsTelemtries")]
    public class IpsTelemetriesController : ApiController
    {
        private AppVersionDataModel db = new AppVersionDataModel();

        // GET: api/IpsTelemetries
        public IQueryable<IpsTelemetry> GetIpsTelemetries()
        {
            return db.IpsTelemetries;
        }

        [Route("IpsOnOldVersions")]
        public async Task<List<IpsTelemetry>> GetIpsOnOldVersions()
        {
            var latestAppV = await db.AppVersionMetadatas
                                .OrderByDescending(x => x.PublishedTime)
                                .FirstAsync();
            var groups = db.IpsTelemetries
                            .OrderByDescending(x => x.OriginationTimeStamp)
                            .GroupBy(x => x.FriendlyDeviceName);
            List<IpsTelemetry> telemetry = new List<IpsTelemetry>();
            foreach (var g in groups)
            {
                telemetry.Add(g.First());
            }
            var oldIps = telemetry.FindAll(x => x.CurrentAppVersion != latestAppV.Version);
            return oldIps;
        }

        [Route("GetDeviceAppVersions")]
        public async Task<List<AppVersionData>> GetDeviceAppVersions()
        {
            var groups = db.IpsTelemetries
                            .OrderByDescending(x => x.OriginationTimeStamp)
                            .GroupBy(x => x.FriendlyDeviceName);
            List<IpsTelemetry> t = new List<IpsTelemetry>();
            foreach(var g in groups)
            {
                t.Add(g.First());
            }
            var groups2 = t.GroupBy(x => x.CurrentAppVersion);                    
            List<AppVersionData> countAppVersion = new List<AppVersionData>();
            foreach (var g in groups2)
            {
                AppVersionData nItem = new AppVersionData();
                nItem.AppVersion = g.Key;
                nItem.NumberOfDevicesOnVersion = g.Count();
                countAppVersion.Add(nItem);
            }
            return countAppVersion;
        }

        // GET: api/IpsTelemetries/5
        [ResponseType(typeof(IpsTelemetry))]
        public async Task<IHttpActionResult> GetIpsTelemetry(int id)
        {
            IpsTelemetry ipsTelemetry = await db.IpsTelemetries.FindAsync(id);
            if (ipsTelemetry == null)
            {
                return NotFound();
            }

            return Ok(ipsTelemetry);
        }

        // PUT: api/IpsTelemetries/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutIpsTelemetry(int id, IpsTelemetry ipsTelemetry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ipsTelemetry.Id)
            {
                return BadRequest();
            }

            db.Entry(ipsTelemetry).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IpsTelemetryExists(id))
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

        // POST: api/IpsTelemetries
        [ResponseType(typeof(IpsTelemetry))]
        public async Task<IHttpActionResult> PostIpsTelemetry(IpsTelemetry ipsTelemetry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.IpsTelemetries.Add(ipsTelemetry);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ipsTelemetry.Id }, ipsTelemetry);
        }

        // DELETE: api/IpsTelemetries/5
        [ResponseType(typeof(IpsTelemetry))]
        public async Task<IHttpActionResult> DeleteIpsTelemetry(int id)
        {
            IpsTelemetry ipsTelemetry = await db.IpsTelemetries.FindAsync(id);
            if (ipsTelemetry == null)
            {
                return NotFound();
            }

            db.IpsTelemetries.Remove(ipsTelemetry);
            await db.SaveChangesAsync();

            return Ok(ipsTelemetry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IpsTelemetryExists(int id)
        {
            return db.IpsTelemetries.Count(e => e.Id == id) > 0;
        }
    }
}