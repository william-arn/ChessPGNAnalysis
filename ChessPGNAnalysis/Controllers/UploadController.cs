using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PGNLibrary;

namespace ChessPGNAnalysis.Controllers
{
    [Authorize]
    public class UploadController : ApiController
    {
        public async Task<HttpResponseMessage> Post(HttpRequestMessage req)
        {
            var content = await req.Content.ReadAsStringAsync();
            List<ChessGame> games;
            try
            {
                games = PGNFunctions.Deserialize(content);
            }
            catch (InvalidOperationException ex)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return req.CreateResponse(HttpStatusCode.OK,$"Parsed file contained {games.Count} games.");
        }
    }
}
